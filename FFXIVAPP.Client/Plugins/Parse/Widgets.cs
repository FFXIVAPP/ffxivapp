// FFXIVAPP.Client
// Widgets.cs
// 
// © 2013 Ryan Wilson

using System;
using FFXIVAPP.Client.Plugins.Parse.WidgetWindows;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Plugins.Parse
{
    [DoNotObfuscate]
    public class Widgets
    {
        private static Widgets _instance;
        private DPSWidget _dpsWidget;

        public static Widgets Instance
        {
            get { return _instance ?? (_instance = new Widgets()); }
            set { _instance = value; }
        }

        public void ShowDPSWidget()
        {
            try
            {
                DPSWidget.Show();
            }
            catch (Exception ex)
            {
            }
        }

        private DPSWidget DPSWidget
        {
            get { return _dpsWidget ?? (_dpsWidget = new DPSWidget()); }
        }
    }
}
