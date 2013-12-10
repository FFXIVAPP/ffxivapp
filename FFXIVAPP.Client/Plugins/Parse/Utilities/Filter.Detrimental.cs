// FFXIVAPP.Client
// Filter.Detrimental.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Plugins.Parse.Enums;
using FFXIVAPP.Client.Plugins.Parse.Helpers;
using FFXIVAPP.Client.Plugins.Parse.Models;
using FFXIVAPP.Client.Plugins.Parse.Models.Events;
using NLog;

namespace FFXIVAPP.Client.Plugins.Parse.Utilities
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
            switch (e.Subject)
            {
                case EventSubject.You:
                case EventSubject.Party:
                case EventSubject.Other:
                case EventSubject.NPC:
                case EventSubject.Engaged:
                case EventSubject.UnEngaged:
                    break;
            }
            if (detrimental.Success)
            {
                return;
            }
            ParsingLogHelper.Log(LogManager.GetCurrentClassLogger(), "Detrimental", e, exp);
        }

        private static void UpdateDetrimentalPlayer(Match detrimental, Line line, Expressions exp, bool isParty = true)
        {
            _isParty = isParty;
            try
            {
            }
            catch (Exception ex)
            {
                ParsingLogHelper.Error(LogManager.GetCurrentClassLogger(), "Detrimental", exp.Event, ex);
            }
        }

        private static void UpdateDetrimentalMonster(Match detrimental, Line line, Expressions exp, bool isParty = true)
        {
            _isParty = isParty;
            try
            {
            }
            catch (Exception ex)
            {
                ParsingLogHelper.Error(LogManager.GetCurrentClassLogger(), "Detrimental", exp.Event, ex);
            }
        }
    }
}
