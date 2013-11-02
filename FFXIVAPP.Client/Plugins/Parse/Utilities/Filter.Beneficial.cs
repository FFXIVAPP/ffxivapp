// FFXIVAPP.Client
// Filter.Beneficial.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Text.RegularExpressions;
using FFXIVAPP.Client.Plugins.Parse.Enums;
using FFXIVAPP.Client.Plugins.Parse.Helpers;
using FFXIVAPP.Client.Plugins.Parse.Models;
using NLog;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Utilities
{
    public static partial class Filter
    {
        private static void ProcessBeneficial(Models.Events.Event e, Expressions exp)
        {
            var line = new Line
            {
                RawLine = e.RawLine
            };
            var beneficial = Regex.Match("ph", @"^\.$");
            switch (e.Subject)
            {
                case EventSubject.You:
                case EventSubject.Party:
                    //switch (e.Direction)
                    //{
                    //    case EventDirection.Self:
                    //    case EventDirection.You:
                    //    case EventDirection.Party:
                    //        beneficial = exp.pBeneficialGain;
                    //        if (beneficial.Success)
                    //        {
                    //            line.Source = Convert.ToString(beneficial.Groups["source"].Value);
                    //            if (e.Subject == EventSubject.You)
                    //            {
                    //                line.Source = String.IsNullOrWhiteSpace(Constants.CharacterName) ? "You" : Constants.CharacterName;
                    //            }
                    //            _lastNameParty = line.Source;
                    //        }
                    //        beneficial = exp.pBeneficialLose;
                    //        if (beneficial.Success)
                    //        {
                    //            line.Source = Convert.ToString(beneficial.Groups["source"].Value);
                    //            if (e.Subject == EventSubject.You)
                    //            {
                    //                line.Source = String.IsNullOrWhiteSpace(Constants.CharacterName) ? "You" : Constants.CharacterName;
                    //            }
                    //            _lastNameParty = line.Source;
                    //        }
                    //        break;
                    //}
                    //break;
                case EventSubject.Other:
                case EventSubject.NPC:
                    break;
                case EventSubject.Engaged:
                case EventSubject.UnEngaged:
                    //switch (e.Direction)
                    //{
                    //    case EventDirection.Engaged:
                    //    case EventDirection.UnEngaged:
                    //        beneficial = exp.mBeneficialGain;
                    //        if (beneficial.Success)
                    //        {
                    //            line.Source = Convert.ToString(beneficial.Groups["source"].Value);
                    //            _lastMobName = line.Source;
                    //        }
                    //        beneficial = exp.mBeneficialLose;
                    //        if (beneficial.Success)
                    //        {
                    //            line.Source = Convert.ToString(beneficial.Groups["source"].Value);
                    //            _lastMobName = line.Source;
                    //        }
                    //        break;
                    //}
                    break;
            }
            if (beneficial.Success)
            {
                return;
            }
            ClearLast();
            ParsingLogHelper.Log(LogManager.GetCurrentClassLogger(), "Beneficial", e, exp);
        }
    }
}
