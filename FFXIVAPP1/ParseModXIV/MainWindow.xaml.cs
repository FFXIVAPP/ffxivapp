using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
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
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;

using System.Collections;
using System.Windows.Markup;

using ParseModXIV.Classes;
using ParseModXIV.Stats;

using AppModXIV.Memory;
using AppModXIV;

namespace ParseModXIV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Boolean DebugChat = true;

        #region " VARIABLES "

        AppModXIV.AutomaticUpdates autoUpdates = new AppModXIV.AutomaticUpdates();
        List<EventMonitor> monitors = new List<EventMonitor>();

        HttpWebRequest HttpWReq;
        HttpWebResponse HttpWResp;
        Encoding resEncoding;

        string SettingsStr = "Resources/Settings_Parse.xml";
        Boolean Started = false;
        public static MainWindow myWindow;
        CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
        TextInfo textInfo;

        private System.Windows.Forms.NotifyIcon MyNotifyIcon;
        myLanguages myLanguages = new myLanguages();

        public string Server = "";
        string Timestamp, mMessage, ChatMode;

        public static int Count = 0;

        Boolean Parsed;
        ChatWorker _worker;

        XDocument xATCodes = XDocument.Load("Resources/ATCodes.xml");
        XDocument xSettings;


        DateTime startTime;
        TimeSpan timeElapsed;

        #endregion

        #region " EVENTS AND MONITORS "

        class LogBoxEventMonitor : EventMonitor
        {
            public RichTextBox output { get; set; }
            private System.IO.StreamWriter outFile = new System.IO.StreamWriter(@"Logs\AllEvents.txt");
            public LogBoxEventMonitor()
                : base("LogBox")
            {
                Filter = EventParser.ALL_EVENTS;
            }

            protected override void HandleEvent(Event e)
            {
                Color logColor = Colors.Black;
                switch (e.Type)
                {
                    case EventType.Attack: logColor = Colors.BlueViolet; break;
                    case EventType.Buff: logColor = Colors.Brown; break;
                    case EventType.Crafting: logColor = Colors.BurlyWood; break;
                    case EventType.Debuff: logColor = Colors.CadetBlue; break;
                    case EventType.Gathering: logColor = Colors.Chartreuse; break;
                    case EventType.Heal: logColor = Colors.Chocolate; break;
                    case EventType.SkillPoints: logColor = Colors.Coral; break;
                }
                AppendRTFText(String.Format("{0} {1} {2} : {3}", e.Type, e.Direction, e.Subject, e.RawLine), logColor, output);
            }
        }

        protected void OnDPSChange(object src, Event e)
        {
            Monitors.DamageMonitor dm = (Monitors.DamageMonitor)src;
            //AppendRTFText(String.Format("Total Damage : {0} [{1}]", dm.TOTAL, e.RawLine), Colors.Green, guiLog);
        }

        protected void OnUnknownEvent(object src, Event e)
        {
            AppendRTFText(String.Format("Unknown event! [Code : {0:x4}] {1}", e.Code, e.RawLine), Colors.Red, guiLog);
        }

        protected void OnStatChanged(object src, StatChangedEvent e)
        {

        }

        #endregion

        #region " FORM LOADS AND CLOSES "

        public MainWindow()
        {
            if (File.Exists("./Resources/Themes/ParseMod.xaml"))
            {
                ResourceDictionary rd = (ResourceDictionary)XamlReader.Load(System.Xml.XmlReader.Create("./Resources/Themes/ParseMod.xaml"));
                this.Resources.MergedDictionaries.Add(rd);
            }

            InitializeComponent();

            Monitors.DamageMonitor damageMonitor = new Monitors.DamageMonitor();
            var timelineMonitor = new Monitors.TimelineMonitor();

            monitors.Add(damageMonitor);
            //monitors.Add(new LogBoxEventMonitor() { output = guiLog });

            EventParser.Instance.OnUnknownLogEvent += OnUnknownEvent;

            textInfo = cultureInfo.TextInfo;

            xSettings = XDocument.Load(SettingsStr);

            string lpath = "./Logs/ParseMod/";
            if (!Directory.Exists(lpath))
            {
                DirectoryInfo di = Directory.CreateDirectory(lpath);
            }

            autoUpdates.currentversion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Func<bool> checkUpdates = () => autoUpdates.checkUpdates("ParseModXIV");
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
            guiMyStatus.Text = "Loading Settings & Checking Updates...";
            startTime = DateTime.Now;

            LoadXML();
            ApplyXML();

            myLanguages.LoadLanguage("English");
            CheckLanguage();
            SetLanguage();

            Start();

            guiMyStatus.Text = "Settings Loaded. Ready to Begin.";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Constants.FFXIVOpen == true)
            {
                stopLogging();
            }

            DeleteXMLNode("Tab", xSettings, SettingsStr);
            MyNotifyIcon.Visible = false;

            UpdateXMLNode("Pos", xSettings, SettingsStr, "X", this.Left.ToString());
            UpdateXMLNode("Pos", xSettings, SettingsStr, "Y", this.Top.ToString());

            UpdateXMLNode("Size", xSettings, SettingsStr, "Height", this.Height.ToString());
            UpdateXMLNode("Size", xSettings, SettingsStr, "Width", this.Width.ToString());
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
            startLogging();
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
                    case "Errors":
                        if (item.Key != null && item.Value != null)
                        {
                            Constants.xErrors.Add(item.Key, item.Value);
                        }
                        break;
                    case "Name":
                        if (item.Key != null && item.Value != null)
                        {
                            Constants_Loc.xName.Add(item.Key, item.Value);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void ApplyXML()
        {
            try
            {
                Constants.LogErrors = int.Parse(Constants.xErrors["Log"]);
            }
            catch
            {
                Constants.LogErrors = 0;
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

            String fname, lname;
            if (Constants_Loc.xName.TryGetValue("First", out fname))
            {
                guiYourFName.Text = fname;
            }
            if (Constants_Loc.xName.TryGetValue("Last", out lname))
            {
                guiYourLName.Text = lname;
            }
            Constants_Loc.xName.TryGetValue("Server", out Server);

            int i = 0;
            foreach (ComboBoxItem item in guiServerName.Items)
            {
                if (item.Content.ToString() == Server)
                {
                    Server = (i + 1).ToString();
                    guiServerName.SelectedIndex = i++;
                    break;
                }
                i++;
            }

            //GetAvatars(guiYourFName.Text, guiYourLName.Text, Server);

            this.Width = SizeW;
            this.Height = SizeH;
        }

        private void Start()
        {
            using (Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/ParseModXIV;component/ParseModXIV.ico")).Stream)
            {
                MyNotifyIcon = new System.Windows.Forms.NotifyIcon();
                MyNotifyIcon.Icon = new System.Drawing.Icon(iconStream);
                iconStream.Dispose();

                MyNotifyIcon.Text = "ParseModXIV - Minimized";

                System.Windows.Forms.ContextMenu myNotify = new System.Windows.Forms.ContextMenu();

                myNotify.MenuItems.Add("&Restore Application");
                myNotify.MenuItems.Add("&Exit");
                myNotify.MenuItems[0].Click += new EventHandler(Restore_Click);
                myNotify.MenuItems[1].Click += new EventHandler(Exit_Click);
                MyNotifyIcon.ContextMenu = myNotify;

                MyNotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(MyNotifyIcon_MouseDoubleClick);
            }
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

        private static void AppendRTFText(string mText, Color mColor, RichTextBox mBox)
        {
            mBox.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                new DispatcherOperationCallback(delegate
                {
                    System.Windows.Documents.Run run = new System.Windows.Documents.Run(mText + "\n", mBox.Selection.End);
                    TextRange tr = new TextRange(run.ContentStart, run.ContentEnd);
                    tr.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(mColor));
                    var para = new Paragraph();
                    para.Inlines.Add(run);
                    mBox.Document.Blocks.Add(para);
                    mBox.Selection.Select(run.ContentStart, run.ContentEnd);
                    mBox.ScrollToEnd();
                    return null;
                }), null);

        }
        private void _worker_OnRawline(string line)
        {

        }

        private void _worker_OnNewline(string line, Boolean JP)
        {
            Parsed = false;

            Timestamp = DateTime.Now.ToString("[HH:mm:ss] ");
            mMessage = line.Substring(line.IndexOf(":") + 1);
            ChatMode = line.Substring(line.IndexOf(":") - 4, 4);

            if (mMessage.Substring(0, 1) == ":")
            {
                mMessage = mMessage.Substring(1);
            }

            if (guiPauseLogging.IsChecked == false)
            {
                EventParser.Instance.ParseAndPublish(Convert.ToUInt16(ChatMode, 16), mMessage);
            }

            if (!Parsed)
            {
                //SaveChatNode("Line", xRawLog, fName, "Log", ChatMode, mMessage, Timestamp.Replace(" ", ""));
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
            }
            else if (this.WindowState == WindowState.Normal)
            {
                MyNotifyIcon.Visible = false;
                this.ShowInTaskbar = true;
            }
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
            guiStart.Content = Constants.LangString["0"].ToString();
            guiFileMenu.Header = Constants.LangString["1"].ToString();
            guiExit.Header = Constants.LangString["2"].ToString();
            guiToolsMenu.Header = Constants.LangString["3"].ToString();
            guiOptionsMenu.Header = Constants.LangString["4"].ToString();
            guiPauseLogging.Header = Constants.LangString["5"].ToString();
            guiLanguage.Header = Constants.LangString["6"].ToString();
            guiHelpMenu.Header = Constants.LangString["7"].ToString();
            guiAbout.Header = Constants.LangString["8"].ToString();
            guiCharMenu.Header = Constants.LangString["9"].ToString();
            guiSetProcess.Header = Constants.LangString["10"].ToString();
            guiRefreshList.Header = Constants.LangString["11"].ToString();
        }

        #endregion

        #region " MENU ITEMS "

        private void guiExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
            MessageBox.Show(String.Format("Version: {0}", AssemblyVersion) + "\n" + AssemblyCopyright + "\n" + String.Format("Company: {0}", AssemblyCompany) + "\n" + String.Format("Description: {0}", AssemblyDescription), "About ParseModXIV©", MessageBoxButton.OK, MessageBoxImage.Information);
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

        #endregion

        private void guiStart_Click(object sender, RoutedEventArgs e)
        {
            if (DebugChat)
            {
                var items = from item in XDocument.Load("Resources/Sample.xml").Descendants("Line")
                            select new xValuePair { Key = (string)item.Attribute("Key"), Value = (string)item.Attribute("Value") };
                
                foreach (var item in items)
                {
                    var code = Convert.ToUInt16(item.Key, 16);
                    var line = item.Value;
                    Func<bool> d = delegate()
                        {
                            EventParser.Instance.ParseAndPublish(code, line);
                            return true;
                        };
                    d.BeginInvoke(null, null);
                   //EventParser.Instance.ParseAndPublish(Convert.ToUInt16(item.Key, 16), item.Value);
                }
            }
            else
            {
                if (guiStart.Content.ToString() == "Start Logging")
                {
                    guiStart.Content = "Stop Logging";

                    if (!Started)
                    {
                        Started = true;
                        GetPID();
                    }
                    else
                    {
                        SetPID();
                    }

                    guiCharMenu.IsEnabled = true;
                }
                else
                {
                    guiStart.Content = "Start Logging";
                    guiCharMenu.IsEnabled = false;

                    stopLogging();
                }
            }
        }

        public void GetAvatars(string FirstName, string LastName, string WorldNum, Image MyImage, GroupBox MyName)
        {
            string result = string.Empty;
            string source = string.Empty;

            string lpath = "./Logs/Avatars/";
            if (!Directory.Exists(lpath))
            {
                DirectoryInfo di = Directory.CreateDirectory(lpath);
            }

            if (!File.Exists("./Logs/Avatars/" + FirstName + LastName + ".png"))
            {
                try
                {
                    string url = String.Format("http://lodestone.finalfantasyxiv.com/rc/search/search?tgt=77&q={0}+{1}&cms=&cw={2}", FirstName, LastName, WorldNum);

                    HttpWReq = (HttpWebRequest)HttpWebRequest.Create(url);
                    HttpWReq.Method = "GET";
                    HttpWReq.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.70 Safari/533.4";
                    HttpWResp = (HttpWebResponse)HttpWReq.GetResponse();

                    resEncoding = Encoding.GetEncoding(HttpWResp.CharacterSet);
                    StreamReader sr = new StreamReader(HttpWResp.GetResponseStream(), resEncoding);

                    string textResponse = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();

                    HttpWResp.Close();

                    source = textResponse;
                    textResponse = null;

                    string StartStr = "http://static2.finalfantasyxiv.com/csnap_sp/";

                    int i = source.IndexOf(StartStr);
                    result = source.Substring(i);
                    result = result.Substring(0, result.IndexOf(".png") + 4);

                    HttpWReq = null;
                    resEncoding = null;
                    HttpWResp = null;

                    if (File.Exists("./Logs/Avatars/" + FirstName + LastName + ".png") == false)
                    {
                        Uri urlUri = new Uri(result);
                        HttpWReq = (HttpWebRequest)HttpWebRequest.CreateDefault(urlUri);
                        HttpWReq.Method = "GET";
                        HttpWReq.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.70 Safari/533.4";

                        byte[] buffer = new byte[4096];

                        using (var target = new FileStream("./Logs/Avatars/" + FirstName + LastName + ".png", FileMode.Create, FileAccess.Write))
                        {
                            using (HttpWResp = (HttpWebResponse)HttpWReq.GetResponse())
                            {
                                using (var stream = HttpWResp.GetResponseStream())
                                {
                                    int read;

                                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                                    {
                                        target.Write(buffer, 0, read);
                                    }
                                }
                            }
                        }

                        HttpWReq = null;
                        resEncoding = null;
                        HttpWResp = null;
                    }

                    System.Drawing.Bitmap _image = new System.Drawing.Bitmap("./Logs/Avatars/" + FirstName + LastName + ".png");
                    MyImage.Source = GetBitmapSource(_image);
                    MyName.Header = FirstName + " " + LastName;
                    MyName.Visibility = Visibility.Visible;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    result = null;
                }
            }
            else
            {
                System.Drawing.Bitmap _image = new System.Drawing.Bitmap("./Logs/Avatars/" + FirstName + LastName + ".png");
                MyImage.Source = GetBitmapSource(_image);
                MyName.Header = FirstName + " " + LastName;
                MyName.Visibility = Visibility.Visible;
            }
        }

        private BitmapSource GetBitmapSource(System.Drawing.Bitmap _image)
        {
            System.Drawing.Bitmap bitmap = _image;
            System.Windows.Media.Imaging.BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bitmap.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            return bitmapSource;
        }

        private void guiYourFName_GotFocus(object sender, RoutedEventArgs e)
        {
            guiYourFName.Text = "";
        }

        private void guiYourLName_GotFocus(object sender, RoutedEventArgs e)
        {
            guiYourLName.Text = "";
        }
    }

    public class xValuePair
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
