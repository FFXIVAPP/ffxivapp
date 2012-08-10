// FFXIVAPP
// AboutV.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace FFXIVAPP.Views
{
    /// <summary>
    ///   Interaction logic for About.xaml
    /// </summary>
    public partial class AboutV
    {
        private static string _version = "";
        private static string _buildDate = "";
        public static AboutV View;

        public AboutV()
        {
            InitializeComponent();
            // Insert code required on object creation below this point.
            View = this;

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            _version = version.ToString();
            _buildDate = new DateTime((version.Build - 1)*TimeSpan.TicksPerDay + version.Revision*TimeSpan.TicksPerSecond*2).AddYears(1999).ToString(CultureInfo.InvariantCulture);

            BuildChanges();
        }

        /// <summary>
        /// </summary>
        private void BuildChanges()
        {
            AddP(Changes, "Current Version:", true);
            AddP(Changes, _version + (String.Format(" (Built : {0})", _buildDate)));
            AddP(Changes, "Changes:", true);
            var l = new[] {"Dual-Box fixed."};
            foreach (var i in l)
            {
                AddP(Changes, String.Format("• {0}", i));
            }
            AddP(Changes, "Working On/Need Help With:", true);
            l = new[] {"Line Examples (All Languages): partial counter, block, parry, resist, evade, part(body), multi-hit start lines and attack values after that.", "A lot is already started but line examples would be great as I'm busy in real life and can't play as much anymore.", "German support is still reliant on getting a full line work-up of all variations and grammar pointed out."};
            foreach (var i in l)
            {
                AddP(Changes, String.Format("• {0}", i));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="flow"> </param>
        /// <param name="message"> </param>
        /// <param name="bold"> </param>
        private static void AddP(FlowDocumentScrollViewer flow, string message, bool bold = false)
        {
            var pgraph = new Paragraph(new Span(new Run(message)));
            pgraph.Foreground = Brushes.Black;
            pgraph.FontWeight = bold ? FontWeights.Bold : FontWeights.Normal;
            pgraph.FontSize = bold ? 14 : 12;
            flow.Document.Blocks.Add(pgraph);
        }
    }
}