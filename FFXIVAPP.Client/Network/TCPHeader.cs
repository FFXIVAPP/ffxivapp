// FFXIVAPP.Client
// TCPHeader.cs
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
using System.Globalization;
using System.IO;
using System.Net;

namespace FFXIVAPP.Client.Network
{
    public class TCPHeader
    {
        private readonly uint _acknowledgementNumber = 555;
        private readonly short _checksum = 555;
        private readonly List<byte> _data = new List<byte>();
        private readonly ushort _destinationPort;
        private readonly ushort _flags = 555;
        private readonly byte _headerLength;
        private readonly ushort _messageLength;
        private readonly uint _sequenceNumber = 555;
        private readonly ushort _sourcePort;
        private readonly ushort _urgentPointer;
        private readonly ushort _window = 555;

       public TCPHeader(byte[] byBuffer, int nReceived)
        {
            try
            {
                using (var memoryStream = new MemoryStream(byBuffer, 0, nReceived))
                {
                    using (var binaryReader = new BinaryReader(memoryStream))
                    {
                        _sourcePort = (ushort) IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                        _destinationPort = (ushort) IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                        _sequenceNumber = (uint) IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
                        _acknowledgementNumber = (uint) IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
                        _flags = (ushort) IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                        _window = (ushort) IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                        _checksum = (short) IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                        _urgentPointer = (ushort) IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                        _headerLength = (byte) (_flags >> 12);
                        _headerLength *= 4;
                        _messageLength = (ushort) (nReceived - _headerLength);
                        var tempData = new byte[nReceived - _headerLength];
                        Array.Copy(byBuffer, _headerLength, tempData, 0, nReceived - _headerLength);
                        foreach (var b in tempData)
                        {
                            _data.Add(b);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public string SourcePort
        {
            get { return _sourcePort.ToString(CultureInfo.InvariantCulture); }
        }

        public string DestinationPort
        {
            get { return _destinationPort.ToString(CultureInfo.InvariantCulture); }
        }

        public string SequenceNumber
        {
            get { return _sequenceNumber.ToString(CultureInfo.InvariantCulture); }
        }

        public string AcknowledgementNumber
        {
            get { return (_flags & 0x10) != 0 ? _acknowledgementNumber.ToString(CultureInfo.InvariantCulture) : ""; }
        }

        public string HeaderLength
        {
            get { return _headerLength.ToString(CultureInfo.InvariantCulture); }
        }

        public string WindowSize
        {
            get { return _window.ToString(CultureInfo.InvariantCulture); }
        }

        public string UrgentPointer
        {
            get { return (_flags & 0x20) != 0 ? _urgentPointer.ToString(CultureInfo.InvariantCulture) : ""; }
        }

        public string Flags
        {
            get
            {
                var newFlags = _flags & 0x3F;
                var stringFlags = String.Format("0x{0:x2} (", newFlags);
                if ((newFlags & 0x01) != 0)
                {
                    stringFlags += "FIN, ";
                }
                if ((newFlags & 0x02) != 0)
                {
                    stringFlags += "SYN, ";
                }
                if ((newFlags & 0x04) != 0)
                {
                    stringFlags += "RST, ";
                }
                if ((newFlags & 0x08) != 0)
                {
                    stringFlags += "PSH, ";
                }
                if ((newFlags & 0x10) != 0)
                {
                    stringFlags += "ACK, ";
                }
                if ((newFlags & 0x20) != 0)
                {
                    stringFlags += "URG";
                }
                stringFlags += ")";
                if (stringFlags.Contains("()"))
                {
                    stringFlags = stringFlags.Remove(stringFlags.Length - 3);
                }
                else if (stringFlags.Contains(", )"))
                {
                    stringFlags = stringFlags.Remove(stringFlags.Length - 3, 2);
                }
                return stringFlags;
            }
        }

        public string Checksum
        {
            get { return String.Format("0x{0:x2}", _checksum); }
        }

        public List<byte> Data
        {
            get { return _data; }
        }

        public ushort MessageLength
        {
            get { return _messageLength; }
        }
    }
}
