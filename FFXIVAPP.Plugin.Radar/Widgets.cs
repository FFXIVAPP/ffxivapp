// FFXIVAPP.Plugin.Radar
// Widgets.cs
// 
// © 2013 Ryan Wilson

using System;
using FFXIVAPP.Plugin.Radar.Windows;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Plugin.Radar
{
    [DoNotObfuscate]
    public class Widgets
    {
        private static Widgets _instance;
        private RadarWidget _radarWidget;

        public static Widgets Instance
        {
            get { return _instance ?? (_instance = new Widgets()); }
            set { _instance = value; }
        }

        public RadarWidget RadarWidget
        {
            get { return _radarWidget ?? (_radarWidget = new RadarWidget()); }
            set { _radarWidget = value; }
        }

        public void ShowRadarWidget()
        {
            try
            {
                RadarWidget.Show();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
