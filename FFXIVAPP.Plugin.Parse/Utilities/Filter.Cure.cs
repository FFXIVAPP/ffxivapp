// FFXIVAPP.Plugin.Parse
// Filter.Cure.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Text.RegularExpressions;
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
        private static void ProcessCure(Event e, Expressions exp)
        {
            var line = new Line
            {
                RawLine = e.RawLine
            };
            var cure = Regex.Match("ph", @"^\.$");
            switch (e.Subject)
            {
                case EventSubject.You:
                    switch (e.Direction)
                    {
                        case EventDirection.You:
                        case EventDirection.Party:
                            cure = exp.pCure;
                            if (cure.Success)
                            {
                                line.Source = _lastNamePlayer;
                                UpdateHealingPlayer(cure, line, exp, false);
                            }
                            break;
                    }
                    break;
                case EventSubject.Party:
                    switch (e.Direction)
                    {
                        case EventDirection.You:
                        case EventDirection.Party:
                            cure = exp.pCure;
                            if (cure.Success)
                            {
                                line.Source = _lastNameParty;
                                UpdateHealingPlayer(cure, line, exp);
                            }
                            break;
                    }
                    break;
            }
            if (cure.Success)
            {
                return;
            }
            ClearLast();
            ParsingLogHelper.Log(LogManager.GetCurrentClassLogger(), "Cure", e, exp);
        }

        private static void UpdateHealingPlayer(Match cure, Line line, Expressions exp, bool isParty = true)
        {
            _isParty = isParty;
            try
            {
                line.Action = isParty ? _lastActionParty : _lastActionPlayer;
                line.Amount = cure.Groups["amount"].Success ? Convert.ToDecimal(cure.Groups["amount"].Value) : 0m;
                line.Crit = cure.Groups["crit"].Success;
                line.Modifier = cure.Groups["modifier"].Success ? Convert.ToDecimal(cure.Groups["modifier"].Value) / 100 : 0m;
                line.Target = Convert.ToString(cure.Groups["target"].Value);
                if (Regex.IsMatch(line.Target.ToLower(), exp.You))
                {
                    line.Target = String.IsNullOrWhiteSpace(Constants.CharacterName) ? "You" : Constants.CharacterName;
                }
                line.RecLossType = Convert.ToString(cure.Groups["type"].Value.ToUpper());
                if (isParty)
                {
                    _lastNameParty = line.Source;
                    if (line.IsEmpty() || (!_isMulti && _lastEventParty.Type != EventType.Actions && _lastEventParty.Type != EventType.Items))
                    {
                        ClearLast(true);
                        return;
                    }
                }
                else
                {
                    if (line.IsEmpty() || (!_isMulti && _lastEventPlayer.Type != EventType.Actions && _lastEventPlayer.Type != EventType.Items))
                    {
                        return;
                    }
                }
                ParseControl.Instance.Timeline.GetSetPlayer(line.Source)
                            .SetHealing(line);
            }
            catch (Exception ex)
            {
                ParsingLogHelper.Error(LogManager.GetCurrentClassLogger(), "Cure", exp.Event, ex);
            }
        }
    }
}
