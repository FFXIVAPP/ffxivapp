// FFXIVAPP
// FlowDocumentHelper.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using FFXIVAPP.Classes.Converters;
using NLog;

namespace FFXIVAPP.Classes.Helpers
{
    internal class FlowDocumentHelper
    {
        private StringToBrushConverter _stb = new StringToBrushConverter();
        private Paragraph _pgraph;
        private Span _tStamp, _tMessage;

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void block_Loaded(object sender, RoutedEventArgs e)
        {
            var block = (Block) sender;
            block.BringIntoView();
            block.Loaded -= block_Loaded;
        }

        /// <summary>
        /// </summary>
        /// <param name="time"> </param>
        /// <param name="message"> </param>
        /// <param name="color"> </param>
        /// <param name="flow"> </param>
        public void AppendFlow(string time, string message, string color, FlowDocumentReader flow)
        {
            var tsc = _stb.Convert(Settings.Default.Color_TimeStamp.ToString());
            var c = _stb.Convert(color);
            _pgraph = new Paragraph();
            _tStamp = new Span(new Run(time)) {Foreground = (Brush) tsc, FontWeight = FontWeights.Bold};
            _tMessage = new Span(new Run(message)) {Foreground = (Brush) c};
            _pgraph.Inlines.Add(_tStamp);
            _pgraph.Inlines.Add(_tMessage);
            flow.Document.Blocks.Add(_pgraph);
            flow.Document.Blocks.LastBlock.Loaded += block_Loaded;
            _tMessage = null;
            _tStamp = null;
            _pgraph = null;
        }
    }
}