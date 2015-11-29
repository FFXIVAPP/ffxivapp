// FFXIVAPP.Client
// ChatLogWorker.cs
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
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Memory;
using NLog;

namespace FFXIVAPP.Client.Memory
{
    internal class ChatLogWorker : INotifyPropertyChanged, IDisposable
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        public ChatLogWorker()
        {
            _scanTimer = new Timer(250);
            _scanTimer.Elapsed += ScanTimerElapsed;
        }

        #region Property Bindings

        private IntPtr ChatPointerMap { get; set; }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            _scanTimer.Elapsed -= ScanTimerElapsed;
        }

        #endregion

        #region Threads

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void ScanTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_isScanning)
            {
                return;
            }
            _isScanning = true;
            double refresh = 250;
            if (Double.TryParse(Settings.Default.ChatLogWorkerRefresh.ToString(CultureInfo.InvariantCulture), out refresh))
            {
                _scanTimer.Interval = refresh;
            }
            Func<bool> scannerWorker = delegate
            {
                var readResult = Reader.GetChatLog(_previousArrayIndex, _previousOffset);

                _previousArrayIndex = readResult.PreviousArrayIndex;
                _previousOffset = readResult.PreviousOffset;

                #region Notifications

                foreach (var chatLogEntry in readResult.ChatLogEntries)
                {
                    AppContextHelper.Instance.RaiseNewChatLogEntry(chatLogEntry);
                }

                #endregion

                _isScanning = false;
                return true;
            };
            scannerWorker.BeginInvoke(delegate { }, scannerWorker);
        }

        #endregion

        #region Declarations

        private static readonly Logger Tracer = Logger;
        private readonly Timer _scanTimer;
        private readonly BackgroundWorker _scanner = new BackgroundWorker();

        private bool _isScanning;

        private int _previousArrayIndex;
        private int _previousOffset;

        #endregion

        #region Timer Controls

        /// <summary>
        /// </summary>
        public void StartScanning()
        {
            _scanTimer.Enabled = true;
        }

        /// <summary>
        /// </summary>
        public void StopScanning()
        {
            _scanTimer.Enabled = false;
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
}
