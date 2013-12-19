// FFXIVAPP.Client
// Filter.Beneficial.cs
// 
// © 2013 Ryan Wilson

using System.Text.RegularExpressions;
using FFXIVAPP.Client.Plugins.Parse.Helpers;
using FFXIVAPP.Client.Plugins.Parse.Models;
using FFXIVAPP.Client.Plugins.Parse.Models.Events;
using NLog;

namespace FFXIVAPP.Client.Plugins.Parse.Utilities
{
    public static partial class Filter
    {
        private static void ProcessBeneficial(Event e, Expressions exp)
        {
            var line = new Line(e.ChatLogEntry)
            {
                EventDirection = e.Direction,
                EventSubject = e.Subject,
                EventType = e.Type
            };
            var beneficial = Regex.Match("ph", @"^\.$");
            if (beneficial.Success)
            {
                return;
            }
            ParsingLogHelper.Log(LogManager.GetCurrentClassLogger(), "Beneficial", e, exp);
        }
    }
}
