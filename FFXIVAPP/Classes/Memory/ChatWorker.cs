// FFXIVAPP
// ChatWorker.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Web;
using Timer = System.Timers.Timer;
using NLog;

namespace FFXIVAPP.Classes.Memory
{
    public class ChatWorker
    {
        public delegate void NewLineEvnetHandler(string line, Boolean jp);
        public event NewLineEvnetHandler OnNewline;
        public delegate void RawLineEvnetHandler(string line);
        public event RawLineEvnetHandler OnRawline;
        private readonly MemoryHandler _handler;
        private readonly Offsets _o;
        private int _lastCount;
        private readonly SynchronizationContext _sync = SynchronizationContext.Current;
        private readonly BackgroundWorker _scanner = new BackgroundWorker();
        private readonly Timer _scanTimer;
        private bool _isScanning;
        private Boolean _jp;
        private Boolean _colorFound;
        private readonly List<uint> _spots = new List<uint>();
        private readonly List<byte> _newText = new List<byte>();
        private List<byte> _nList, _aList, _cList;
        private string _cleaned;
        private readonly string[] _checks = new[] { "0020", "0021", "0023", "0027", "0028", "0046", "0047", "0048", "0049", "005C" };
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// </summary>
        /// <param name="p"> </param>
        /// <param name="o"> </param>
        public ChatWorker(Process p, Offsets o)
        {
            _scanTimer = new Timer(500);
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
        /// <param name="line"> </param>
        private void PostLineEvent(string line)
        {
            _sync.Post(RaiseLineEvent, line);
        }

        /// <summary>
        /// </summary>
        /// <param name="state"> </param>
        private void RaiseLineEvent(object state)
        {
            OnNewline((string) state, _jp);
        }

        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        private void PostRawEvent(string line)
        {
            _sync.Post(RaiseRawEvent, line);
        }

        /// <summary>
        /// </summary>
        /// <param name="state"> </param>
        private void RaiseRawEvent(object state)
        {
            OnRawline((string) state);
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
            var count = (int) (cp.OffsetArrayStop - cp.OffsetArrayStart)/4 - 1;
            if (_lastCount == 0)
            {
                _lastCount = (int) cp.LineCount;
            }
            else if (cp.LineCount > _lastCount)
            {
                if (_spots.Count > 0)
                {
                    _spots.Clear();
                }
                for (var i = count; i > count - (cp.LineCount - _lastCount); i--)
                {
                    if (i < 0)
                    {
                        break;
                    }
                    _handler.Address = cp.OffsetArrayStart + (uint) ((i - 1)*4);
                    _spots.Insert(0, cp.LogStart + (uint) _handler.GetInt32());
                }
                try
                {
                    for (var i = 0; i < _spots.Count; i++)
                    {
                        if (i < 0)
                        {
                            break;
                        }
                        uint length;
                        if (i < _spots.Count - 1)
                        {
                            length = _spots[i + 1] - _spots[i];
                        }
                        else
                        {
                            length = cp.LogNextEntry - _spots[i];
                        }
                        _handler.Address = _spots[i];
                        var text = _handler.GetByteArray((int) length);
                        if (_newText.Count > 0)
                        {
                            _newText.Clear();
                        }
                        foreach (var t in text)
                        {
                            if (t != 0)
                            {
                                _newText.Add(t);
                            }
                        }
                        var tmpString = "";
                        for (var j = 0; j < _newText.Count; j++)
                        {
                            tmpString += _newText[j].ToString(CultureInfo.CurrentUICulture) + " ";
                        }
                        PostRawEvent(tmpString.Substring(0, tmpString.Length - 1));
                        var results = CleanUpStringAt(_newText.ToArray(), CultureInfo.CurrentUICulture);
                        if (results.Length > 5)
                        {
                            PostLineEvent(results);
                        }
                    }
                }
                catch(Exception ex)
                {
                    Logger.Error("ErrorEvent : ", ex.Message + ex.StackTrace + ex.InnerException);
                }
                _lastCount = (int) cp.LineCount;
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

        /// <summary>
        /// </summary>
        /// <param name="bytes"> </param>
        /// <param name="ci"> </param>
        /// <returns> </returns>
        private string CleanUpStringAt(byte[] bytes, CultureInfo ci)
        {
            _jp = false;
            _cleaned = "";
            _aList = new List<byte>();
            _nList = new List<byte>();
            _cList = new List<byte>();
            for (var t = 0; t < 4; t++)
            {
                _cList.Add(bytes[t]);
            }
            var check = Encoding.UTF8.GetString(_cList.ToArray());
            for (var x = 0; x < bytes.Count(); x++)
            {
                if (bytes[x] == 2)
                {
                    if (bytes[x + 1] == 29 && bytes[x + 2] == 1 && bytes[x + 3] == 3)
                    {
                        x += 4;
                    }
                    else if (bytes[x + 1] == 16 && bytes[x + 2] == 1 && bytes[x + 3] == 3)
                    {
                        x += 4;
                    }
                }
                if (_checks.Contains(check))
                {
                    if (bytes[x] == 2 && _colorFound == false)
                    {
                        x += 18;
                        _colorFound = true;
                    }
                    else if (bytes[x] == 2 && _colorFound)
                    {
                        x += 10;
                        _colorFound = false;
                    }
                    _nList.Add(bytes[x]);
                }
                else
                {
                    if (bytes[x] == 2)
                    {
                        int length = bytes[x + 2];
                        if (length == 3)
                        {
                            x = x + 3;
                            if (bytes[x] == 1)
                            {
                                x++;
                            }
                            while (bytes[x] != 3)
                            {
                                _aList.AddRange(Encoding.UTF8.GetBytes(bytes[x].ToString("X2")));
                                x++;
                            }
                        }
                        else
                        {
                            x = x + 5;
                            var end = length - 3;
                            for (var t = 0; t < end; t++)
                            {
                                _aList.AddRange(Encoding.UTF8.GetBytes(bytes[x].ToString("X2")));
                                x++;
                            }
                        }
                        if (bytes[x] == 3)
                        {
                            x++;
                        }
                        string aCheckstr;
                        try
                        {
                            aCheckstr = Constants.XAtCodes[Encoding.UTF8.GetString(_aList.ToArray())];
                        }
                        catch
                        {
                            aCheckstr = "";
                        }

                        if (aCheckstr != "")
                        {
                            var aCheckbyte = Encoding.UTF8.GetBytes(aCheckstr);
                            _nList.AddRange(aCheckbyte);
                        }
                        else
                        {
                            _nList.AddRange(_aList);
                        }
                        _aList.Clear();
                    }
                    else
                    {
                        if (bytes[x] > 127)
                        {
                            _jp = true;
                        }
                        _nList.Add(bytes[x]);
                    }
                }
            }
            _cleaned = HttpUtility.HtmlDecode(Encoding.UTF8.GetString(_nList.ToArray())).Replace("  ", " ");
            _aList.Clear();
            _nList.Clear();
            _cList.Clear();
            return _cleaned.Length < 5 && ci.TwoLetterISOLanguageName == "ja" ? HttpUtility.HtmlDecode(Encoding.UTF8.GetString(bytes.ToArray())).Replace("  ", " ") : _cleaned;
        }

        #endregion
    }
}