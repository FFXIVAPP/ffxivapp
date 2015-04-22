// FFXIVAPP.Client
// IPHeader.cs
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
    public class IPHeader
    {
        private readonly byte _TTL;
        private readonly short _checksum;
        private readonly List<byte> _data = new List<byte>();
        private readonly uint _destinationIPAddress;
        private readonly byte _differentiatedServices;
        private readonly ushort _flags;
        private readonly byte _headerLength;
        private readonly ushort _identification;
        private readonly byte _protocol;
        private readonly uint _sourceIPAddress;
        private readonly ushort _totalLength;
        private readonly byte _versionAndHeaderLength;

        public IPHeader(byte[] byBuffer, int nReceived)
        {
            try
            {
                using (var memoryStream = new MemoryStream(byBuffer, 0, nReceived))
                {
                    using (var binaryReader = new BinaryReader(memoryStream))
                    {
                        _versionAndHeaderLength = binaryReader.ReadByte();
                        _differentiatedServices = binaryReader.ReadByte();
                        _totalLength = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                        _identification = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                        _flags = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                        _TTL = binaryReader.ReadByte();
                        _protocol = binaryReader.ReadByte();
                        _checksum = IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                        _sourceIPAddress = (uint)(binaryReader.ReadInt32());
                        _destinationIPAddress = (uint)(binaryReader.ReadInt32());
                        _headerLength = _versionAndHeaderLength;
                        _headerLength <<= 4;
                        _headerLength >>= 4;
                        _headerLength *= 4;
                        var tempData = new byte[_totalLength - _headerLength];
                        Array.Copy(byBuffer, _headerLength, tempData, 0, _totalLength - _headerLength);
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

        public string Version
        {
            get
            {
                switch ((_versionAndHeaderLength >> 4))
                {
                    case 4:
                        return "IP v4";
                    case 6:
                        return "IP v6";
                    default:
                        return "Unknown";
                }
            }
        }

        public string HeaderLength
        {
            get { return _headerLength.ToString(); }
        }

        public ushort MessageLength
        {
            get { return (ushort) (_totalLength - _headerLength); }
        }

        public string DifferentiatedServices
        {
            get { return String.Format("0x{0:x2} ({1})", _differentiatedServices, _differentiatedServices); }
        }

        public string Flags
        {
            get
            {
                var newFlags = _flags >> 13;
                switch (newFlags)
                {
                    case 2:
                        return "Don't fragment";
                    case 1:
                        return "More fragments to come";
                    default:
                        return newFlags.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        public string FragmentationOffset
        {
            get
            {
                var newOffset = _flags << 3;
                newOffset >>= 3;
                return newOffset.ToString(CultureInfo.InvariantCulture);
            }
        }

        public string TTL
        {
            get { return _TTL.ToString(CultureInfo.InvariantCulture); }
        }

        public Protocol ProtocolType
        {
            get
            {
                switch (_protocol)
                {
                    case 6:
                        return Protocol.TCP;
                    case 17:
                        return Protocol.UDP;
                    default:
                        return Protocol.Unknown;
                }
            }
        }

        public string Checksum
        {
            get { return String.Format("0x{0:x2}", _checksum); }
        }

        public IPAddress SourceAddress
        {
            get { return new IPAddress(_sourceIPAddress); }
        }

        public IPAddress DestinationAddress
        {
            get { return new IPAddress(_destinationIPAddress); }
        }

        public string TotalLength
        {
            get { return _totalLength.ToString(CultureInfo.InvariantCulture); }
        }

        public string Identification
        {
            get { return _identification.ToString(CultureInfo.InvariantCulture); }
        }

        public List<byte> Data
        {
            get { return _data; }
        }
    }
}
