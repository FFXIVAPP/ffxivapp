using System;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Diagnostics;
using System.IO;
using System.Net;
using System.Web;
using System.Reflection;
using System.Windows.Threading;
using System.Xml.Linq;
using System.Linq;
using System.Windows.Markup;

using LogModXIV.Classes;
using LogModXIV.Windows;

using LogModXIV.Properties;

using AppModXIV;
using AppModXIV.Memory;

namespace LogModXIV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region " VARIABLES "

        AppModXIV.AutomaticUpdates autoUpdates = new AppModXIV.AutomaticUpdates();

        string SettingsStr = "Resources/Settings_Log.xml";

        public static MainWindow myWindow;

        Hashtable _offsets = GetLanguage();

        DateTime startTime;
        TimeSpan timeElapsed;
        int totalExp = 0;
        int totalSkill = 0;
        Boolean skillFound = false;
        Boolean TabFound = false;

        DispatcherTimer expTimer = new DispatcherTimer();
        private System.Windows.Forms.NotifyIcon MyNotifyIcon;

        myLanguages myLanguages = new myLanguages();
        HttpWebRequest HttpWReq;
        HttpWebResponse HttpWResp;
        Encoding resEncoding;

        public Color TS_Color;
        public Color B_Color;

        string Timestamp, mMessage, ChatMode, TempTranString;

        SolidColorBrush RawBrush = new SolidColorBrush();
        SolidColorBrush TSBrush = new SolidColorBrush();
        SolidColorBrush MessageBrush = new SolidColorBrush();
        SolidColorBrush LangBrush = new SolidColorBrush();
        SolidColorBrush TranBrush = new SolidColorBrush();
        SolidColorBrush RandomBrush = new SolidColorBrush();
        BrushConverter Conv = new BrushConverter();

        public static int Count = 0;

        public static RichTextBox[] NewText = new RichTextBox[1];
        public static ArrayList TabNames = new ArrayList();
        public static ListBox[] ChatScan = new ListBox[1];

        ChatWorker _worker;


        XDocument xATCodes = XDocument.Load("Resources/ATCodes.xml");
        XDocument xRawLog, xSettings;

        string fName, logName;

        #endregion

        #region " FORM OPEN-CLOSE "

        public MainWindow()
        {
            if (File.Exists("./Resources/Themes/LogMod.xaml"))
            {
                ResourceDictionary rd = (ResourceDictionary)XamlReader.Load(System.Xml.XmlReader.Create("./Resources/Themes/LogMod.xaml"));
                this.Resources.MergedDictionaries.Add(rd);
            }

            InitializeComponent();
            xSettings = XDocument.Load(SettingsStr);

            string lpath = "./Logs/LogMod/";
            if (!Directory.Exists(lpath))
            {
                DirectoryInfo di = Directory.CreateDirectory(lpath);
            }

            autoUpdates.currentversion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Func<bool> checkUpdates = () => autoUpdates.checkUpdates("LogModXIV");
            checkUpdates.BeginInvoke(result =>
            {
                if (checkUpdates.EndInvoke(result)) MessageBox.Show("Update Available! For Full Functionality, Please Goto https://secure.ffxiv-app.com/products/ & Download The Latest Release.", "Information!");
                autoUpdates.checkDLLs("AppModXIV", "");
            }, null);
        }

        void Restore_Click(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        void Exit_Click(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            myWindow = this;
            guiMyStatus.Text = "Loading Settings...";
            startTime = DateTime.Now;

            LoadXML();
            ApplyXML();

            GetPID();

            myLanguages.LoadLanguage("English");
            CheckLanguage();
            SetLanguage();

            Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Constants.FFXIVOpen == true)
            {
                DeleteXMLNode("Tab", xSettings, SettingsStr);
                MyNotifyIcon.Visible = false;
                stopLogging();

                if (TabNames.Count > 0)
                {
                    for (int i = 0; i <= TabNames.Count - 1; i++)
                    {
                        SaveXMLNode("Tab", xSettings, SettingsStr, "Settings", TabNames[i].ToString(), Constants.xTab[TabNames[i].ToString()]);
                    }
                }

                if (guiTSay.IsChecked == true)
                {
                    UpdateXMLNode("Translate", xSettings, SettingsStr, "Say", "1");
                }
                else
                {
                    UpdateXMLNode("Translate", xSettings, SettingsStr, "Say", "0");
                }

                if (guiTParty.IsChecked == true)
                {
                    UpdateXMLNode("Translate", xSettings, SettingsStr, "Party", "1");
                }
                else
                {
                    UpdateXMLNode("Translate", xSettings, SettingsStr, "Party", "0");
                }

                if (guiTTell.IsChecked == true)
                {
                    UpdateXMLNode("Translate", xSettings, SettingsStr, "Tell", "1");
                }
                else
                {
                    UpdateXMLNode("Translate", xSettings, SettingsStr, "Tell", "0");
                }

                if (guiTLS.IsChecked == true)
                {
                    UpdateXMLNode("Translate", xSettings, SettingsStr, "LS", "1");
                }
                else
                {
                    UpdateXMLNode("Translate", xSettings, SettingsStr, "LS", "0");
                }

                if (guiTShout.IsChecked == true)
                {
                    UpdateXMLNode("Translate", xSettings, SettingsStr, "Shout", "1");
                }
                else
                {
                    UpdateXMLNode("Translate", xSettings, SettingsStr, "Shout", "0");
                }

                UpdateXMLNode("Pos", xSettings, SettingsStr, "X", this.Left.ToString());
                UpdateXMLNode("Pos", xSettings, SettingsStr, "Y", this.Top.ToString());

                UpdateXMLNode("Size", xSettings, SettingsStr, "Height", this.Height.ToString());
                UpdateXMLNode("Size", xSettings, SettingsStr, "Width", this.Width.ToString());

                Settings.Default.Save();
            }
        }

        private void UpdateXMLNode(string mNode, XDocument xDoc, string fileName, string Key, string Value)
        {
            xDoc.Descendants(mNode).Where(x => x.Attribute("Key").Value == Key).Single().SetAttributeValue("Value", Value);
            xDoc.Save(fileName);
        }

        private void SaveXMLNode(string mNode, XDocument xDoc, string fileName, string mRoot, string Key, string Value)
        {
            xDoc.Element(mRoot).Add(new XElement(mNode, new XAttribute("Key", Key), new XAttribute("Value", Value)));
            xDoc.Save(fileName);
        }

        private void SaveChatNode(string mNode, XDocument xDoc, string fileName, string mRoot, string Key, string Value, string time)
        {
            xDoc.Element(mRoot).Add(new XElement(mNode, new XAttribute("Key", Key), new XAttribute("Value", Value), new XAttribute("Time", time)));
            xDoc.Save(fileName);
        }

        private void DeleteXMLNode(string mNode, XDocument xDoc, string fileName)
        {
            var q = from node in xDoc.Descendants(mNode)
                    select node;
            q.ToList().ForEach(x => x.Remove());
            xDoc.Save(fileName);
        }

        #endregion

        #region " START-UPS "

        private void GetPID()
        {
            Constants.FFXIV_PID = Process.GetProcessesByName("ffxivgame");

            if (Constants.FFXIV_PID.Length == 0)
            {
                MessageBox.Show("Please make sure FFXIV is fully loaded before running.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                Constants.FFXIVOpen = false;
                this.Close();
            }
            else
            {
                Constants.FFXIVOpen = true;

                foreach (Process proc in Constants.FFXIV_PID)
                {
                    guiPIDSelect.Items.Add(proc.Id);
                }

                guiPIDSelect.SelectedIndex = 0;

                SetPID();
            }
        }

        private void SetPID()
        {
            Constants.PID = Constants.FFXIV_PID[guiPIDSelect.SelectedIndex].Id;
            //Constants.FFXIV_PID[guiPIDSelect.SelectedIndex].PriorityClass = ProcessPriorityClass.High;
            startLogging();

            fName = "./Logs/LogMod/" + DateTime.Now.ToShortDateString().ToString().Replace("/", "_") + "_ID" + guiPIDSelect.SelectedIndex.ToString() + ".xml";
            logName = DateTime.Now.ToShortDateString().ToString().Replace("/", "_");

            if (File.Exists(fName))
            {
                xRawLog = XDocument.Load(fName);
            }
            else
            {
                xRawLog = new XDocument(new XComment("Chat Logging - " + logName), new XElement("Log"));
                xRawLog.Save(fName);
            }
        }

        private void LoadXML()
        {
            for (int i = 0; i < Constants_Loc.rSettings.Length; i++)
            {
                LoadSettingsXML(Constants_Loc.rSettings[i]);
            }

            var items = from item in xATCodes.Descendants("Code")
                        select new xValuePair { Key = (string)item.Attribute("Key"), Value = (string)item.Attribute("Value") };

            foreach (var item in items)
            {
                Constants.xATCodes.Add(item.Key, item.Value);
            }
        }

        private void LoadSettingsXML(string Setting)
        {
            var items = from item in xSettings.Descendants(Setting)
                        select new xValuePair { Key = (string)item.Attribute("Key"), Value = (string)item.Attribute("Value") };

            foreach (var item in items)
            {
                switch (Setting)
                {
                    case "Tab":
                        if (item.Key != null && item.Value != null)
                        {
                            Constants.xTab.Add(item.Key, item.Value);
                        }
                        break;
                    case "Color":
                        if (item.Key != null && item.Value != null)
                        {
                            Constants.xColor.Add(item.Key, item.Value);
                        }
                        break;
                    case "Pos":
                        if (item.Key != null && item.Value != null)
                        {
                            Constants.xPos.Add(item.Key, item.Value);
                        }
                        break;
                    case "Size":
                        if (item.Key != null && item.Value != null)
                        {
                            Constants.xSize.Add(item.Key, item.Value);
                        }
                        break;
                    //case "Memory":
                    //    if (item.Key != null && item.Value != null)
                    //    {
                    //        Constants.xMemory.Add(item.Key, item.Value);
                    //    }
                    //    break;
                    case "Translate":
                        if (item.Key != null && item.Value != null)
                        {
                            Constants.xTranslate.Add(item.Key, item.Value);
                        }
                        break;
                    case "Errors":
                        if (item.Key != null && item.Value != null)
                        {
                            Constants.xErrors.Add(item.Key, item.Value);
                        }
                        break;
                    case "TopMost":
                        if (item.Key != null && item.Value != null)
                        {
                            Constants_Loc.xTop.Add(item.Key, item.Value);
                        }
                        else
                        {
                            Constants_Loc.xTop.Add("Top", "1");
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void ApplyXML()
        {
            string[] SplitOfChatCodes;
            foreach (KeyValuePair<string, string> Tab in Constants.xTab)
            {
                if (Tab.Key != "" && Tab.Value != "")
                {
                    SplitOfChatCodes = Tab.Value.Split(',');
                    AddTabPageName(Tab.Key, SplitOfChatCodes);
                }
            }
            SplitOfChatCodes = null;

            guiChatControl.SelectedIndex = 1;

            try
            {
                Constants.LogErrors = int.Parse(Constants.xErrors["Log"]);
            }
            catch
            {
                Constants.LogErrors = 0;
            }

            foreach (KeyValuePair<string, string> Trans in Constants.xTranslate)
            {
                switch (Trans.Key)
                {
                    case "Say":
                        if (Trans.Value == "1")
                        {
                            guiTSay.IsChecked = true;
                        }
                        break;
                    case "Party":
                        if (Trans.Value == "1")
                        {
                            guiTParty.IsChecked = true;
                        }
                        break;
                    case "Tell":
                        if (Trans.Value == "1")
                        {
                            guiTTell.IsChecked = true;
                        }
                        break;
                    case "LS":
                        if (Trans.Value == "1")
                        {
                            guiTLS.IsChecked = true;
                        }
                        break;
                    case "Shout":
                        if (Trans.Value == "1")
                        {
                            guiTShout.IsChecked = true;
                        }
                        break;
                    default:
                        break;
                }
            }

            int PosX, PosY, SizeH, SizeW;

            try
            {
                PosX = Convert.ToInt32(Constants.xPos["X"]);
                PosY = Convert.ToInt32(Constants.xPos["Y"]);
            }
            catch
            {
                PosX = 200;
                PosY = 200;
            }

            Point Location = new Point(PosX, PosY);
            this.Left = Location.X;
            this.Top = Location.Y;

            try
            {
                SizeH = Convert.ToInt32(Constants.xSize["Height"]);
                SizeW = Convert.ToInt32(Constants.xSize["Width"]);
            }
            catch
            {
                SizeW = 200;
                SizeH = 200;
            }

            this.Width = SizeW;
            this.Height = SizeH;

            if (Constants_Loc.xTop["Top"] == "0")
            {
                myWindow.Topmost = false;
            }
        }

        private void Start()
        {
            UpdateColors();
            UpdateFonts();

            using (Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/LogModXIV;component/LogModXIV.ico")).Stream)
            {
                MyNotifyIcon = new System.Windows.Forms.NotifyIcon();
                MyNotifyIcon.Icon = new System.Drawing.Icon(iconStream);
                iconStream.Dispose();

                MyNotifyIcon.Text = "LogModXIV - Minimized";

                System.Windows.Forms.ContextMenu myNotify = new System.Windows.Forms.ContextMenu();

                myNotify.MenuItems.Add("&Restore Application");
                myNotify.MenuItems.Add("&Exit");
                myNotify.MenuItems[0].Click += new EventHandler(Restore_Click);
                myNotify.MenuItems[1].Click += new EventHandler(Exit_Click);
                MyNotifyIcon.ContextMenu = myNotify;

                MyNotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(MyNotifyIcon_MouseDoubleClick);
            }

            expTimer.Tick += expTimer_Tick;
            expTimer.Interval = TimeSpan.FromMilliseconds(100);
            expTimer.Start();
        }

        void expTimer_Tick(object sender, EventArgs e)
        {
            expTimer.Interval = TimeSpan.FromMilliseconds(1000);
            timeElapsed = DateTime.Now - startTime;
            int expPHour = Convert.ToInt32(totalExp / (timeElapsed.TotalMinutes / 60));
            int spPHour = Convert.ToInt32(totalSkill / (timeElapsed.TotalMinutes / 60));

            guiMyStatus.Text = "Totals ~ Exp: " + totalExp.ToString() + "   SP: " + totalSkill.ToString() + "   Exp/Hour: " + expPHour.ToString() + "   SP/Hour: " + spPHour.ToString();

            GC.Collect();
        }

        private void UpdateFonts()
        {
            if (Settings.Default.LM_Font != null)
            {
                System.Drawing.Font font = Settings.Default.LM_Font;

                guiAllLog.FontFamily = new System.Windows.Media.FontFamily(font.Name);
                guiAllLog.FontWeight = font.Bold ? System.Windows.FontWeights.Bold : System.Windows.FontWeights.Regular;
                guiAllLog.FontStyle = font.Italic ? System.Windows.FontStyles.Italic : System.Windows.FontStyles.Normal;
                guiAllLog.FontSize = font.Size;

                guiTransLog.FontFamily = new System.Windows.Media.FontFamily(font.Name);
                guiTransLog.FontWeight = font.Bold ? System.Windows.FontWeights.Bold : System.Windows.FontWeights.Regular;
                guiTransLog.FontStyle = font.Italic ? System.Windows.FontStyles.Italic : System.Windows.FontStyles.Normal;
                guiTransLog.FontSize = font.Size;

                guiRawLog.FontFamily = new System.Windows.Media.FontFamily(font.Name);
                guiRawLog.FontWeight = font.Bold ? System.Windows.FontWeights.Bold : System.Windows.FontWeights.Regular;
                guiRawLog.FontStyle = font.Italic ? System.Windows.FontStyles.Italic : System.Windows.FontStyles.Normal;
                guiRawLog.FontSize = font.Size;

                for (int a = 0; a <= TabNames.Count - 1; a++)
                {
                    NewText[a].FontFamily = new System.Windows.Media.FontFamily(font.Name);
                    NewText[a].FontWeight = font.Bold ? System.Windows.FontWeights.Bold : System.Windows.FontWeights.Regular;
                    NewText[a].FontStyle = font.Italic ? System.Windows.FontStyles.Italic : System.Windows.FontStyles.Normal;
                    NewText[a].FontSize = font.Size;
                }

                font.Dispose();
            }
        }

        private void UpdateColors()
        {
            TS_Color = Settings.Default.LM_TS_Color;
            B_Color = Settings.Default.B_Color;

            SolidColorBrush tColor = new SolidColorBrush();
            tColor.Color = B_Color;

            guiAllLog.Background = tColor;
            guiTransLog.Background = tColor;
            guiRawLog.Background = tColor;

            for (int a = 0; a <= TabNames.Count - 1; a++)
            {
                NewText[a].Background = tColor;
            }

            tColor = null;
        }

        #endregion

        #region " LOGGING CONTROL "

        public void startLogging()
        {
            Process p = Process.GetProcessById(Constants.PID);
            if (p != null)
            {
                Offsets o = new Offsets(p);

                _worker = new ChatWorker(p, o);
                _worker.OnNewline += _worker_OnNewline;
                _worker.OnRawline += _worker_OnRawline;
                _worker.StartLogging();
            }
        }

        private void stopLogging()
        {
            _worker.StopLogging();
            _worker = null;
        }

        #endregion

        #region " CHAT EVENT PARSING "

        private void AppendRTFText(string mText, SolidColorBrush mColor, RichTextBox mBox)
        {
            TextRange tr = new TextRange(mBox.Document.ContentEnd, mBox.Document.ContentEnd);
            tr.Text = mText;
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, mColor);
            mBox.CaretPosition = mBox.Document.ContentEnd;
            mBox.ScrollToEnd();

            tr = null;
        }

        private void _worker_OnRawline(string line)
        {
            if (guiShowRaw.IsChecked == true)
            {
                RawBrush.Color = Colors.White;

                string[] splitOfLine = line.Split(' ');
                string tmpString = "";

                for (int j = 0; j < splitOfLine.Length; j++)
                {
                    tmpString += (char)int.Parse(splitOfLine[j]);
                }

                if (guiPureRaw.IsChecked == true)
                {
                    AppendRTFText(line + "\n", RawBrush, guiRawLog);
                    AppendRTFText(tmpString + "\n\n", RawBrush, guiRawLog);
                }
                else
                {
                    AppendRTFText(tmpString + "\n", RawBrush, guiRawLog);
                }

                splitOfLine = null;
                tmpString = null;
            }
        }

        private void _worker_OnNewline(string line, Boolean JP)
        {
            //guiMyStatus.Text = "Currently Logging: " + guiPIDSelect.Text;
            Timestamp = DateTime.Now.ToString("[HH:mm:ss] ");
            mMessage = line.Substring(line.IndexOf(":") + 1);
            ChatMode = line.Substring(line.IndexOf(":") - 4, 4);

            if (mMessage.Substring(0, 1) == ":")
            {
                mMessage = mMessage.Substring(1);
            }

            if (guiSaveLogs.IsChecked == true)
            {
                SaveChatNode("Line", xRawLog, fName, "Log", ChatMode, mMessage, Timestamp.Replace(" ", ""));
            }

            if (guiStopLogging.IsChecked == false)
            {
                if (ChatMode == "0043" || ChatMode == "0042")
                {
                    string tmpmsg = mMessage.Substring(mMessage.IndexOf("gain") + 5);
                    tmpmsg = tmpmsg.Substring(0, tmpmsg.IndexOf(" "));
                    tmpmsg = tmpmsg.Replace(",", "");

                    if (mMessage.Contains("experience"))
                    {
                        skillFound = false;
                        totalExp += int.Parse(tmpmsg);
                    }
                    else if (mMessage.Contains("skill") && skillFound == false)
                    {
                        skillFound = true;
                        totalSkill += int.Parse(tmpmsg);
                    }

                    tmpmsg = null;
                }
                else
                {
                    skillFound = false;
                }

                TSBrush.Color = TS_Color;

                try
                {
                    MessageBrush = Conv.ConvertFromString("#" + Constants.xColor[ChatMode]) as SolidColorBrush;
                }
                catch
                {
                    MessageBrush = Conv.ConvertFromString("#FFFFFF") as SolidColorBrush;
                }

                TabFound = false;

                for (int a = 0; a <= ChatScan.Length - 2; a++)
                {
                    if (ChatScan[a].Items.Contains(ChatMode))
                    {
                        TabFound = true;
                        AppendRTFText(Timestamp, TSBrush, NewText[a]);
                        AppendRTFText(mMessage + "\n", MessageBrush, NewText[a]);
                    }
                }

                if (TabFound == false && guiShowAll.IsChecked == true)
                {
                    AppendRTFText(Timestamp, TSBrush, guiAllLog);
                    AppendRTFText(mMessage + "\n", MessageBrush, guiAllLog);
                }
            }

            if (guiTransGameText.IsChecked == true && JP == true && guiTransOnly.IsChecked == true)
            {
                if (CheckMode(ChatMode, Constants.CMSay))
                {
                    if (guiTSay.IsChecked == true)
                    {
                        RetreiveLang(mMessage, true, JP);
                    }
                }
                if (CheckMode(ChatMode, Constants.CMTell))
                {
                    if (guiTTell.IsChecked == true)
                    {
                        RetreiveLang(mMessage, true, JP);
                    }
                }
                if (CheckMode(ChatMode, Constants.CMParty))
                {
                    if (guiTParty.IsChecked == true)
                    {
                        RetreiveLang(mMessage, true, JP);
                    }
                }
                if (CheckMode(ChatMode, Constants.CMShout))
                {
                    if (guiTShout.IsChecked == true)
                    {
                        RetreiveLang(mMessage, true, JP);
                    }
                }
                if (CheckMode(ChatMode, Constants.CMLS))
                {
                    if (guiTLS.IsChecked == true)
                    {
                        RetreiveLang(mMessage, true, JP);
                    }
                }
            }
            else if (guiTransGameText.IsChecked == true && guiTransOnly.IsChecked == false)
            {
                if (CheckMode(ChatMode, Constants.CMSay))
                {
                    if (guiTSay.IsChecked == true)
                    {
                        RetreiveLang(mMessage, false, JP);
                    }
                }
                if (CheckMode(ChatMode, Constants.CMTell))
                {
                    if (guiTTell.IsChecked == true)
                    {
                        RetreiveLang(mMessage, false, JP);
                    }
                }
                if (CheckMode(ChatMode, Constants.CMParty))
                {
                    if (guiTParty.IsChecked == true)
                    {
                        RetreiveLang(mMessage, false, JP);
                    }
                }
                if (CheckMode(ChatMode, Constants.CMShout))
                {
                    if (guiTShout.IsChecked == true)
                    {
                        RetreiveLang(mMessage, false, JP);
                    }
                }
                if (CheckMode(ChatMode, Constants.CMLS))
                {
                    if (guiTLS.IsChecked == true)
                    {
                        RetreiveLang(mMessage, false, JP);
                    }
                }
            }
        }

        private Boolean CheckMode(string ChatMode, string[] Log)
        {
            for (int i = 0; i < Log.Length; i++)
            {
                if (Log[i] == ChatMode)
                {
                    return true;
                }
            }

            return false;
        }

        private void RetreiveLang(string rMessage, Boolean JPOnly, Boolean JP)
        {
            string mResults = "";
            string player = rMessage.Substring(0, rMessage.IndexOf(":")) + ": ";
            string tmpMessage = rMessage.Substring(rMessage.IndexOf(":") + 1);

            mResults = Translate(tmpMessage, JPOnly, JP);
            if (mResults.Length > 0 && rMessage != mResults)
            {
                LangBrush.Color = Colors.Green;
                TranBrush.Color = Colors.Yellow;

                AppendRTFText("[LANG] ", LangBrush, guiTransLog);
                guiTransLog.AppendText(player);
                AppendRTFText(mResults + "\n", TranBrush, guiTransLog);
            }

            mResults = player = tmpMessage = null;
        }

        #endregion

        #region " TRANSLATION "

        private string Translate(string message, Boolean JPOnly, Boolean JP)
        {
            TempTranString = "";

            if (JPOnly == true)
            {
                if (JP == true)
                {
                    TempTranString = TranslateText(message, "ja", _offsets[guiFinalLangCombo.Text.ToString()].ToString(), guiTransOnly.IsChecked, false);
                    return TempTranString;
                }
            }
            else
            {
                if (JP == true)
                {
                    TempTranString = TranslateText(message, "ja", _offsets[guiFinalLangCombo.Text.ToString()].ToString(), guiTransOnly.IsChecked, false);
                    return TempTranString;
                }
                else
                {
                    TempTranString = TranslateText(message, "en", _offsets[guiFinalLangCombo.Text.ToString()].ToString(), guiTransOnly.IsChecked, false);
                    return TempTranString;
                }
            }
            //return "Failed to Get Proper Translation";
            return "";
        }

        public string TranslateText(string TextToTranslate, string lngInput, string lngOutput, Boolean JPOnly, Boolean Romaji)
        {
            string result = string.Empty;
            string source = string.Empty;
            string roman = string.Empty;

            string contentEncoding;
            string charSet;

            try
            {
                string url = String.Format("http://translate.google.ca/translate_t?hl=&ie=UTF-8&text={0}&sl={1}&tl={2}#", TextToTranslate, lngInput, lngOutput);
                string bgl = string.Format("http://translate.google.ca/translate_t?hl=&ie=UTF-8&text={0}&sl=auto&tl={1}#auto|{1}|{0}", TextToTranslate, lngOutput);


                if (JPOnly == true)
                {
                    HttpWReq = (HttpWebRequest)WebRequest.Create(url);
                }
                else
                {
                    HttpWReq = (HttpWebRequest)WebRequest.Create(bgl);
                }

                HttpWResp = (HttpWebResponse)HttpWReq.GetResponse();

                resEncoding = Encoding.GetEncoding(HttpWResp.CharacterSet);
                StreamReader sr = new StreamReader(HttpWResp.GetResponseStream(), resEncoding);

                string textResponse = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();

                HttpWResp.Close();

                source = textResponse;
                textResponse = null;

                contentEncoding = HttpWResp.ContentEncoding;
                charSet = HttpWResp.CharacterSet;

                string StartStr = "id=result_box";
                string EndStr = "</span></span>";

                int i = source.IndexOf(StartStr) + 20;
                result = source.Substring(i);
                int f = result.IndexOf(EndStr);
                int f2 = f + 28;

                if (result.Substring(f, f2).Contains("<div id="))
                {
                    string EndString = "</div><div id=gt-res-dict";
                    int y = result.IndexOf("translit");

                    int x = result.Substring(y).IndexOf(EndString);
                    roman = result.Substring(y, x);
                    roman = sMid(roman, roman.IndexOf(">") + 1);

                    EndString = null;
                }

                result = result.Substring(0, f);
                result = result.Replace("</span>", "•");
                result = result.Replace("><span", "•");

                string[] NewResult = result.Split('•');
                result = "";

                for (i = 0; i <= NewResult.Length - 1; i++)
                {
                    if (!string.IsNullOrEmpty(NewResult[i]))
                    {
                        if (NewResult[i].Contains(">"))
                        {
                            string tmpResult = NewResult[i].ToString();
                            int tmpLength = NewResult[i].IndexOf(">") + 1;
                            result += tmpResult.Substring(tmpLength);

                            tmpResult = null;
                        }
                    }
                }

                StartStr = EndStr = null;
                url = bgl = source = null;
                NewResult = null;

                HttpWReq = null;
                resEncoding = null;
                HttpWResp = null;
            }
            catch (Exception e)
            {
                //if (Constants.LogErrors == 1)
                //{
                //    ErrorLogging.LogError(e.Message + e.StackTrace + e.InnerException);
                //}
                result = null;
                roman = null;
            }

            if (Romaji == true)
            {
                if (!string.IsNullOrEmpty(roman))
                {
                    result = HttpUtility.HtmlDecode(roman);
                }
                else
                {
                    result = HttpUtility.HtmlDecode(result);
                }
            }
            else
            {
                result = HttpUtility.HtmlDecode(result);
            }

            return result;
        }

        public static Hashtable GetLanguage()
        {
            Hashtable _offsets = new Hashtable();
            _offsets.Add("Albanian", "sq");
            _offsets.Add("Arabic", "ar");
            _offsets.Add("Bulgarian", "bg");
            _offsets.Add("Catalan", "ca");
            _offsets.Add("Chinese (Simplified)", "zh-CN");
            _offsets.Add("Chinese (Traditional)", "zh-TW");
            _offsets.Add("Croatian", "hr");
            _offsets.Add("Czech", "cs");
            _offsets.Add("Danish", "da");
            _offsets.Add("Dutch", "nl");
            _offsets.Add("English", "en");
            _offsets.Add("Estonian", "et");
            _offsets.Add("Filipino", "tl");
            _offsets.Add("Finnish", "fi");
            _offsets.Add("French", "fr");
            _offsets.Add("Galician", "gl");
            _offsets.Add("German", "de");
            _offsets.Add("Greek", "el");
            _offsets.Add("Hebrew", "iw");
            _offsets.Add("Hindi", "hi");
            _offsets.Add("Hungarian", "hu");
            _offsets.Add("Indonesian", "id");
            _offsets.Add("Italian", "it");
            _offsets.Add("Japanese", "ja");
            _offsets.Add("Korean", "ko");
            _offsets.Add("Latvian", "lv");
            _offsets.Add("Lithuanian", "lt");
            _offsets.Add("Maltese", "mt");
            _offsets.Add("Norwegian", "no");
            _offsets.Add("Polish", "pl");
            _offsets.Add("Portuguese", "pt");
            _offsets.Add("Romanian", "ro");
            _offsets.Add("Russian", "ru");
            _offsets.Add("Serbian", "sr");
            _offsets.Add("Slovak", "sk");
            _offsets.Add("Slovenian", "sl");
            _offsets.Add("Spanish", "es");
            _offsets.Add("Swedish", "sv");
            _offsets.Add("Thai", "th");
            _offsets.Add("Turkish", "tr");
            _offsets.Add("Ukrainian", "uk");
            _offsets.Add("Vietnamese", "vi");
            return _offsets;
        }

        #endregion

        #region " STRING CONTROLS "

        public static string sMid(string param, int startIndex)
        {
            return param.Substring(startIndex);
        }

        #endregion

        #region " MENU ITEMS "

        private void guiExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void guiLogFont_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FontDialog d = new System.Windows.Forms.FontDialog();

            try
            {
                if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Settings.Default.LM_Font = d.Font;
                    UpdateFonts();
                }
                else
                {
                    d.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Warning!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                d.Dispose();
            }
        }

        private void guiChatlogBGColor_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog d = new System.Windows.Forms.ColorDialog();

            try
            {
                if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    System.Drawing.Color mC = d.Color;

                    Settings.Default.B_Color = Color.FromRgb(mC.R, mC.G, mC.B);
                    UpdateColors();
                }
                else
                {
                    d.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Warning!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                d.Dispose();
            }
        }

        private void guiTSColor_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog d = new System.Windows.Forms.ColorDialog();

            try
            {
                if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    System.Drawing.Color mC = d.Color;

                    Settings.Default.LM_TS_Color = Color.FromRgb(mC.R, mC.G, mC.B);
                    UpdateColors();
                }
                else
                {
                    d.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Warning!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                d.Dispose();
            }
        }

        private void guiLAlbanian_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[0].ToString());
            SetLanguage();
        }

        private void guiLArabic_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[1].ToString());
            SetLanguage();
        }

        private void guiLBulgarian_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[2].ToString());
            SetLanguage();
        }

        private void guiLCatalan_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[3].ToString());
            SetLanguage();
        }

        private void guiLCSimplified_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[4].ToString());
            SetLanguage();
        }

        private void guiLCTraditional_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[5].ToString());
            SetLanguage();
        }

        private void guiLCroatian_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[6].ToString());
            SetLanguage();
        }

        private void guiLCzech_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[7].ToString());
            SetLanguage();
        }

        private void guiLDanish_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[8].ToString());
            SetLanguage();
        }

        private void guiLDutch_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[9].ToString());
            SetLanguage();
        }

        private void guiLEnglish_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[10].ToString());
            SetLanguage();
        }

        private void guiLEstonian_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[11].ToString());
            SetLanguage();
        }

        private void guiLFilipino_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[12].ToString());
            SetLanguage();
        }

        private void guiLFinnish_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[13].ToString());
            SetLanguage();
        }

        private void guiLFrench_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[14].ToString());
            SetLanguage();

        }

        private void guiLGalician_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[15].ToString());
            SetLanguage();
        }

        private void guiLGerman_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[16].ToString());
            SetLanguage();
        }

        private void guiLGreek_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[17].ToString());
            SetLanguage();
        }

        private void guiLHebrew_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[18].ToString());
            SetLanguage();
        }

        private void guiLHindi_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[19].ToString());
            SetLanguage();
        }

        private void guiLHungarian_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[20].ToString());
            SetLanguage();
        }

        private void guiLIndonesian_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[21].ToString());
            SetLanguage();
        }

        private void guiLItalian_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[22].ToString());
            SetLanguage();
        }

        private void guiLJapanese_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[23].ToString());
            SetLanguage();
        }

        private void guiLKorean_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[24].ToString());
            SetLanguage();
        }

        private void guiLLatvian_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[25].ToString());
            SetLanguage();
        }

        private void guiLLithuanian_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[26].ToString());
            SetLanguage();
        }

        private void guiLMaltese_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[27].ToString());
            SetLanguage();
        }

        private void guiLNorwegian_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[28].ToString());
            SetLanguage();
        }

        private void guiLPolish_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[29].ToString());
            SetLanguage();
        }

        private void guiLPortuguese_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[30].ToString());
            SetLanguage();
        }

        private void guiLRomanian_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[31].ToString());
            SetLanguage();
        }

        private void guiLRussian_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[32].ToString());
            SetLanguage();
        }

        private void guiLSerbian_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[33].ToString());
            SetLanguage();
        }

        private void guiLSlovak_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[34].ToString());
            SetLanguage();
        }

        private void guiLSlovenian_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[35].ToString());
            SetLanguage();
        }

        private void guiLSpanish_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[36].ToString());
            SetLanguage();
        }

        private void guiLSwedish_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[37].ToString());
            SetLanguage();
        }

        private void guiLThai_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[38].ToString());
            SetLanguage();
        }

        private void guiLTurkish_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[39].ToString());
            SetLanguage();
        }

        private void guiLUkrainian_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[40].ToString());
            SetLanguage();
        }

        private void guiLVietnamese_Click(object sender, RoutedEventArgs e)
        {
            myLanguages.LoadLanguage(Constants.rLanguages[41].ToString());
            SetLanguage();
        }

        private void guiAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(String.Format("Version: {0}", AssemblyVersion) + "\n" + AssemblyCopyright + "\n" + String.Format("Company: {0}", AssemblyCompany) + "\n" + String.Format("Description: {0}", AssemblyDescription), "About LogModXIV©", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        #endregion

        private void guiSetProcess_Click(object sender, RoutedEventArgs e)
        {
            stopLogging();
            SetPID();
        }

        private void guiRefreshList_Click(object sender, RoutedEventArgs e)
        {
            stopLogging();
            guiPIDSelect.Items.Clear();
            GetPID();
        }

        private void guiReset_Click(object sender, RoutedEventArgs e)
        {
            totalExp = 0;
            totalSkill = 0;
            startTime = DateTime.Now;
        }

        private void guiShowRaw_Click(object sender, RoutedEventArgs e)
        {
            if (guiShowRaw.IsChecked == true)
            {
                guiRawTab.Visibility = Visibility.Visible;
                guiPureRaw.IsEnabled = true;
            }
            else
            {
                guiChatControl.SelectedIndex = 1;
                guiRawTab.Visibility = Visibility.Collapsed;
                guiPureRaw.IsEnabled = false;
            }
        }

        private void guiShowAll_Click(object sender, RoutedEventArgs e)
        {
            if (guiShowAll.IsChecked == true)
            {
                guiAllTab.Visibility = Visibility.Visible;
            }
            else
            {
                guiChatControl.SelectedIndex = 1;
                guiAllTab.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region " TAB CONTROLS "

        private void guiButtonAddTabs_Click(object sender, RoutedEventArgs e)
        {
            TabUpdater TabUpdater = new TabUpdater();
            myWindow.Topmost = !(myWindow.Topmost);
            TabUpdater.ShowDialog();
            TabUpdater = null;
            myWindow.Topmost = !(myWindow.Topmost);
        }

        private void guiButtonDelTabs_Click(object sender, RoutedEventArgs e)
        {
            DelTabPage();
        }

        public void AddTabPageName(string NameOfTab, string[] SplitOfChatCodes)
        {
            string ChatString = "";
            for (int i = 0; i <= SplitOfChatCodes.Length - 1; i++)
            {
                if (i == SplitOfChatCodes.Length - 1)
                {
                    ChatString += SplitOfChatCodes[i].ToString();
                }
                else
                {
                    ChatString += SplitOfChatCodes[i].ToString() + ",";
                }
            }

            string NameCheck = "";
            try
            {
                NameCheck = Constants.xTab[NameOfTab];
            }
            catch
            {
                Constants.xTab.Add(NameOfTab, ChatString);
            }

            NameCheck = ChatString = null;

            TabItem NewTab = new TabItem();
            RichTextBox NewTextBox = new RichTextBox();
            ListBox NewListBox = new ListBox();

            Array.Resize(ref NewText, (NewText).GetUpperBound(0) + 2);
            Array.Resize(ref ChatScan, (ChatScan).GetUpperBound(0) + 2);

            NewText[Count] = NewTextBox;
            ChatScan[Count] = NewListBox;

            NewText[Count].SetValue(Paragraph.LineHeightProperty, 0.5);

            for (int i = 0; i <= SplitOfChatCodes.Length - 1; i++)
            {
                ChatScan[Count].Items.Add(SplitOfChatCodes[i]);
            }

            NewTab.Name = NameOfTab;
            NewTab.Header = NewTab.Name;

            NewText[Count].Name = NameOfTab;

            guiChatControl.Items.Add(NewTab);
            NewTab.Content = NewText[Count];

            if (Settings.Default.LM_Font != null)
            {
                System.Drawing.Font f = Settings.Default.LM_Font;

                NewText[Count].FontFamily = new System.Windows.Media.FontFamily(f.Name);
                NewText[Count].FontWeight = f.Bold ? System.Windows.FontWeights.Bold : System.Windows.FontWeights.Regular;
                NewText[Count].FontStyle = f.Italic ? System.Windows.FontStyles.Italic : System.Windows.FontStyles.Normal;
                NewText[Count].FontSize = f.Size;

                f.Dispose();
            }

            NewText[Count].Background = Brushes.Black;
            NewText[Count].Foreground = Brushes.White;
            NewText[Count].VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            guiChatControl.SelectedIndex = guiChatControl.Items.Count - 1;

            TabNames.Add(NameOfTab);

            NewTab = null;
            NewTextBox = null;
            NewListBox = null;

            Count += 1;
        }

        private void DelTabPage()
        {
            if (guiChatControl.SelectedIndex == 0 || guiChatControl.SelectedIndex == 1 || guiChatControl.SelectedIndex == 2)
            {
                return;
            }
            else
            {
                if (TabNames.Count > 0)
                {
                    for (int i2 = guiChatControl.SelectedIndex - 3; i2 <= (NewText).GetUpperBound(0) - 1; i2++)
                    {
                        NewText[i2] = NewText[i2 + 1];
                    }
                    Array.Resize(ref NewText, (NewText).GetUpperBound(0));

                    for (int i2 = guiChatControl.SelectedIndex - 3; i2 <= (ChatScan).GetUpperBound(0) - 1; i2++)
                    {
                        ChatScan[i2] = ChatScan[i2 + 1];
                    }
                    Array.Resize(ref ChatScan, (ChatScan).GetUpperBound(0));

                    Constants.xTab.Remove(TabNames[guiChatControl.SelectedIndex - 3].ToString());
                    TabNames.RemoveAt(guiChatControl.SelectedIndex - 3);
                    guiChatControl.Items.RemoveAt(guiChatControl.SelectedIndex);

                    if (TabNames.Count == 0)
                    {
                        guiChatControl.SelectedIndex = 1;
                    }
                    else
                    {
                        guiChatControl.SelectedIndex = guiChatControl.Items.Count - 1;
                    }

                    Count--;
                }
            }
        }

        #endregion

        #region " LOGMOD OPTIONS "

        void MyNotifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                MyNotifyIcon.Visible = true;
                this.WindowStyle = System.Windows.WindowStyle.ToolWindow;
            }
            else if (this.WindowState == WindowState.Normal)
            {
                MyNotifyIcon.Visible = false;
                this.ShowInTaskbar = true;
                this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
            }
        }

        private void guiButtonTrans_Click(object sender, RoutedEventArgs e)
        {
            ChatterTrans();
        }

        private void Chatter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ChatterTrans();
            }
        }

        private void ChatterTrans()
        {
            string tmpTranString;

            tmpTranString = TranslateText(Chatter.Text, _offsets[guiTFrom.Text.ToString()].ToString(), _offsets[guiTTo.Text.ToString()].ToString(), guiTransOnly.IsChecked, guiSendRomaji.IsChecked);
            Clipboard.SetText(tmpTranString);

            Chatter.Clear();
            Chatter.Text = tmpTranString;

            tmpTranString = null;
        }

        private void guiSendTextToGame_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region " GUI LANGUAGE "

        private void CheckLanguage()
        {
            for (int i = 0; i < Constants.rLanguages.Length - 1; i++)
            {
                Boolean result;
                string mLang = Constants.rLanguages[i].ToString();
                if (mLang == "Simplified" | mLang == "Traditional") { mLang = "C" + mLang; }
                var myMenuItem = (MenuItem)this.FindName("guiL" + mLang);

                result = myLanguages.LoadLanguage(Constants.rLanguages[i].ToString(), i);

                if (result == false)
                {
                    myMenuItem.Visibility = Visibility.Collapsed;
                }

                mLang = null;
                myMenuItem = null;
            }
        }

        private void SetLanguage()
        {
            guiFileMenu.Header = Constants.LangString["0"].ToString();
            guiExit.Header = Constants.LangString["1"].ToString(); ;
            guiToolsMenu.Header = Constants.LangString["2"].ToString(); ;
            guiOptionsMenu.Header = Constants.LangString["3"].ToString(); ;
            guiLanguage.Header = Constants.LangString["4"].ToString(); ;
            guiLEnglish.Header = Constants.LangString["5"].ToString(); ;
            guiLJapanese.Header = Constants.LangString["6"].ToString();
            guiStopLogging.Header = Constants.LangString["7"].ToString();
            guiLogFont.Header = Constants.LangString["8"].ToString();
            guiChatlogBGColor.Header = Constants.LangString["9"].ToString();
            guiTSColor.Header = Constants.LangString["10"].ToString();
            guiTranslateMenu.Header = Constants.LangString["11"].ToString();
            guiTransGameText.Header = Constants.LangString["12"].ToString();
            guiSendTextToGame.Header = Constants.LangString["13"].ToString();
            guiSendRomaji.Header = Constants.LangString["14"].ToString();
            guiChatToTransMenu.Header = Constants.LangString["15"].ToString();
            guiTSay.Header = Constants.LangString["16"].ToString();
            guiTTell.Header = Constants.LangString["17"].ToString();
            guiTParty.Header = Constants.LangString["18"].ToString();
            guiTLS.Header = Constants.LangString["19"].ToString();
            guiTShout.Header = Constants.LangString["20"].ToString();
            guiTransOnly.Header = Constants.LangString["21"].ToString();
            guiTransOnly.IsChecked = true;
            guiTransTo.Header = Constants.LangString["22"].ToString();
            guiHelpMenu.Header = Constants.LangString["23"].ToString();
            guiAbout.Header = Constants.LangString["24"].ToString();
            guiCharMenu.Header = Constants.LangString["25"].ToString();
            guiSetProcess.Header = Constants.LangString["26"].ToString();
            guiRefreshList.Header = Constants.LangString["27"].ToString();
            guiLabelFrom.Content = Constants.LangString["28"].ToString();
            guiLabelTo.Content = Constants.LangString["29"].ToString();
            guiFinalLangCombo.SelectedIndex = Convert.ToInt32(Constants.LangString["30"]);
            guiTFrom.SelectedIndex = Convert.ToInt32(Constants.LangString["31"]);
            guiTTo.SelectedIndex = Convert.ToInt32(Constants.LangString["32"]);
            guiPureRaw.Header = Constants.LangString["33"];
            guiShowRaw.Header = Constants.LangString["34"];
            guiShowAll.Header = Constants.LangString["35"];
            guiSaveLogs.Header = Constants.LangString["36"];
        }

        #endregion

        #region " RANDOM COLOR "

        private Random random;

        private SolidColorBrush GetRandomColor()
        {
            random = new Random();

            byte Red = (byte)random.Next(0, 255);
            byte Green = (byte)random.Next(0, 255);
            byte Blue = (byte)random.Next(0, 255);

            RandomBrush.Color = Color.FromRgb(Red, Green, Blue);

            return RandomBrush;
        }

        #endregion

    }

    public class xValuePair
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
