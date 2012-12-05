// FFXIVAPP.Client
// ChatWorker.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using System.Windows;
using FFXIVAPP.Common.Utilities;
using NLog;
using Timer = System.Timers.Timer;

namespace FFXIVAPP.Client.Memory
{
    internal class ChatWorker : INotifyPropertyChanged, IDisposable
    {
        #region Property Bindings

        #endregion

        #region Declarations

        private readonly MemoryHandler _handler;
        private readonly SigFinder _offsets;
        private readonly SynchronizationContext _sync = SynchronizationContext.Current;
        private readonly BackgroundWorker _scanner = new BackgroundWorker();
        private readonly Timer _scanTimer;
        private readonly List<uint> _spots = new List<uint>();
        private int _lastCount;
        private uint _lastChatNum;
        private bool _isScanning;

        private static readonly Logger Tracer = LogManager.GetCurrentClassLogger();

        #endregion

        #region Events

        public event NewLineEventHandler OnNewline = delegate { };

        /// <summary>
        /// </summary>
        /// <param name="chatEntry"> </param>
        private void PostLineEvent(ChatEntry chatEntry)
        {
            _sync.Post(RaiseLineEvent, chatEntry);
        }

        /// <summary>
        /// </summary>
        /// <param name="state"> </param>
        private void RaiseLineEvent(object state)
        {
            OnNewline((ChatEntry) state);
        }

        #endregion

        #region Delegates

        public delegate void NewLineEventHandler(ChatEntry chatEntry);

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="process"> </param>
        /// <param name="offsets"> </param>
        public ChatWorker(Process process, SigFinder offsets)
        {
            _scanTimer = new Timer(100);
            _scanTimer.Elapsed += ScanTimerElapsed;
            _scanner.DoWork += ScannerDoWork;
            _scanner.RunWorkerCompleted += ScannerRunWorkerCompleted;
            _handler = new MemoryHandler(process, 0);
            _offsets = offsets;
        }

        #region Timer Controls

        /// <summary>
        /// </summary>
        public void StartLogging()
        {
            _scanTimer.Enabled = true;
        }

        /// <summary>
        /// </summary>
        public void StopLogging()
        {
            _scanTimer.Enabled = false;
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
            if (_scanner.IsBusy != true)
            {
                _scanner.RunWorkerAsync();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void ScannerDoWork(object sender, DoWorkEventArgs e)
        {
            _isScanning = true;
            _handler.Address = _offsets.Locations["CHATLOG"];
            var chatPointers = _handler.GetStructure<ChatPointers>();
            if (_lastCount == 0)
            {
                _lastCount = (int)chatPointers.LineCount1;
            }
            if (_lastCount == chatPointers.LineCount1)
            {
                return;
            }
            _spots.Clear();
            var index = (int)(chatPointers.OffsetArrayPos - chatPointers.OffsetArrayStart) / 4;
            var lengths = new List<int>();
            try
            {
                for (var i = chatPointers.LineCount1 - _lastCount; i > 0; i--)
                {
                    var getline = ((index - i) < 0) ? (index - i) + 60 : index - i;
                    int lineLen;
                    if (getline == 0)
                    {
                        _handler.Address = chatPointers.OffsetArrayStart;
                        lineLen = _handler.GetInt32();
                    }
                    else
                    {
                        _handler.Address = chatPointers.OffsetArrayStart + (uint)((getline - 1) * 4);
                        var previous = _handler.GetInt32();
                        _handler.Address = chatPointers.OffsetArrayStart + (uint)(getline * 4);
                        var current = _handler.GetInt32();
                        lineLen = current - previous;
                    }
                    lengths.Add(lineLen);
                    _handler.Address = chatPointers.OffsetArrayStart + (uint)((getline - 1) * 4);
                    _spots.Add(chatPointers.LogStart + (uint)_handler.GetInt32());
                }
                var limit = _spots.Count;
                for (var i = 0; i < limit; i++)
                {
                    _spots[i] = (_spots[i] > _lastChatNum) ? _spots[i] : chatPointers.LogStart;
                    _handler.Address = _spots[i];
                    var text = _handler.GetByteArray(lengths[i]);
                    var chatEntry = new ChatEntry(text.ToArray());
                    if (Regex.IsMatch(chatEntry.Combined, @"[\w\d]{4}::?.+"))
                    {
                        PostLineEvent(chatEntry);
                    }
                    else
                    {
                        Tracer.Debug("DebugLineEvent: {0}", text.ToArray());
                    }
                    _lastChatNum = _spots[i];
                }
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
            }
            _lastCount = (int)chatPointers.LineCount1;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void ScannerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _isScanning = false;
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
            _scanTimer.Elapsed -= ScanTimerElapsed;
            _scanner.DoWork -= ScannerDoWork;
            _scanner.RunWorkerCompleted -= ScannerRunWorkerCompleted;
        }

        #endregion
    }
}