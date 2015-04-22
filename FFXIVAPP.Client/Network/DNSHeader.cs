// FFXIVAPP.Client
// DNSHeader.cs
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
using System.Globalization;
using System.IO;
using System.Net;

namespace FFXIVAPP.Client.Network
{
    public class DNSHeader
    {
        private readonly ushort _flags;
        private readonly ushort _identification;
        private readonly ushort _totalAdditionalRR;
        private readonly ushort _totalAnswerRR;
        private readonly ushort _totalAuthorityRR;
        private readonly ushort _totalQuestions;

        public DNSHeader(byte[] byBuffer, int nReceived)
        {
            try
            {
                using (var memoryStream = new MemoryStream(byBuffer, 0, nReceived))
                {
                    using (var binaryReader = new BinaryReader(memoryStream))
                    {
                        _identification = (ushort) IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                        _flags = (ushort) IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                        _totalQuestions = (ushort) IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                        _totalAnswerRR = (ushort) IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                        _totalAuthorityRR = (ushort) IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                        _totalAdditionalRR = (ushort) IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public string Identification
        {
            get { return String.Format("0x{0:x2}", _identification); }
        }

        public string Flags
        {
            get { return String.Format("0x{0:x2}", _flags); }
        }

        public string TotalQuestions
        {
            get { return _totalQuestions.ToString(CultureInfo.InvariantCulture); }
        }

        public string TotalAnswerRR
        {
            get { return _totalAnswerRR.ToString(CultureInfo.InvariantCulture); }
        }

        public string TotalAuthorityRR
        {
            get { return _totalAuthorityRR.ToString(CultureInfo.InvariantCulture); }
        }

        public string TotalAdditionalRR
        {
            get { return _totalAdditionalRR.ToString(CultureInfo.InvariantCulture); }
        }
    }
}
