// FFXIVAPP.Plugin.Parse
// Filter.Actions.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Enums;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Models.Parse;
using FFXIVAPP.Client.Models.Parse.Events;
using FFXIVAPP.Common.Helpers;
using NLog;

#endregion

namespace FFXIVAPP.Client.Utilities
{
    public static partial class Filter
    {
        private static void ProcessActions(Event e, Expressions exp)
        {
            var line = new Line
            {
                RawLine = e.RawLine
            };
            var actions = Regex.Match("ph", @"^\.$");
            switch (e.Subject)
            {
                case EventSubject.You:
                    switch (e.Direction)
                    {
                            // casts/uses
                        case EventDirection.Self:
                            actions = exp.pActions;
                            if (actions.Success)
                            {
                                line.Source = String.IsNullOrWhiteSpace(Constants.CharacterName) ? "You" : Constants.CharacterName;
                                if (actions.Groups["source"].Success)
                                {
                                    line.Source = ParseHelper.GetPetFromPlayer(Convert.ToString(actions.Groups["source"].Value), exp);
                                }
                                _lastNamePlayer = line.Source;
                                UpdateActionsPlayers(actions, line, exp, false);
                            }
                            break;
                    }
                    break;
                case EventSubject.Party:
                    switch (e.Direction)
                    {
                            // casts/uses
                        case EventDirection.Self:
                            actions = exp.pActions;
                            if (actions.Success)
                            {
                                line.Source = Convert.ToString(actions.Groups["source"].Value);
                                _lastNameParty = line.Source;
                                UpdateActionsPlayers(actions, line, exp);
                            }
                            break;
                    }
                    break;
                case EventSubject.Engaged:
                case EventSubject.UnEngaged:
                    switch (e.Direction)
                    {
                        case EventDirection.Self:
                            actions = exp.mActions;
                            if (actions.Success)
                            {
                                _lastMobName = StringHelper.TitleCase(Convert.ToString(actions.Groups["source"].Value));
                                _lastMobAction = StringHelper.TitleCase(Convert.ToString(actions.Groups["action"].Value));
                            }
                            break;
                    }
                    break;
            }
            if (actions.Success)
            {
                _isMulti = MultiTarget.IsMulti(StringHelper.TitleCase(Convert.ToString(actions.Groups["action"].Value)));
                return;
            }
            _isMulti = false;
            ClearLast(true);
            ParsingLogHelper.Log(LogManager.GetCurrentClassLogger(), "Action", e, exp);
        }

        private static void UpdateActionsPlayers(Match actions, Line line, Expressions exp, bool isParty = true)
        {
            _isParty = isParty;
            try
            {
                ParseControl.Instance.Timeline.GetSetPlayer(line.Source);
                if (isParty)
                {
                    _lastActionParty = StringHelper.TitleCase(Convert.ToString(actions.Groups["action"].Value));
                }
                else
                {
                    _lastActionPlayer = StringHelper.TitleCase(Convert.ToString(actions.Groups["action"].Value));
                }
            }
            catch (Exception ex)
            {
                ParsingLogHelper.Error(LogManager.GetCurrentClassLogger(), "Action", exp.Event, ex);
            }
        }
    }
}
