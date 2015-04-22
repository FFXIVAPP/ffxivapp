// FFXIVAPP.Client
// IPHelper.cs
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
using System.Runtime.InteropServices;

namespace FFXIVAPP.Client.Network
{
    public static class IPHelper
    {
        #region Public Methods

        public static TCPTable GetExtendedTCPTable(bool sorted)
        {
            var tcpRows = new List<TCPRow>();
            var tcpTable = IntPtr.Zero;
            var tcpTableLength = 0;
            if (UnsafeNativeMethods.GetExtendedTcpTable(tcpTable, ref tcpTableLength, sorted, 2, UnsafeNativeMethods.TCP_TABLE_CLASS.OWNER_PID_ALL) == 0)
            {
                return new TCPTable(tcpRows);
            }
            try
            {
                tcpTable = Marshal.AllocHGlobal(tcpTableLength);
                if (UnsafeNativeMethods.GetExtendedTcpTable(tcpTable, ref tcpTableLength, true, 2, UnsafeNativeMethods.TCP_TABLE_CLASS.OWNER_PID_ALL) == 0)
                {
                    var table = (UnsafeNativeMethods.TCPTable) Marshal.PtrToStructure(tcpTable, typeof (UnsafeNativeMethods.TCPTable));
                    var rowPtr = (IntPtr) ((long) tcpTable + Marshal.SizeOf(table.Length));
                    for (var i = 0; i < table.Length; ++i)
                    {
                        tcpRows.Add(new TCPRow((UnsafeNativeMethods.TCPRow) Marshal.PtrToStructure(rowPtr, typeof (UnsafeNativeMethods.TCPRow))));
                        rowPtr = (IntPtr) ((long) rowPtr + Marshal.SizeOf(typeof (UnsafeNativeMethods.TCPRow)));
                    }
                }
            }
            finally
            {
                if (tcpTable != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(tcpTable);
                }
            }
            return new TCPTable(tcpRows);
        }

        #endregion
    }
}
