// FFXIVAPP
// ChatWorker.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using NLog;
using Timer = System.Timers.Timer;

namespace FFXIVAPP.Classes.Memory
{
    public class ChatWorker
    {
        public event NewLineEvnetHandler OnNewline;

        public delegate void NewLineEvnetHandler(ChatEntry cle);

        public event RawLineEvnetHandler OnRawline;

        public delegate void RawLineEvnetHandler(ChatEntry cle);

        private readonly MemoryHandler _handler;
        private readonly Offsets _o;
        private readonly SynchronizationContext _sync = SynchronizationContext.Current;
        private readonly BackgroundWorker _scanner = new BackgroundWorker();
        private readonly Timer _scanTimer;
        private bool _isScanning;
        private int _lastCount;
        private uint _lastChatNum;
        private readonly List<uint> _spots = new List<uint>();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// </summary>
        /// <param name="p"> </param>
        /// <param name="o"> </param>
        public ChatWorker(Process p, Offsets o)
        {
            _scanTimer = new Timer(100);
            _scanTimer.Elapsed += _scanTimer_Elapsed;
            _scanner.DoWork += Scanner_DoWork;
            _scanner.RunWorkerCompleted += Scanner_RunWorkerCompleted;
            _handler = new MemoryHandler(p, 0);
            _o = o;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void _scanTimer_Elapsed(object sender, ElapsedEventArgs e)
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

        #region Public

        /// <summary>
        /// </summary>
        public void StartLogging()
        {
            //Constants.something = _o.Locations["name of location"];
            _scanTimer.Enabled = true;
        }

        /// <summary>
        /// </summary>
        public void StopLogging()
        {
            _scanTimer.Enabled = false;
        }

        #endregion

        #region Private

        /// <summary>
        /// </summary>
        private void PostLineEvent(ChatEntry cle)
        {
            _sync.Post(RaiseLineEvent, cle);
        }

        /// <summary>
        /// </summary>
        /// <param name="state"> </param>
        private void RaiseLineEvent(object state)
        {
            OnNewline((ChatEntry) state);
        }

        /// <summary>
        /// </summary>
        private void PostRawEvent(ChatEntry cle)
        {
            _sync.Post(RaiseRawEvent, cle);
        }

        /// <summary>
        /// </summary>
        /// <param name="state"> </param>
        private void RaiseRawEvent(object state)
        {
            OnRawline((ChatEntry) state);
        }

        #endregion

        #region Threads

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void Scanner_DoWork(object sender, DoWorkEventArgs e)
        {
            _isScanning = true;
            _handler.Address = _o.Locations["CHAT_POINTER"];
            var cp = _handler.GetStructure<ChatPointers>();
            if (_lastCount == 0)
            {
                _lastCount = (int) cp.LineCount1;
            }
            if (_lastCount != cp.LineCount1)
            {
                _spots.Clear();
                var index = (int) (cp.OffsetArrayPos - cp.OffsetArrayStart)/4;
                var lengths = new List<int>();
                try
                {
                    for (var i = cp.LineCount1 - _lastCount; i > 0; i--)
                    {
                        var getline = ((index - i) < 0) ? (index - i) + 60 : index - i;
                        int lineLen;
                        if (getline == 0)
                        {
                            _handler.Address = cp.OffsetArrayStart;
                            lineLen = _handler.GetInt32();
                        }
                        else
                        {
                            _handler.Address = cp.OffsetArrayStart + (uint) ((getline - 1)*4);
                            var p = _handler.GetInt32();
                            _handler.Address = cp.OffsetArrayStart + (uint) (getline*4);
                            var c = _handler.GetInt32();
                            lineLen = c - p;
                        }
                        lengths.Add(lineLen);
                        _handler.Address = cp.OffsetArrayStart + (uint) ((getline - 1)*4);
                        _spots.Add(cp.LogStart + (uint) _handler.GetInt32());
                    }
                    var limit = _spots.Count;
                    for (var i = 0; i < limit; i++)
                    {
                        _spots[i] = (_spots[i] > _lastChatNum) ? _spots[i] : cp.LogStart;
                        _handler.Address = _spots[i];
                        var text = _handler.GetByteArray(lengths[i]);
                        var cle = new ChatEntry(text.ToArray());
                        PostRawEvent(cle);
                        if (Regex.IsMatch(cle.Combined, @"[\w\d]{4}::?.+"))
                        {
                            PostLineEvent(cle);
                        }
                        _lastChatNum = _spots[i];
                    }
                }
                catch (Exception ex)
                {
                    Logger.Warn("{0} :\n{1}", ex.Message, ex.StackTrace);
                }
                _lastCount = (int) cp.LineCount1;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void Scanner_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _isScanning = false;
        }

        #endregion
    }
}