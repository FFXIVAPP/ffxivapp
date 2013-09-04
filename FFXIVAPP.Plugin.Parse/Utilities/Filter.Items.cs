// FFXIVAPP.Plugin.Parse
// Filter.Items.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Text.RegularExpressions;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Plugin.Parse.Enums;
using FFXIVAPP.Plugin.Parse.Helpers;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Models.Events;
using NLog;

#endregion

namespace FFXIVAPP.Plugin.Parse.Utilities
{
    public static partial class Filter
    {
        private static void ProcessItems(Event e, Expressions exp)
        {
            var line = new Line
            {
                RawLine = e.RawLine
            };
            var items = Regex.Match("ph", @"^\.$");
            switch (e.Subject)
            {
                case EventSubject.You:
                    switch (e.Direction)
                    {
                        case EventDirection.Self:
                            _isParty = false;
                            items = exp.pItems;
                            if (items.Success)
                            {
                                line.Source = String.IsNullOrWhiteSpace(Constants.CharacterName) ? "You" : Constants.CharacterName;
                                _lastNamePlayer = line.Source;
                                _lastActionPlayer = StringHelper.TitleCase(Convert.ToString(items.Groups["item"].Value));
                            }
                            break;
                    }
                    break;
                case EventSubject.Party:
                    switch (e.Direction)
                    {
                        case EventDirection.Self:
                            items = exp.pItems;
                            if (items.Success)
                            {
                                line.Source = Convert.ToString(items.Groups["source"].Value);
                                _lastNameParty = line.Source;
                                _lastActionParty = StringHelper.TitleCase(Convert.ToString(items.Groups["item"].Value));
                            }
                            break;
                    }
                    break;
                case EventSubject.Engaged:
                case EventSubject.UnEngaged:
                    break;
            }
            if (items.Success)
            {
                return;
            }
            ClearLast();
            ParsingLogHelper.Log(LogManager.GetCurrentClassLogger(), "Item", e, exp);
        }
    }
}
