// FFXIVAPP.Client ~ NetworkWorker.cs
// 
// Copyright © 2007 - 2017 Ryan Wilson - All Rights Reserved
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Helpers;
using NetFwTypeLib;
using NLog;

namespace FFXIVAPP.Client.Network
{
    internal class NetworkWorker : INotifyPropertyChanged, IDisposable
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
        }

        #endregion

        #region Property Bindings

        #endregion

        #region Declarations

        private List<ServerConnection> DroppedConnections = new List<ServerConnection>();
        private object Lock = new object();
        private List<ServerConnection> ServerConnections = new List<ServerConnection>();
        private List<SocketObject> Sockets = new List<SocketObject>();

        #endregion

        #region Timer Controls

        /// <summary>
        /// </summary>
        public void StartScanning()
        {
            var interfaces = GetNetworkInterfaces();
            foreach (var item in interfaces.Where(x => !String.IsNullOrWhiteSpace(x)))
            {
                Sockets.Add(new SocketObject
                {
                    IPAddress = item
                });
            }
            UpdateConnectionList();

            ValidateNetworkAccess();

            if (Settings.Default.NetworkUseWinPCap)
            {
                // ISSUE: method pointer
                WinPcapWrapper.DataReceived += WinPcapWrapper_DataReceived;
            }

            foreach (var stateObject in Sockets)
            {
                try
                {
                    if (Settings.Default.NetworkUseWinPCap)
                    {
                        var allDevices = WinPcapWrapper.GetAllDevices();
                        stateObject.device = allDevices.FirstOrDefault(x => x.Addresses.Contains(stateObject.IPAddress));
                        if (!string.IsNullOrWhiteSpace(stateObject.device.Name))
                        {
                            WinPcapWrapper.StartCapture(stateObject);
                        }
                    }
                    else
                    {
                        stateObject.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
                        stateObject.Socket.Bind(new IPEndPoint(IPAddress.Parse(stateObject.IPAddress), 0));
                        stateObject.Socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AcceptConnection, true);
                        var inFlags = new byte[]
                        {
                            1, 0, 0, 0
                        };
                        var outFlags = new byte[4];
                        stateObject.Socket.IOControl(IOControlCode.ReceiveAll, inFlags, outFlags);
                        stateObject.Socket.ReceiveBufferSize = 0x7D000;
                        stateObject.Socket.BeginReceive(stateObject.Buffer, 0, stateObject.Buffer.Length, SocketFlags.None, OnReceive, stateObject);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        /// <summary>
        /// </summary>
        public void StopScanning()
        {
            foreach (var stateObject in Sockets)
            {
                try
                {
                    if (stateObject == null)
                    {
                        continue;
                    }
                    if (stateObject.Socket != null)
                    {
                        stateObject.Socket.Shutdown(SocketShutdown.Both);
                        stateObject.Socket.Close();
                        stateObject.Socket.Dispose();
                        stateObject.Socket = null;
                    }
                    lock (stateObject.SocketLock)
                    {
                        stateObject.Connections = new List<NetworkConnection>();
                    }
                }
                catch (Exception ex)
                {
                }
            }
            Sockets.Clear();
            ServerConnections.Clear();
            DroppedConnections.Clear();
        }

        #endregion

        #region Threads

        private void WinPcapWrapper_DataReceived(object sender, WinPcapWrapper.DataReceivedEventArgs e)
        {
            try
            {
                ParseData(e.Device.State, e.Data, e.Data.Length);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error processing Network Data.", ex);
            }
        }


        private object ReceiveLock = new object();

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                var asyncState = (SocketObject) ar.AsyncState;
                int nReceived;
                var newBuffer = new byte[0x20000];
                var lastBuffer = newBuffer;
                try
                {
                    nReceived = asyncState.Socket.EndReceive(ar);
                }
                catch (Exception ex)
                {
                    nReceived = 0;
                }

                // swap buffers and begin receiving again ASAP, so we don't miss any packets
                lastBuffer = asyncState.Buffer;
                asyncState.Buffer = newBuffer;
                asyncState.Socket.BeginReceive(asyncState.Buffer, 0, asyncState.Buffer.Length, SocketFlags.None, OnReceive, asyncState);

                if (nReceived > 0)
                {
                    try
                    {
                        lock (ReceiveLock)
                        {
                            DispatcherHelper.Invoke(() => ParseData(asyncState, lastBuffer, nReceived));
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
            }
        }

        #region Parsing

        private void ParseData(SocketObject asyncState, byte[] byteData, int nReceived)
        {
            if ((byteData == null) || (byteData[9] != 6))
            {
                return;
            }
            var startIndex = (byte) ((byteData[0] & 15) * 4);
            var lengthCheck = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(byteData, 2));
            if ((nReceived < lengthCheck) || (startIndex > lengthCheck))
            {
                return;
            }
            var IP = new IPHeader(byteData, nReceived);
            var TCP = new TCPHeader(byteData, nReceived);
            var serverConnection = new ServerConnection
            {
                SourceAddress = (uint) BitConverter.ToInt32(byteData, 12),
                DestinationAddress = (uint) BitConverter.ToInt32(byteData, 16),
                SourcePort = (ushort) BitConverter.ToInt16(byteData, startIndex),
                DestinationPort = (ushort) BitConverter.ToInt16(byteData, startIndex + 2),
                TimeStamp = DateTime.Now
                /*
                    // these don't return the right ports for some reason
                    DestinationAddress = BitConverter.ToUInt32(IP.DestinationAddress.GetAddressBytes(), 0),
                    DestinationPort = Convert.ToUInt16(TCP.DestinationPort),
                    SourcePort = Convert.ToUInt16(TCP.SourcePort),
                    SourceAddress = BitConverter.ToUInt32(IP.SourceAddress.GetAddressBytes(), 0),
                    TimeStamp = DateTime.Now
                 */
            };
            lock (Lock)
            {
                var found = Enumerable.Contains(ServerConnections, serverConnection);
                if (!found)
                {
                    if (Enumerable.Contains(DroppedConnections, serverConnection))
                    {
                        return;
                    }
                    UpdateConnectionList();
                    if (!Enumerable.Contains(ServerConnections, serverConnection))
                    {
                        DroppedConnections.Add(serverConnection);
                        return;
                    }
                }
            }
            if ((startIndex + 12) > nReceived)
            {
                return;
            }
            var nextTCPSequence = (uint) IPAddress.NetworkToHostOrder(BitConverter.ToInt32(byteData, startIndex + 4));
            var cut = (byte) (((byteData[startIndex + 12] & 240) >> 4) * 4);
            var length = (nReceived - startIndex) - cut;
            if ((length < 0) || (length > 0x10000))
            {
                return;
            }

            if (lengthCheck == startIndex + cut)
            {
                return;
            }

            lock (asyncState.SocketLock)
            {
                var connection = asyncState.Connections.FirstOrDefault(x => x.Equals(serverConnection));
                if (connection == null)
                {
                    connection = new NetworkConnection
                    {
                        SourceAddress = serverConnection.SourceAddress,
                        SourcePort = serverConnection.SourcePort,
                        DestinationAddress = serverConnection.DestinationAddress,
                        DestinationPort = serverConnection.DestinationPort
                    };
                    asyncState.Connections.Add(connection);
                }
                if (length == 0)
                {
                    return;
                }
                var destinationBuffer = new byte[length];
                Array.Copy(byteData, startIndex + cut, destinationBuffer, 0, length);
                if (connection.StalePackets.ContainsKey(nextTCPSequence))
                {
                    connection.StalePackets.Remove(nextTCPSequence);
                }
                var packet = new NetworkPacket
                {
                    TCPSequence = nextTCPSequence,
                    Buffer = destinationBuffer,
                    Push = (byteData[startIndex + 13] & 8) != 0
                };
                connection.StalePackets.Add(nextTCPSequence, packet);


                if (!connection.NextTCPSequence.HasValue)
                {
                    connection.NextTCPSequence = nextTCPSequence;
                }
                if (connection.StalePackets.Count == 1)
                {
                    connection.LastGoodNetworkPacketTime = DateTime.Now;
                }

                if (!connection.StalePackets.Any(x => (x.Key <= connection.NextTCPSequence.Value)))
                {
                    if (DateTime.Now.Subtract(connection.LastGoodNetworkPacketTime)
                                .TotalSeconds <= 10.0)
                    {
                        return;
                    }
                    connection.NextTCPSequence = connection.StalePackets.Min(x => x.Key);
                }
                while (connection.StalePackets.Any(x => x.Key <= connection.NextTCPSequence.Value))
                {
                    NetworkPacket stalePacket;
                    uint sequenceLength = 0;
                    if (connection.StalePackets.ContainsKey(connection.NextTCPSequence.Value))
                    {
                        stalePacket = connection.StalePackets[connection.NextTCPSequence.Value];
                    }
                    else
                    {
                        stalePacket = (connection.StalePackets.Where(x => x.Key <= connection.NextTCPSequence.Value)
                                                 .OrderBy(x => x.Key)).FirstOrDefault()
                                                                      .Value;
                        sequenceLength = connection.NextTCPSequence.Value - stalePacket.TCPSequence;
                    }
                    connection.StalePackets.Remove(stalePacket.TCPSequence);
                    if (connection.NetworkBufferPosition == 0)
                    {
                        connection.LastNetworkBufferUpdate = DateTime.Now;
                    }
                    if (sequenceLength >= stalePacket.Buffer.Length)
                    {
                        continue;
                    }
                    connection.NextTCPSequence = stalePacket.TCPSequence + ((uint) stalePacket.Buffer.Length);
                    Array.Copy(stalePacket.Buffer, sequenceLength, connection.NetworkBuffer, connection.NetworkBufferPosition, stalePacket.Buffer.Length - sequenceLength);
                    connection.NetworkBufferPosition += stalePacket.Buffer.Length - ((int) sequenceLength);
                    if (stalePacket.Push)
                    {
                        ProcessNetworkBuffer(connection);
                    }
                }
            }
        }

        #endregion

        #region Processing

        private void ProcessNetworkBuffer(NetworkConnection connection)
        {
            while (connection.NetworkBufferPosition >= 0x1C)
            {
                uint bufferSize = 0;
                byte[] destinationArray;
                lock (connection.NetworkBufferLock)
                {
                    var indexes = new List<uint>
                    {
                        BitConverter.ToUInt32(connection.NetworkBuffer, 0),
                        BitConverter.ToUInt32(connection.NetworkBuffer, 4),
                        BitConverter.ToUInt32(connection.NetworkBuffer, 8),
                        BitConverter.ToUInt32(connection.NetworkBuffer, 12)
                    };
                    if ((indexes[0] != 0x41A05252) && ((indexes.Any(x => x != 0))))
                    {
                        AdjustNetworkBuffer(connection);
                        return;
                    }
                    bufferSize = BitConverter.ToUInt32(connection.NetworkBuffer, 0x18);
                    if ((bufferSize == 0) || (bufferSize > 0x10000))
                    {
                        AdjustNetworkBuffer(connection);
                        return;
                    }
                    if (connection.NetworkBufferPosition < bufferSize)
                    {
                        if (DateTime.Now.Subtract(connection.LastNetworkBufferUpdate)
                                    .Seconds > 5)
                        {
                            AdjustNetworkBuffer(connection);
                        }
                        break;
                    }
                    destinationArray = new byte[bufferSize];
                    Array.Copy(connection.NetworkBuffer, destinationArray, bufferSize);
                    Array.Copy(connection.NetworkBuffer, bufferSize, connection.NetworkBuffer, 0L, connection.NetworkBufferPosition - bufferSize);
                    connection.NetworkBufferPosition -= (int) bufferSize;
                    connection.LastNetworkBufferUpdate = DateTime.Now;
                }
                if (bufferSize <= 40)
                {
                    return;
                }
                var timeDifference = BitConverter.ToUInt64(destinationArray, 0x10);
                var time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(timeDifference)
                                                                              .ToLocalTime();
                int limiter = BitConverter.ToInt16(destinationArray, 30);
                int encoding = BitConverter.ToInt16(destinationArray, 0x20);
                var bytes = new byte[0x10000];
                int messageLength;
                switch (encoding)
                {
                    case 0:
                    case 1:
                        messageLength = ((int) bufferSize) - 40;
                        for (var i = 0; i < ((bufferSize / 4) - 10); i++)
                        {
                            Array.Copy(BitConverter.GetBytes(BitConverter.ToUInt32(destinationArray, (i * 4) + 40)), 0, bytes, i * 4, 4);
                        }
                        break;
                    default:
                        try
                        {
                            using (var decompressedStream = new DeflateStream(new MemoryStream(destinationArray, 0x2A, destinationArray.Length - 0x2A), CompressionMode.Decompress))
                            {
                                messageLength = decompressedStream.Read(bytes, 0, bytes.Length);
                            }
                        }
                        catch (Exception ex)
                        {
                            return;
                        }
                        break;
                }
                var position = 0;
                try
                {
                    for (var i = 0; i < limiter; i++)
                    {
                        if ((position + 4) > messageLength)
                        {
                            return;
                        }
                        var messageSize = BitConverter.ToUInt32(bytes, position);
                        if ((position + messageSize) > messageLength)
                        {
                            return;
                        }
                        if (messageSize > 0x18)
                        {
                            var networkPacket = new Common.Core.Network.NetworkPacket
                            {
                                Key = BitConverter.ToUInt32(bytes, position + 0x10),
                                Buffer = bytes,
                                CurrentPosition = position,
                                MessageSize = (int) messageSize,
                                PacketDate = time
                            };
                            AppContextHelper.Instance.RaiseNewPacket(networkPacket);
                        }
                        position += (int) messageSize;
                    }
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }

        #endregion

        #endregion

        #region Functions

        public bool ValidateNetworkAccess()
        {
            if (Sockets == null || !Sockets.Any())
            {
                return false;
            }
            try
            {
                var firewallWrapper = new FirewallWrapper();
                if (firewallWrapper.IsFirewallDisabled())
                {
                    return true;
                }

                firewallWrapper.AddFirewallApplicationEntry();

                if (firewallWrapper.IsFirewallApplicationConfigured())
                {
                    if (firewallWrapper.IsFirewallRuleConfigured())
                    {
                        return true;
                    }
                }
                var num = (int) MessageBox.Show("Unable to access network data due to Windows Firewall.  Please disable, or add a TCP rule for FFXIVAPP.Client.");
                return false;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("800706D9"))
                {
                    MessageBox.Show("Cannot determine Windows Firewall Status.");
                    return true;
                }
                MessageBox.Show("Error validating firewall: " + ex.Message);
                return false;
            }
        }


        internal class FirewallWrapper
        {
            private const string appName = "FFXIVAPP.Client";

            internal bool IsFirewallDisabled()
            {
                var netFwMgr = Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwMgr")) as INetFwMgr;
                return netFwMgr != null && !netFwMgr.LocalPolicy.CurrentProfile.FirewallEnabled;
            }

            internal bool IsFirewallApplicationConfigured()
            {
                var flag = false;
                var netFwMgr = Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwMgr")) as INetFwMgr;
                if (netFwMgr == null)
                {
                    return false;
                }
                var enumerator = netFwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications.GetEnumerator();
                if (enumerator == null)
                {
                    return false;
                }
                while (enumerator.MoveNext() && !flag)
                {
                    var authorizedApplication = enumerator.Current as INetFwAuthorizedApplication;
                    if (authorizedApplication != null && authorizedApplication.Name == appName && authorizedApplication.Enabled)
                    {
                        flag = true;
                    }
                }
                return flag;
            }

            internal bool IsFirewallRuleConfigured()
            {
                var flag = false;
                var netFwPolicy2 = Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2")) as INetFwPolicy2;
                if (netFwPolicy2 == null)
                {
                    return false;
                }
                var enumerator = netFwPolicy2.Rules.GetEnumerator();
                if (enumerator == null)
                {
                    return false;
                }
                while (enumerator.MoveNext() && !flag)
                {
                    var netFwRule2 = enumerator.Current as INetFwRule2;
                    if (netFwRule2 != null && netFwRule2.Name == appName && (netFwRule2.Enabled && netFwRule2.Protocol == 6))
                    {
                        flag = true;
                    }
                }
                return flag;
            }

            internal void AddFirewallApplicationEntry()
            {
                try
                {
                    var netFwMgr = Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwMgr")) as INetFwMgr;
                    if (netFwMgr == null)
                    {
                        throw new ApplicationException("Unable to connect to fireall.");
                    }
                    var app = Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwAuthorizedApplication")) as INetFwAuthorizedApplication;
                    if (app == null)
                    {
                        throw new ApplicationException("Unable to create new Firewall Application reference.");
                    }
                    app.Enabled = true;
                    app.IpVersion = NET_FW_IP_VERSION_.NET_FW_IP_VERSION_ANY;
                    app.Name = appName;
                    app.ProcessImageFileName = Application.ExecutablePath;
                    app.Scope = NET_FW_SCOPE_.NET_FW_SCOPE_ALL;

                    netFwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications.Add(app);
                }
                catch (Exception ex)
                {
                }
            }
        }


        private IEnumerable<string> GetNetworkInterfaces()
        {
            var source = new List<string>();
            foreach (var networkInterface in App.AvailableNetworkInterfaces.Where(i => i.Name == Settings.Default.DefaultNetworkInterface))
            {
                using (var enumerator = (networkInterface.GetIPProperties()
                                                         .UnicastAddresses.Select(x => x.Address.ToString())).GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        var ip = enumerator.Current;
                        if (((ip.Length <= 15) && ip.Contains('.')) && !source.Any(x => (x == ip)))
                        {
                            source.Add(ip);
                        }
                    }
                }
            }
            return source;
        }

        private void UpdateConnectionList()
        {
            var serverIPList = GetXIVServerIPList();
            var toAdd = new List<ServerConnection>();
            if (serverIPList != null)
            {
                if (ServerConnections.Any())
                {
                    foreach (var server in serverIPList)
                    {
                        var serverIP = server;
                        //foreach (var connection in ServerConnections.Where(connection => !serverIP.Equals(connection)))

                        if (!ServerConnections.Any(connection => !serverIP.Equals(connection)))
                        {
                            ServerConnections.Add(server);

                            //toAdd.Add(server);
                        }
                    }
                    /*
                    foreach (var server in toAdd)
                    {
                        ServerConnections.Add(server);
                    }
                     */
                }
                else
                {
                    ServerConnections = serverIPList;
                }
            }
        }

        private static List<ServerConnection> GetXIVServerIPList()
        {
            var list1 = new List<ServerConnection>();

            var id = Constants.ProcessModel.ProcessID;
            var num1 = IntPtr.Zero;
            var dwOutBufLen = 0;
            var error = 0;
            var num2 = IntPtr.Zero;
            for (var index = 0; index < 5; ++index)
            {
                error = (int) UnsafeNativeMethods.GetExtendedTcpTable(num1, ref dwOutBufLen, false, 2U, UnsafeNativeMethods.TCP_TABLE_CLASS.OWNER_PID_ALL, 0U);
                if (error != 0)
                {
                    if (num1 != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(num1);
                        var num3 = IntPtr.Zero;
                    }
                    num1 = Marshal.AllocHGlobal(dwOutBufLen);
                }
                else
                {
                    break;
                }
            }
            try
            {
                if (error != 0)
                {
                    throw new Win32Exception(error);
                }
                var num3 = Marshal.ReadInt32(num1);
                var num4 = IntPtr.Add(num1, 4);
                for (var index = 0; index <= num3 - 1; ++index)
                {
                    var mibTcprowEx = (UnsafeNativeMethods.MIB_TCPROW_EX) Marshal.PtrToStructure(num4, typeof(UnsafeNativeMethods.MIB_TCPROW_EX));
                    if (mibTcprowEx.dwProcessId == id)
                    {
                        var list2 = list1;
                        var ffxivConnection1 = new ServerConnection();
                        ffxivConnection1.SourceAddress = mibTcprowEx.dwRemoteAddr;
                        ffxivConnection1.SourcePort = (ushort) mibTcprowEx.dwRemotePort;
                        ffxivConnection1.DestinationAddress = mibTcprowEx.dwLocalAddr;
                        ffxivConnection1.DestinationPort = (ushort) mibTcprowEx.dwLocalPort;
                        list2.Add(ffxivConnection1);
                    }
                    num4 = IntPtr.Add(num4, Marshal.SizeOf(typeof(UnsafeNativeMethods.MIB_TCPROW_EX)));
                }
            }
            catch
            {
                throw new Win32Exception(error);
            }
            finally
            {
                Marshal.FreeHGlobal(num1);
            }
            return list1;

            /*
            var tables = IPHelper.GetExtendedTCPTable(true);
            return (tables.Cast<TCPRow>()
                          .Where(table => table.ProcessId == Constants.ProcessModel.ProcessID)
                          .Select(table => new ServerConnection
                          {
                              SourceAddress = BitConverter.ToUInt32(table.RemoteEndPoint.Address.GetAddressBytes(), 0),
                              SourcePort = (ushort)table.RemoteEndPoint.Port,
                              DestinationAddress = BitConverter.ToUInt32(table.LocalEndPoint.Address.GetAddressBytes(), 0),
                              DestinationPort = (ushort)table.LocalEndPoint.Port
                          })).ToList();
             */
        }

        private void AdjustNetworkBuffer(NetworkConnection connection)
        {
            var startIndex = 1;
            while ((BitConverter.ToUInt32(connection.NetworkBuffer, startIndex) != 0x41A05252) && (startIndex < connection.NetworkBufferPosition))
            {
                startIndex++;
            }
            if (startIndex >= connection.NetworkBufferPosition)
            {
                connection.NetworkBufferPosition = 0;
            }
            else
            {
                Array.Copy(connection.NetworkBuffer, startIndex, connection.NetworkBuffer, 0, connection.NetworkBufferPosition - startIndex);
                connection.NetworkBufferPosition -= startIndex;
            }
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }

    internal static class WinPcapWrapper
    {
        private const int PCAP_ERRBUF_SIZE = 256;
        private const int PCAP_OPENFLAG_PROMISCUOUS = 1;
        private const int KERNEL_BUFFER_SIZE = 1048576;
        private const int DLT_EN10MB = 1;
        private const int DLT_NULL = 0;
        private const int AF_INET = 2;
        private static ConcurrentDictionary<string, DeviceState> _activeDevices;
        private static EventHandler<DataReceivedEventArgs> _DataReceived;

        static WinPcapWrapper()
        {
            _activeDevices = new ConcurrentDictionary<string, DeviceState>();
        }

        public static event EventHandler<DataReceivedEventArgs> DataReceived
        {
            add
            {
                var eventHandler1 = _DataReceived;
                EventHandler<DataReceivedEventArgs> comparand;
                do
                {
                    comparand = eventHandler1;
                    var eventHandler2 = (EventHandler<DataReceivedEventArgs>) Delegate.Combine(comparand, value);
                    eventHandler1 = Interlocked.CompareExchange(ref _DataReceived, eventHandler2, comparand);
                }
                while (eventHandler1 != comparand);
            }
            remove
            {
                var eventHandler1 = _DataReceived;
                EventHandler<DataReceivedEventArgs> comparand;
                do
                {
                    comparand = eventHandler1;
                    var eventHandler2 = (EventHandler<DataReceivedEventArgs>) Delegate.Remove(comparand, value);
                    eventHandler1 = Interlocked.CompareExchange(ref _DataReceived, eventHandler2, comparand);
                }
                while (eventHandler1 != comparand);
            }
        }

        [DllImport("wpcap.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int pcap_findalldevs(ref IntPtr alldevsp, StringBuilder errbuff);

        [DllImport("wpcap.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void pcap_freealldevs(IntPtr alldevsp);

        [DllImport("wpcap.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr pcap_open(string source, int snaplen, int flags, int read_timeout, IntPtr auth, StringBuilder errbuff);

        [DllImport("wpcap.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int pcap_datalink(IntPtr p);

        [DllImport("wpcap.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int pcap_setbuff(IntPtr p, int dim);

        [DllImport("wpcap.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int pcap_compile(IntPtr p, IntPtr fp, string str, int optimize, uint netmask);

        [DllImport("wpcap.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int pcap_setfilter(IntPtr p, IntPtr fp);

        [DllImport("wpcap.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void pcap_freecode(IntPtr fp);

        [DllImport("wpcap.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int pcap_next_ex(IntPtr p, ref IntPtr pkt_header, ref IntPtr pkt_data);

        [DllImport("wpcap.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void pcap_close(IntPtr p);

        private static void OnDataReceived(DataReceivedEventArgs e)
        {
            var eventHandler = _DataReceived;
            if (eventHandler == null)
            {
                return;
            }
            eventHandler(null, e);
        }

        public static IList<Device> GetAllDevices()
        {
            var list = new List<Device>();
            var alldevsp = IntPtr.Zero;
            try
            {
                var errbuff = new StringBuilder(256);
                if (pcap_findalldevs(ref alldevsp, errbuff) != 0)
                {
                    throw new ApplicationException("Cannot enumerate devices: [" + errbuff + "].");
                }
                pcap_if pcapIf;
                for (var ptr1 = alldevsp; ptr1 != IntPtr.Zero; ptr1 = pcapIf.next)
                {
                    pcapIf = (pcap_if) Marshal.PtrToStructure(ptr1, typeof(pcap_if));
                    var device = new Device();
                    device.Name = pcapIf.name;
                    device.Description = pcapIf.description;
                    device.Addresses = new List<string>();
                    pcap_addr pcapAddr;
                    for (var ptr2 = pcapIf.addresses; ptr2 != IntPtr.Zero; ptr2 = pcapAddr.next)
                    {
                        pcapAddr = (pcap_addr) Marshal.PtrToStructure(ptr2, typeof(pcap_addr));
                        if (pcapAddr.addr != IntPtr.Zero)
                        {
                            var sockaddrIn = (sockaddr_in) Marshal.PtrToStructure(pcapAddr.addr, typeof(sockaddr_in));
                            if (sockaddrIn.sin_family == 2)
                            {
                                device.Addresses.Add(sockaddrIn.sin_addr[0] + "." + sockaddrIn.sin_addr[1] + "." + sockaddrIn.sin_addr[2] + "." + sockaddrIn.sin_addr[3]);
                            }
                        }
                    }
                    list.Add(device);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to get WinPcap device list.", ex);
            }
            finally
            {
                if (alldevsp != IntPtr.Zero)
                {
                    pcap_freealldevs(alldevsp);
                }
            }
            return list;
        }

        public static void StartCapture(SocketObject state)
        {
            var deviceState1 = new DeviceState();

            lock (deviceState1)
            {
                deviceState1.Device = state.device;
                deviceState1.Handle = IntPtr.Zero;
                deviceState1.Cancel = false;
                deviceState1.State = state;
                var num = Marshal.AllocHGlobal(12);
                try
                {
                    if (_activeDevices.ContainsKey(state.device.Name))
                    {
                        StopCapture(state.device.Name);
                    }
                    var errbuff = new StringBuilder(256);
                    deviceState1.Handle = pcap_open(state.device.Name, 65536, 0, 500, IntPtr.Zero, errbuff);
                    if (deviceState1.Handle == IntPtr.Zero)
                    {
                        throw new ApplicationException("Cannot open pcap interface [" + state.device.Name + "].  Error: " + errbuff);
                    }
                    deviceState1.LinkType = pcap_datalink(deviceState1.Handle);
                    if (deviceState1.LinkType != 1 && deviceState1.LinkType != 0)
                    {
                        throw new ApplicationException("Interface [" + state.device.Description + "] does not appear to support Ethernet.");
                    }
                    if (pcap_compile(deviceState1.Handle, num, "ip and tcp", 1, 0U) != 0)
                    {
                        throw new ApplicationException("Unable to create TCP packet filter.");
                    }
                    if (pcap_setfilter(deviceState1.Handle, num) != 0)
                    {
                        throw new ApplicationException("Unable to apply TCP packet filter.");
                    }
                    pcap_freecode(num);
                    _activeDevices[state.device.Name] = deviceState1;
                }
                catch (Exception ex)
                {
                    if (deviceState1.Handle != IntPtr.Zero)
                    {
                        pcap_close(deviceState1.Handle);
                    }
                    throw new ApplicationException("Unable to open winpcap device [" + state.device.Name + "].", ex);
                }
                finally
                {
                    Marshal.FreeHGlobal(num);
                }
                ThreadPool.QueueUserWorkItem(PollNetworkDevice, _activeDevices[state.device.Name]);
            }
        }

        public static void StopCapture(string deviceName)
        {
            try
            {
                if (!_activeDevices.ContainsKey(deviceName))
                {
                    return;
                }
                var deviceState1 = _activeDevices[deviceName];

                lock (deviceState1)
                {
                    deviceState1.Cancel = true;
                    Thread.Sleep(500);
                    if (deviceState1.Handle != IntPtr.Zero)
                    {
                        pcap_close(deviceState1.Handle);
                    }
                    deviceState1.Handle = IntPtr.Zero;
                    var deviceState3 = (DeviceState) null;
                    _activeDevices.TryRemove(deviceName, out deviceState3);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void StopAllCapture()
        {
            var values = _activeDevices.Values;
            foreach (var deviceName in values.Select(x => x.Device.Name)
                                             .Where(x => x != ""))
            {
                StopCapture(deviceName);
            }
        }

        private static void PollNetworkDevice(object stateInfo)
        {
            var deviceState = (DeviceState) stateInfo;
            var offset = deviceState.LinkType == 1 ? 14 : 4;
            var pkt_data = IntPtr.Zero;
            var pkt_header = IntPtr.Zero;
            while (!deviceState.Cancel)
            {
                var num = pcap_next_ex(deviceState.Handle, ref pkt_header, ref pkt_data);
                if (num < 0)
                {
                    break;
                }
                if (num == 0)
                {
                    Thread.Sleep(10);
                }
                else
                {
                    var pcapPkthdr = (pcap_pkthdr) Marshal.PtrToStructure(pkt_header, typeof(pcap_pkthdr));
                    if (pcapPkthdr.caplen > offset)
                    {
                        var destination = new byte[pcapPkthdr.caplen - offset];
                        Marshal.Copy(IntPtr.Add(pkt_data, offset), destination, 0, (int) pcapPkthdr.caplen - offset);
                        var e = new DataReceivedEventArgs();
                        e.Data = destination;
                        e.Device = deviceState;
                        OnDataReceived(e);
                    }
                    else
                    {
                        continue;
                    }
                }
                pkt_header = IntPtr.Zero;
                pkt_data = IntPtr.Zero;
                if (deviceState.Cancel)
                {
                    break;
                }
            }
        }

        private struct pcap_addr
        {
            public IntPtr addr;
            public IntPtr broadaddr;
            public IntPtr dstaddr;
            public IntPtr netmask;
            public IntPtr next;
        }

        private struct pcap_if
        {
            public IntPtr addresses;
            public string description;
            public uint flags;
            public string name;
            public IntPtr next;
        }

        private struct sockaddr_in
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] sin_addr;

            public short sin_family;
            public ushort sin_port;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] sin_zero;
        }

        private struct pcap_pkthdr
        {
            public uint caplen;
            public uint len;
            public uint npkt;
            public uint timestamp_sec;
            public uint timestamp_usec;
        }

        private struct bpf_program
        {
            public IntPtr bf_insns;
            public uint bf_len;
        }

        public struct Device
        {
            public string Name { get; internal set; }
            public string Description { get; internal set; }
            public List<string> Addresses { get; internal set; }
        }

        public class DeviceState
        {
            public SocketObject State { get; internal set; }
            public Device Device { get; internal set; }
            public int LinkType { get; internal set; }
            public IntPtr Handle { get; internal set; }
            public bool Cancel { get; internal set; }
        }

        public class DataReceivedEventArgs : EventArgs
        {
            public byte[] Data { get; set; }
            public DeviceState Device { get; set; }
        }
    }
}
