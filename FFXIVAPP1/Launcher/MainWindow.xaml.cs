using System;
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
using System.Runtime.InteropServices;

using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Windows.Threading;

using Launcher.Properties;

using AppModXIV;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region " VARIABLES "
        AutomaticUpdates autoUpdates = new AutomaticUpdates();
        private BitField bitField;
        OpenFileDialog ofd = new OpenFileDialog();

        Process[] proc;
        Process processById;

        Globals.RECT orig = new Globals.RECT();
        Boolean useFullScreen = false;
        Boolean CanSave = false;

        public static TARGETMAP TargetMap = new TARGETMAP();

        StringBuilder sb;
        string result;
        int temp_res;

        string my_path = AppDomain.CurrentDomain.BaseDirectory;
        string settings;

        public static MainWindow myWindow;

        private System.Windows.Forms.NotifyIcon MyNotifyIcon = new System.Windows.Forms.NotifyIcon();
        System.Windows.Forms.ContextMenu myNotify = new System.Windows.Forms.ContextMenu();

        string myFlags;
        public static string pName;
        public static int gBorder, gTop, gHor, gVer;

        DispatcherTimer procTimer = new DispatcherTimer();
        DispatcherTimer quitTimer = new DispatcherTimer();

        #endregion

        #region " CONSTRUCTOR "

        public MainWindow()
        {
            InitializeComponent();
            proc = Process.GetProcessesByName("launcher");
            if (proc.Length > 1)
            {
                Environment.Exit(0);
            }

            bitField = new BitField(BitField.Flag.Clear);

            autoUpdates.currentversion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Func<bool> checkUpdates = () => autoUpdates.checkUpdates("Launcher");
            checkUpdates.BeginInvoke(result =>
            {
                if (checkUpdates.EndInvoke(result)) MessageBox.Show("Update Available! For Full Functionality, Please Goto https://secure.ffxiv-app.com/products/ & Download The Latest Release.", "Information!");
                autoUpdates.checkDLLs("AppModXIV", "");
                Constants.DLLVersion = Marshal.PtrToStringAnsi(Globals.GetDllVersion());
                autoUpdates.checkDLLs("WinModXIV", Constants.DLLVersion);
            }, null);
        }

        #endregion

        #region " LOAD 'n' CLOSE "

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            myWindow = this;
            Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            if (CanSave)
            {
                SaveSettings();

                Globals.EndHook();

                LoadINI();
                LoadOptions();

                SetupHook();
            }
            e.Cancel = true;
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

        private void Window_Closed(object sender, EventArgs e)
        {
            MyNotifyIcon.Visible = false;
            Globals.EndHook();
        }

        #endregion

        #region " OPENING FUNCTIONS "

        private void Start()
        {
            Boolean Valid = true;

            SetupNotify();

            String[] args = App.mArgs;
            if (args != null)
            {
                settings = my_path + args[0].Replace("/", "\\") + ".ini";

                if (File.Exists(settings))
                {
                    LoadINI();
                    LoadOptions();

                    if (Valid)
                    {
                        SetupHook();
                        Launch();
                    }

                    this.WindowState = WindowState.Minimized;
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            else
            {
                Environment.Exit(0);
            }

            procTimer.Tick += new EventHandler(procTimer_Tick);
            procTimer.Interval = TimeSpan.FromMilliseconds(100);
            procTimer.Start();

            quitTimer.Tick += new EventHandler(quitTimer_Tick);
            quitTimer.Interval = TimeSpan.FromMilliseconds(3000);
        }

        void procTimer_Tick(object sender, EventArgs e)
        {
            AppModXIV.Constants.FFXIV_PID = Process.GetProcessesByName(pName);
            if (AppModXIV.Constants.FFXIV_PID.Length > 0)
            {
                GetBorders();
                if (useFullScreen)
                {
                    Fullscreen();
                }
            }
            GC.Collect();
        }

        void quitTimer_Tick(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
            GC.Collect();
        }

        public static Boolean IsNumeric(String input, NumberStyles numberStyle)
        {
            Double temp;
            Boolean result = Double.TryParse(input, numberStyle, CultureInfo.CurrentCulture, out temp);
            return result;
        }

        #endregion

        #region " LOAD 'n' SAVE OPTIONS "

        private void LoadINI()
        {
            pName = gui_HookPath.Text = result = GetValue("target", "path");
            TargetMap.path = result;

            gui_LaunchPath.Text = GetValue("target", "game");
            pName = pName.Substring(pName.LastIndexOf("\\") + 1);
            pName = pName.Substring(0, pName.IndexOf("."));

            if (gui_LaunchPath.Text == gui_HookPath.Text)
            {
                gui_UseHookPath.IsChecked = true;
            }

            result = GetValue("target", "fullscreen");
            useFullScreen = Convert.ToBoolean(Int32.Parse(result));

            TargetMap.dxversion = 0;

            myFlags = result = GetValue("target", "flag");
            TargetMap.flags = Int32.Parse(result);

            gui_InitX.Text = result = GetValue("target", "initx");
            TargetMap.initx = Int32.Parse(result);

            gui_InitY.Text = result = GetValue("target", "inity");
            TargetMap.inity = Int32.Parse(result);

            gui_MinX.Text = result = GetValue("target", "minx");
            TargetMap.minx = Int32.Parse(result);

            gui_MinY.Text = result = GetValue("target", "miny");
            TargetMap.miny = Int32.Parse(result);

            gui_MaxX.Text = result = GetValue("target", "maxx");
            TargetMap.maxx = Int32.Parse(result);

            gui_MaxY.Text = result = GetValue("target", "maxy");
            TargetMap.maxy = Int32.Parse(result);
        }

        private void SaveSettings()
        {
            SetValue("target", "path", gui_HookPath.Text);
            SetValue("target", "game", gui_LaunchPath.Text);
            SetValue("target", "flag", myFlags);
            SetValue("target", "initx", gui_InitX.Text);
            SetValue("target", "inity", gui_InitY.Text);
            SetValue("target", "minx", gui_MinX.Text);
            SetValue("target", "miny", gui_MinY.Text);
            SetValue("target", "maxx", gui_MaxX.Text);
            SetValue("target", "maxy", gui_MaxY.Text);
        }

        #endregion

        #region " HOOKER STUFF "

        private void SetupHook()
        {
            Globals.SetTarget(ref TargetMap);
            Globals.StartHook();
        }

        #endregion

        #region " INI CONTROL "

        private string GetValue(string category, string key)
        {
            sb = new StringBuilder(500);
            temp_res = Globals.GetPrivateProfileString(category, key, "0", sb, (int)sb.Capacity, settings);
            result = sb.ToString();
            sb = null;

            return result;
        }

        private void SetValue(string category, string key, string value)
        {
            Globals.WritePrivateProfileString(category, key, value, settings);
        }

        #endregion

        #region " NOTIFY OPTIONS "

        private void SetupNotify()
        {
            Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/Launcher;component/Launcher.ico")).Stream;
            MyNotifyIcon.Icon = new System.Drawing.Icon(iconStream);
            iconStream.Dispose();

            MyNotifyIcon.Text = "Launcher - Minimized";

            myNotify.MenuItems.Add("&Configure Launcher");
            myNotify.MenuItems.Add("&Exit");
            myNotify.MenuItems[0].Click += new EventHandler(Restore_Click);
            myNotify.MenuItems[1].Click += new EventHandler(Exit_Click);
            MyNotifyIcon.ContextMenu = myNotify;

            MyNotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(MyNotifyIcon_MouseDoubleClick);
        }

        private void MyNotifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        private void Restore_Click(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region " ASSEMBLY ATTRIBUTE ACCESSORS "

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

        #region " FLAG CONTROLS "

        private void RedrawCheckbox()
        {
            SetFlags(BitField.Flag.f1, checkBox1);
            SetFlags(BitField.Flag.f2, checkBox2);
            SetFlags(BitField.Flag.f5, checkBox5);
            SetFlags(BitField.Flag.f6, checkBox6);
            SetFlags(BitField.Flag.f7, checkBox7);
            SetFlags(BitField.Flag.f8, checkBox8);
        }

        private void SetFlags(BitField.Flag flag, CheckBox cb)
        {
            if (bitField.AllOn(flag))
            {
                cb.IsChecked = true;
            }
            else
            {
                cb.IsChecked = false;
            }
        }

        private void Redraw()
        {
            myFlags = bitField.ToStringDec();
        }

        private void Toggle(BitField.Flag flag)
        {
            bitField.SetToggle(flag);
        }

        private void checkBox1_Click(object sender, RoutedEventArgs e)
        {
            if (bitField.AllOn(BitField.Flag.f1) != checkBox1.IsChecked)
            {
                Toggle(BitField.Flag.f1);
            }
            Redraw();
        }

        private void checkBox2_Click(object sender, RoutedEventArgs e)
        {
            if (bitField.AllOn(BitField.Flag.f2) != checkBox2.IsChecked)
            {
                Toggle(BitField.Flag.f2);
            }
            Redraw();
        }

        private void checkBox5_Click(object sender, RoutedEventArgs e)
        {
            if (bitField.AllOn(BitField.Flag.f5) != checkBox5.IsChecked)
            {
                Toggle(BitField.Flag.f5);
            }
            Redraw();
        }

        private void checkBox6_Click(object sender, RoutedEventArgs e)
        {
            if (bitField.AllOn(BitField.Flag.f6) != checkBox6.IsChecked)
            {
                Toggle(BitField.Flag.f6);
            }
            Redraw();
        }

        private void checkBox7_Click(object sender, RoutedEventArgs e)
        {
            if (bitField.AllOn(BitField.Flag.f7) != checkBox7.IsChecked)
            {
                Toggle(BitField.Flag.f7);
            }
            Redraw();
        }

        private void checkBox8_Click(object sender, RoutedEventArgs e)
        {
            if (bitField.AllOn(BitField.Flag.f8) != checkBox8.IsChecked)
            {
                Toggle(BitField.Flag.f8);
            }
            Redraw();
        }

        private void LoadOptions()
        {
            bitField.Mask = System.Convert.ToUInt64(myFlags, 10);
            bitField.Mask = bitField.Mask;
            RedrawCheckbox();
            Redraw();
        }

        #endregion

        #region " LAUNCH/HOOK PATH CONTROL "

        private void gui_UseHookPath_Click(object sender, RoutedEventArgs e)
        {
            if (gui_UseHookPath.IsChecked == true)
            {
                gui_LaunchPath.Text = gui_HookPath.Text;
            }
        }

        private void getHookPath_Click(object sender, RoutedEventArgs e)
        {
            if (ofd.ShowDialog() == true)
            {
                gui_HookPath.Text = ofd.FileName;
            }
        }

        private void getLaunchPath_Click(object sender, RoutedEventArgs e)
        {
            gui_UseHookPath.IsChecked = false;

            if (ofd.ShowDialog() == true)
            {
                gui_LaunchPath.Text = ofd.FileName;
            }
        }

        private void gui_Launch_Click(object sender, RoutedEventArgs e)
        {

            myWindow.WindowState = WindowState.Minimized;
            Globals.EndHook();
            SaveSettings();

            LoadINI();
            LoadOptions();

            SetupHook();
            Launch();
        }

        private void Launch()
        {
            proc = Process.GetProcessesByName(pName);
            if (!(proc.Length > 0))
            {
                System.Diagnostics.Process m = new System.Diagnostics.Process();
                m.StartInfo.FileName = gui_LaunchPath.Text;

                try
                {
                    m.Start();
                }
                catch
                {
                    MessageBox.Show("Launch path is invalid. Please double check settings.", "Warning!");
                }
            }
        }

        #endregion

        #region " WINDOW FUNCTIONS "

        public void GetBorders()
        {
            proc = Process.GetProcessesByName(MainWindow.pName);
            if (proc.Length > 0)
            {
                processById = Process.GetProcessById(proc[0].Id);
                int nIndex = -16;
                IntPtr mainWindowHandle = processById.MainWindowHandle;
                WindowStyles windowLong = (WindowStyles)Globals.GetWindowLong(mainWindowHandle, nIndex);
                Globals.GetWindowRect((int)mainWindowHandle, ref orig);

                gBorder = (orig.Right - orig.Left - TargetMap.maxx) / 2;
                gTop = (orig.Bottom - orig.Top) - TargetMap.maxy - gBorder;
                gHor = TargetMap.maxx;
                gVer = TargetMap.maxy;
            }
            else
            {
                MessageBox.Show("Game not in memory.", "Warning!");
            }
        }

        public void Fullscreen()
        {
            proc = Process.GetProcessesByName(MainWindow.pName);
            if (proc.Length > 0)
            {
                processById = Process.GetProcessById(proc[0].Id);
                int nIndex = -16;
                IntPtr mainWindowHandle = processById.MainWindowHandle;
                WindowStyles windowLong = (WindowStyles)Globals.GetWindowLong(mainWindowHandle, nIndex);
                //Globals.GetWindowRect((int)mainWindowHandle, ref full);
                WindowStyles styles = WindowStyles.WS_VISIBLE;
                Globals.SetWindowLongPtr(mainWindowHandle, nIndex, (IntPtr)styles);
                Globals.MoveWindow(mainWindowHandle, 0, 0, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height, true);

                if (quitTimer.IsEnabled == false)
                {
                    quitTimer.Start();
                }
            }
            else
            {
                MessageBox.Show("Game not in memory.", "Warning!");
            }
        }

        public void Restore()
        {
            proc = Process.GetProcessesByName(MainWindow.pName);
            if (proc.Length > 0)
            {
                processById = Process.GetProcessById(proc[0].Id);
                WindowStyles styles = WindowStyles.WS_CAPTION | WindowStyles.WS_SYSMENU | WindowStyles.WS_VISIBLE | WindowStyles.WS_MINIMIZEBOX | WindowStyles.WS_MAXIMIZEBOX | WindowStyles.WS_SIZEBOX | WindowStyles.WS_OVERLAPPED;
                int nIndex = -16;
                IntPtr mainWindowHandle = processById.MainWindowHandle;
                Globals.SetWindowLongPtr(mainWindowHandle, nIndex, (IntPtr)styles);
                int h = (gBorder * 2) + gHor;
                int v = gBorder + gVer + gTop;
                Globals.MoveWindow(mainWindowHandle, MainWindow.TargetMap.initx + 10, MainWindow.TargetMap.inity + 10, h, v, true);
            }
            else
            {
                MessageBox.Show("Game not in memory.", "Warning!");
            }
        }

        #endregion
    }

    #region " TARGET STRUCTURE "

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct TARGETMAP
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string path;
        public long dxversion;
        public long flags;
        public int initx;
        public int inity;
        public int minx;
        public int miny;
        public int maxx;
        public int maxy;
    }

    #endregion

    #region " GLOBAL FUNCTION IMPORTS "

    public class Globals
    {
        [DllImport("WinModXIV",
            SetLastError = true,
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        public static extern int SetTarget(
            ref TARGETMAP TARGET);

        [DllImport("WinModXIV",
            SetLastError = true,
            CallingConvention = CallingConvention.Cdecl)]
        public static extern int StartHook();

        [DllImport("WinModXIV",
            SetLastError = true,
            CallingConvention = CallingConvention.Cdecl)]
        public static extern int EndHook();

        [DllImport("WinModXIV",
            SetLastError = true,
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        public static extern IntPtr GetDllVersion();

        [DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileStringW",
            SetLastError = true,
            CharSet = CharSet.Unicode,
            ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern int GetPrivateProfileString(
          string lpAppName,
          string lpKeyName,
          string lpDefault,
          StringBuilder lpReturnedString,
          int nSize,
          string lpFilename);

        [DllImport("KERNEL32.DLL", EntryPoint = "WritePrivateProfileStringW",
            SetLastError = true,
            CharSet = CharSet.Unicode,
            ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern int WritePrivateProfileString(
          string lpAppName,
          string lpKeyName,
          string lpString,
          string lpFilename);

        [DllImport("user32", EntryPoint = "FindWindowA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int FindWindow([MarshalAs(UnmanagedType.VBByRefStr)] ref string lpClassName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpWindowName);

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetWindowRect(int hwnd, ref RECT lpRect);

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool MoveWindow(IntPtr hwnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

        public static int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            return 0;
        }

        public static int GetWindowLong(IntPtr hWnd, int nIndex)
        {
            return 0;
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8)
            {
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            }
            return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }

    #endregion

    #region " CONSTANTS "

    public class Constants
    {
        public static string DLLVersion = "";
    }

    #endregion

    #region " WINDOWSTYLES "

    public enum WindowStyles : long
    {
        WS_BORDER = 0x800000L,
        WS_CAPTION = 0xc00000L,
        WS_CHILD = 0x40000000L,
        WS_CHILDWINDOW = 0x40000000L,
        WS_CLIPCHILDREN = 0x2000000L,
        WS_CLIPSIBLINGS = 0x4000000L,
        WS_DISABLED = 0x8000000L,
        WS_DLGFRAME = 0x400000L,
        WS_EX_ACCEPTFILES = 0x10L,
        WS_EX_APPWINDOW = 0x40000L,
        WS_EX_CLIENTEDGE = 0x200L,
        WS_EX_COMPOSITED = 0x2000000L,
        WS_EX_CONTEXTHELP = 0x400L,
        WS_EX_CONTROLPARENT = 0x10000L,
        WS_EX_DLGMODALFRAME = 1L,
        WS_EX_LAYERED = 0x80000L,
        WS_EX_LAYOUTRTL = 0x400000L,
        WS_EX_LEFT = 0L,
        WS_EX_LEFTSCROLLBAR = 0x4000L,
        WS_EX_LTRREADING = 0L,
        WS_EX_MDICHILD = 0x40L,
        WS_EX_NOACTIVATE = 0x4000000L,
        WS_EX_NOINHERITLAYOUT = 0x100000L,
        WS_EX_NOPARENTNOTIFY = 4L,
        WS_EX_OVERLAPPEDWINDOW = 0x300L,
        WS_EX_PALETTEWINDOW = 0x188L,
        WS_EX_RIGHT = 0x1000L,
        WS_EX_RIGHTSCROLLBAR = 0L,
        WS_EX_RTLREADING = 0x2000L,
        WS_EX_STATICEDGE = 0x20000L,
        WS_EX_TOOLWINDOW = 0x80L,
        WS_EX_TOPMOST = 8L,
        WS_EX_TRANSPARENT = 0x20L,
        WS_EX_WINDOWEDGE = 0x100L,
        WS_GROUP = 0x20000L,
        WS_HSCROLL = 0x100000L,
        WS_ICONIC = 0x20000000L,
        WS_MAXIMIZE = 0x1000000L,
        WS_MAXIMIZEBOX = 0x10000L,
        WS_MINIMIZE = 0x20000000L,
        WS_MINIMIZEBOX = 0x20000L,
        WS_OVERLAPPED = 0L,
        WS_OVERLAPPEDWINDOW = 0xcf0000L,
        WS_POPUP = 0x80000000L,
        WS_POPUPWINDOW = 0x80880000L,
        WS_SIZEBOX = 0x40000L,
        WS_SYSMENU = 0x80000L,
        WS_TABSTOP = 0x10000L,
        WS_THICKFRAME = 0x40000L,
        WS_TILED = 0L,
        WS_TILEDWINDOW = 0xcf0000L,
        WS_VISIBLE = 0x10000000L,
        WS_VSCROLL = 0x200000L
    }

    #endregion
}
