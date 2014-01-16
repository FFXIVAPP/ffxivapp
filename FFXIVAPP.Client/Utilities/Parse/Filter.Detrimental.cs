// FFXIVAPP.Client
// Filter.Detrimental.cs
// 
// © 2013 Ryan Wilson

using System.Text.RegularExpressions;
using FFXIVAPP.Client.Helpers.Parse;
using FFXIVAPP.Client.Models.Parse;
using FFXIVAPP.Client.Models.Parse.Events;

namespace FFXIVAPP.Client.Utilities.Parse
{
    public static partial class Filter
    {
        private static void ProcessDetrimental(Event e, Expressions exp)
        {
            var line = new Line(e.ChatLogEntry)
            {
                EventDirection = e.Direction,
                EventSubject = e.Subject,
                EventType = e.Type
            };
            var detrimental = Regex.Match("ph", @"^\.$");
            if (detrimental.Success)
            {
                return;
            }
            ParsingLogHelper.Log(Logger, "Detrimental", e, exp);
        }
    }
}
