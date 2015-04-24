// FFXIVAPP.Client
// NetworkWorker.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Helpers;
using NLog;

namespace FFXIVAPP.Client.Network
{
    internal class NetworkWorker : INotifyPropertyChanged, IDisposable
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Property Bindings

        #endregion

        #region Declarations

        private List<ServerConnection> DroppedConnections = new List<ServerConnection>();
        private object Lock = new object();
        private List<ServerConnection> ServerConnections = new List<ServerConnection>();
        private List<SocketObject> Sockets = new List<SocketObject>();

        #endregion

        public NetworkWorker()
        {
        }

        #region Timer Controls

        /// <summary>
        /// </summary>
        public void StartScanning()
        {
            var interfaces = GetNetworkInterfaces();
            foreach (var item in interfaces.Where(x => !String.IsNullOrWhiteSpace(x))
                                           .Select(address => new SocketObject
                                           {
                                               IPAddress = address
                                           }))
            {
                Sockets.Add(item);
            }
            UpdateConnectionList();
            foreach (var stateObject in Sockets)
            {
                try
                {
                    stateObject.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
                    stateObject.Socket.Bind(new IPEndPoint(IPAddress.Parse(stateObject.IPAddress), 0));
                    stateObject.Socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AcceptConnection, true);
                    var isTrue = new byte[]
                    {
                        3, 0, 0, 0
                    };
                    var isOut = new byte[]
                    {
                        1, 0, 0, 0
                    };
                    stateObject.Socket.IOControl(IOControlCode.ReceiveAll, isTrue, isOut);
                    stateObject.Socket.ReceiveBufferSize = 0x7D000;
                    stateObject.Socket.BeginReceive(stateObject.Buffer, 0, stateObject.Buffer.Length, SocketFlags.None, OnReceive, stateObject);
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

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                var asyncState = (SocketObject) ar.AsyncState;
                int nReceived;
                try
                {
                    nReceived = asyncState.Socket.EndReceive(ar);
                }
                catch (Exception ex)
                {
                    nReceived = 0;
                }
                if (nReceived > 0)
                {
                    try
                    {
                        DispatcherHelper.Invoke(() => ParseData(asyncState, asyncState.Buffer, nReceived));
                    }
                    catch (Exception ex)
                    {
                    }
                }
                asyncState.Socket.BeginReceive(asyncState.Buffer, 0, asyncState.Buffer.Length, SocketFlags.None, OnReceive, asyncState);
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
                DestinationAddress = BitConverter.ToUInt32(IP.DestinationAddress.GetAddressBytes(), 0),
                DestinationPort = Convert.ToUInt16(TCP.DestinationPort),
                SourcePort = Convert.ToUInt16(TCP.SourcePort),
                SourceAddress = BitConverter.ToUInt32(IP.SourceAddress.GetAddressBytes(), 0),
                TimeStamp = DateTime.Now
            };
            lock (Lock)
            {
                var found = Enumerable.Contains(ServerConnections, serverConnection);
                if (!found)
                {
                    if (Enumerable.Contains(DroppedConnections, serverConnection))
                    {
                        found = true;
                    }
                    if (found)
                    {
                        return;
                    }
                    UpdateConnectionList();
                    if (Enumerable.Contains(ServerConnections, serverConnection))
                    {
                        found = true;
                    }
                    if (!found)
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
            lock (asyncState.SocketLock)
            {
                Func<KeyValuePair<uint, NetworkPacket>, bool> func = null;
                Func<NetworkConnection, bool> predicate = x => x.Equals(serverConnection);
                var connection = asyncState.Connections.FirstOrDefault(predicate);
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
                        if (func == null)
                        {
                            func = x => x.Key <= connection.NextTCPSequence.Value;
                        }
                        stalePacket = (connection.StalePackets.Where(func)
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
            while (true)
            {
                if (connection.NetworkBufferPosition < 0x1C)
                {
                    break;
                }
                uint bufferSize;
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
                            using (var destinationStream = new MemoryStream(destinationArray, 0x2A, destinationArray.Length - 0x2A))
                            {
                                using (var decompressedStream = new DeflateStream(destinationStream, CompressionMode.Decompress))
                                {
                                    messageLength = decompressedStream.Read(bytes, 0, bytes.Length);
                                }
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
            if (serverIPList != null)
            {
                if (ServerConnections.Any())
                {
                    foreach (var server in serverIPList)
                    {
                        var serverIP = server;
                        foreach (var connection in ServerConnections.Where(connection => !serverIP.Equals(connection)))
                        {
                            ServerConnections.Add(server);
                        }
                    }
                }
                else
                {
                    ServerConnections = serverIPList;
                }
            }
            var removeFromDropped = new List<ServerConnection>();
            foreach (var connection in DroppedConnections)
            {
                if (DateTime.Now.Subtract(connection.TimeStamp)
                            .TotalMilliseconds > 30000.0)
                {
                    removeFromDropped.Add(connection);
                    continue;
                }
                var found = Enumerable.Contains(ServerConnections, connection);
                if (found)
                {
                    removeFromDropped.Add(connection);
                }
            }
            foreach (var connection in removeFromDropped)
            {
                DroppedConnections.Remove(connection);
            }
        }

        private static List<ServerConnection> GetXIVServerIPList()
        {
            var tables = IPHelper.GetExtendedTCPTable(true);
            return (tables.Cast<TCPRow>()
                          .Where(table => table.ProcessId == Constants.ProcessID)
                          .Select(table => new ServerConnection
                          {
                              SourceAddress = BitConverter.ToUInt32(table.RemoteEndPoint.Address.GetAddressBytes(), 0),
                              SourcePort = (ushort) table.RemoteEndPoint.Port,
                              DestinationAddress = BitConverter.ToUInt32(table.LocalEndPoint.Address.GetAddressBytes(), 0),
                              DestinationPort = (ushort) table.LocalEndPoint.Port
                          })).ToList();
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

        #region Implementation of IDisposable

        public void Dispose()
        {
        }

        #endregion
    }
}
