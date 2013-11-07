// FFXIVAPP.Client
// Filter.Detrimental.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Plugins.Parse.Enums;
using FFXIVAPP.Client.Plugins.Parse.Helpers;
using FFXIVAPP.Client.Plugins.Parse.Models;
using FFXIVAPP.Client.Plugins.Parse.Models.StatGroups;
using NLog;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Utilities
{
    public static partial class Filter
    {
        private static void ProcessDetrimental(Models.Events.Event e, Expressions exp)
        {
            var line = new Line
            {
                RawLine = e.RawLine
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
            ClearLast();
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
