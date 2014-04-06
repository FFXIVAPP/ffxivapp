// FFXIVAPP.Client
// ChatEntry.cs
// 
// Copyright © 2007 - 2014 Ryan Wilson - All Rights Reserved
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
using System.Linq;
using System.Text;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Helpers;

namespace FFXIVAPP.Client.Memory
{
    public static class ChatEntry
    {
        public static ChatLogEntry Process(byte[] raw)
        {
            var chatLogEntry = new ChatLogEntry();
            try
            {
                chatLogEntry.Bytes = raw;
                chatLogEntry.Raw = Encoding.UTF8.GetString(raw.ToArray());
                var cut = (chatLogEntry.Raw.Substring(13, 1) == ":") ? 14 : 13;
                var cleaned = new ChatCleaner(raw).Result;
                chatLogEntry.Line = XmlHelper.SanitizeXmlString(cleaned.Substring(cut));
                chatLogEntry.Line = new ChatCleaner(chatLogEntry.Line).Result;
                chatLogEntry.JP = Encoding.UTF8.GetBytes(chatLogEntry.Line)
                                          .Any(b => b > 128);
                chatLogEntry.Code = chatLogEntry.Raw.Substring(8, 4);
                chatLogEntry.Combined = String.Format("{0}:{1}", chatLogEntry.Code, chatLogEntry.Line);
                chatLogEntry.TimeStamp = DateTimeHelper.UnixTimeStampToDateTime(Int32.Parse(chatLogEntry.Raw.Substring(0, 8), NumberStyles.HexNumber));
            }
            catch (Exception ex)
            {
                chatLogEntry.Bytes = new byte[0];
                chatLogEntry.Raw = "";
                chatLogEntry.Line = "";
                chatLogEntry.Code = "";
                chatLogEntry.Combined = "";
            }
            return chatLogEntry;
        }
    }
}
