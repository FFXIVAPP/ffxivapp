// FFXIVAPP.Common
// FlowDocHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using FFXIVAPP.Common.Converters;

#endregion

namespace FFXIVAPP.Common.Helpers
{
    public class FlowDocHelper
    {
        private readonly StringToBrushConverter _stb = new StringToBrushConverter();

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private static void BlockLoaded(object sender, RoutedEventArgs e)
        {
            var block = (Block) sender;
            block.BringIntoView();
            block.Loaded -= BlockLoaded;
        }

        /// <summary>
        /// </summary>
        /// <param name="time"> </param>
        /// <param name="playerName"></param>
        /// <param name="line"> </param>
        /// <param name="colors"> </param>
        /// <param name="flow"> </param>
        public void AppendFlow(string time, string playerName, string line, string[] colors, FlowDocumentReader flow)
        {
            Func<bool> funcAppend = delegate
            {
                DispatcherHelper.Invoke(delegate
                {
                    var timeStampColor = _stb.Convert(String.IsNullOrWhiteSpace(colors[0]) ? "#FFFFFFFF" : colors[0]);
                    var lineColor = _stb.Convert(String.IsNullOrWhiteSpace(colors[1]) ? "#FFFFFFFF" : colors[1]);
                    var paraGraph = new Paragraph();
                    var timeStamp = new Span(new Run(time))
                    {
                        Foreground = (Brush) timeStampColor,
                        FontWeight = FontWeights.Bold
                    };
                    var coloredLine = new Span(new Run(line))
                    {
                        Foreground = (Brush) lineColor
                    };
                    paraGraph.Inlines.Add(timeStamp);
                    if (!String.IsNullOrWhiteSpace(playerName))
                    {
                        var playerColor = _stb.Convert("#FFFF00FF");
                        var playerLine = new Span(new Run("[" + playerName + "] "))
                        {
                            Foreground = (Brush) playerColor
                        };
                        paraGraph.Inlines.Add(playerLine);
                    }
                    paraGraph.Inlines.Add(coloredLine);
                    flow.Document.Blocks.Add(paraGraph);
                    flow.Document.Blocks.LastBlock.Loaded += BlockLoaded;
                    if (flow.Document.Blocks.Count <= 500)
                    {
                        return;
                    }
                    flow.Document.Blocks.Clear();
                });
                return true;
            };
            funcAppend.BeginInvoke(null, null);
        }
    }
}
