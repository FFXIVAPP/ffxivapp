using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows;
using System.Web;

namespace AppModXIV.Memory
{
    public class ChatWorker
    {
        public static string Chat;
        MemoryHandler _handler;
        Process _proc;
        System.Timers.Timer _scanTimer;
        Offsets _o;
        int _lastCount = 0;
        SynchronizationContext _sync = SynchronizationContext.Current;

        BackgroundWorker _Scanner = new BackgroundWorker();

        public delegate void NewLineEvnetHandler(string line, Boolean JP);
        public event NewLineEvnetHandler OnNewline;

        public delegate void RawLineEvnetHandler(string line);
        public event RawLineEvnetHandler OnRawline;

        Boolean JP = false;
        List<uint> spots = new List<uint>();
        List<byte> newText = new List<byte>();

        private bool _isScanning;

        Boolean ATFound, endAT;
        List<byte> bList, nList, aList;
        string Cleaned;

        public ChatWorker(Process p, Offsets o)
        {
            _scanTimer = new System.Timers.Timer(500);
            _scanTimer.Elapsed += _scanTimer_Elapsed;

            _Scanner.DoWork += new DoWorkEventHandler(Scanner_DoWork);
            _Scanner.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Scanner_RunWorkerCompleted);

            _proc = p;
            _handler = new MemoryHandler(p, 0);
            _o = o;
        }

        void _scanTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!_isScanning)
            {
                if (_Scanner.IsBusy != true)
                {
                    _Scanner.RunWorkerAsync();
                }
            }
        }

        #region  Public Methods
        public void StartLogging()
        {
            Constants.CHATLOG = _o.Locations["CHATPOINTER"];
            //Constants.CRAFTING = _o.Locations["CRAFTPOINTER"];

            _scanTimer.Enabled = true;
        }
        public void StopLogging()
        {
            _scanTimer.Enabled = false;
        }
        #endregion

        #region Private Methods
        private void PostLineEvent(string line, Boolean JP)
        {
            _sync.Post(RaiseLineEvent, line);
        }

        private void RaiseLineEvent(object state)
        {
            OnNewline((string)state, JP);
        }

        private void PostRawEvent(string line)
        {
            _sync.Post(RaiseRawEvent, line);
        }

        private void RaiseRawEvent(object state)
        {
            OnRawline((string)state);
        }
        #endregion

        #region Thread Methods
        public void Scanner_DoWork(object sender, DoWorkEventArgs e)
        //public void Scanner()
        {
            _isScanning = true;
            _handler.address = _o.Locations["CHATPOINTER"];
            Chat = Convert.ToString(_o.Locations["CHATPOINTER"]);

            ChatPointers cp = _handler.GetStructure<ChatPointers>();
            //MessageBox.Show(Chat.ToString());
            int count = (int)(cp.OffsetArrayStop - cp.OffsetArrayStart) / 4 - 1;
            if (_lastCount == 0)
            {
                _lastCount = (int)cp.LineCount;

            }
            else if (cp.LineCount > _lastCount)
            {

                if (spots.Count > 0)
                {
                    spots.Clear();
                }

                for (int i = count; i > count - (cp.LineCount - _lastCount); i--)
                {
                    if (i < 0) { break; }
                    _handler.address = (uint)cp.OffsetArrayStart + (uint)((i - 1) * 4);
                    spots.Insert(0, cp.LogStart + (uint)_handler.GetInt32());
                }

                uint length = 0;

                try
                {
                    for (int i = 0; i < spots.Count; i++)
                    {
                        if (i < 0) { break; }


                        if (i < spots.Count - 1)
                        {
                            length = spots[i + 1] - spots[i];
                        }
                        else
                        {
                            length = cp.LogNextEntry - spots[i];
                        }

                        _handler.address = (uint)spots[i];

                        if (length < 2147483648)
                        {
                            byte[] text = _handler.GetByteArray((int)length);

                            if (newText.Count > 0)
                            {
                                newText.Clear();
                            }

                            for (int x = 0; x < text.Length; x++)
                            {
                                if (text[x] != 0)
                                    newText.Add(text[x]);
                            }

                            text = null;
                            string tmpString = "";

                            for (int j = 0; j < newText.Count; j++)
                            {
                                tmpString += newText[j].ToString() + " ";
                            }

                            if (tmpString != null)
                            {
                                PostRawEvent(tmpString.Substring(0, tmpString.Length - 1));
                                tmpString = null;
                            }

                            string results = CleanUpStringAT(newText.ToArray());
                            if (results.Length > 5)
                            {
                                PostLineEvent(results, JP);
                                results = null;
                            }
                        }
                    }
                }
                catch (ArithmeticException ex)
                {
                    //if (Constants.LogErrors == 1)
                    //{
                    //    ErrorLogging.LogError(ex.Message + ex.StackTrace + ex.InnerException);
                    //    ErrorLogging.LogError("Length: " + length.ToString());
                    //}
                }
                catch (Exception ex)
                {
                    //if (Constants.LogErrors == 1)
                    //{
                    //    ErrorLogging.LogError(ex.Message + ex.StackTrace + ex.InnerException);
                    //}
                }

                _lastCount = (int)cp.LineCount;
            }
        }

        public void Scanner_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _isScanning = false;
        }

        private string CleanUpStringAT(byte[] bytes)
        {
            Constants.bJP = false;
            JP = false;
            ATFound = false;
            Cleaned = "";

            aList = new List<byte>();
            bList = new List<byte>(bytes);
            nList = new List<byte>();

            int length = 0;
            int count = 1000;
            int lCheck = 1;

            for (int x = 0; x < bList.Count; x++)
            {
                if (bytes[x] == 2 && ATFound == false)
                {
                    length = bytes[x + 2] + 3;
                    count = x + length;
                    ATFound = true;
                }

                if (x == count && ATFound == true)
                {
                    ATFound = false;
                }

                if (ATFound)
                {
                    endAT = false;
                    if (lCheck == length)
                    {
                        aList.AddRange(Encoding.UTF8.GetBytes(bytes[x].ToString("X2")));
                        endAT = true;
                    }
                    else
                    {
                        if (lCheck > 3)
                        {
                            aList.AddRange(Encoding.UTF8.GetBytes(bytes[x].ToString("X2") + "-"));
                        }
                    }

                    if (endAT)
                    {
                        string aCheckstr = "";

                        try
                        {
                            aCheckstr = Constants.xATCodes[System.Text.Encoding.UTF8.GetString(aList.ToArray())];
                        }
                        catch
                        {
                            aCheckstr = "";
                        }

                        if (aCheckstr != "")
                        {
                            byte[] aCheckbyte = Encoding.UTF8.GetBytes(aCheckstr);
                            nList.AddRange(aCheckbyte);

                            aCheckstr = null;
                            aCheckbyte = null;
                        }
                        else
                        {
                            nList.AddRange(aList);
                        }
                        aList.Clear();
                    }

                    lCheck++;
                }
                else
                {
                    if (bytes[x] > 127)
                    {
                        JP = true;
                        Constants.bJP = true;
                    }
                    lCheck = 1;
                    nList.Add(bytes[x]);
                }


            }

            Cleaned = HttpUtility.HtmlDecode(System.Text.Encoding.UTF8.GetString(nList.ToArray()));

            aList.Clear();
            bList.Clear();
            nList.Clear();

            return Cleaned;
        }

        #endregion
    }
}
