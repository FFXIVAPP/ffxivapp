// FFXIVAPP.Plugin.Radar
// WidgetTopMostHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Plugin.Radar.Interop;
using FFXIVAPP.Plugin.Radar.Properties;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Plugin.Radar.Helpers
{
    [DoNotObfuscate]
    public static class WidgetTopMostHelper
    {
        private static WinAPI.WinEventDelegate _delegate;
        private static IntPtr _mainHandleHook;

        #region Helpers

        private static WindowInteropHelper _radarWidgetInteropHelper;

        private static WindowInteropHelper RadarWidgetInteropHelper
        {
            get { return _radarWidgetInteropHelper ?? (_radarWidgetInteropHelper = new WindowInteropHelper(Widgets.Instance.RadarWidget)); }
        }

        #endregion

        private static Timer SetWindowTimer { get; set; }

        public static void HookWidgetTopMost()
        {
            try
            {
                _delegate = BringWidgetsIntoFocus;
                _mainHandleHook = WinAPI.SetWinEventHook(WinAPI.EVENT_SYSTEM_FOREGROUND, WinAPI.EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, _delegate, 0, 0, WinAPI.WINEVENT_OUTOFCONTEXT);
            }
            catch (Exception e)
            {
            }
            SetWindowTimer = new Timer(1000);
            SetWindowTimer.Elapsed += SetWindowTimerOnElapsed;
            SetWindowTimer.Start();
        }

        private static void SetWindowTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            DispatcherHelper.Invoke(() => BringWidgetsIntoFocus(), DispatcherPriority.Normal);
        }

        private static void BringWidgetsIntoFocus(IntPtr hwineventhook, uint eventtype, IntPtr hwnd, int idobject, int idchild, uint dweventthread, uint dwmseventtime)
        {
            BringWidgetsIntoFocus(true);
        }

        private static void BringWidgetsIntoFocus(bool force = false)
        {
            try
            {
                var handle = WinAPI.GetForegroundWindow();
                var activeTitle = WinAPI.GetActiveWindowTitle();

                var stayOnTop = Application.Current.Windows.OfType<Window>()
                                           .Any(w => w.Title == activeTitle) || Regex.IsMatch(activeTitle.ToUpper(), @"^(FINAL FANTASY XIV)", SharedRegEx.DefaultOptions);

                // If any of the windows are focused, don't try to hide any of them, or it'll prevent us from moving/closing them
                if (handle == RadarWidgetInteropHelper.Handle)
                {
                    return;
                }
                if (Settings.Default.ShowRadarWidgetOnLoad)
                {
                    ToggleTopMost(Widgets.Instance.RadarWidget, stayOnTop, force);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="window"></param>
        /// <param name="stayOnTop"></param>
        /// <param name="force"></param>
        private static void ToggleTopMost(Window window, bool stayOnTop, bool force)
        {
            if (window.Topmost && stayOnTop && !force)
            {
                return;
            }
            window.Topmost = false;
            if (!stayOnTop)
            {
                if (window.IsVisible)
                {
                    window.Hide();
                }
                return;
            }
            window.Topmost = true;
            if (!window.IsVisible)
            {
                window.Show();
            }
        }
    }
}
