// FFXIVAPP.Client
// Filter.Actions.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Plugins.Parse.Enums;
using FFXIVAPP.Client.Plugins.Parse.Helpers;
using FFXIVAPP.Client.Plugins.Parse.Models;
using FFXIVAPP.Client.Plugins.Parse.Models.Events;
using FFXIVAPP.Common.Helpers;
using NLog;

namespace FFXIVAPP.Client.Plugins.Parse.Utilities
{
    public static partial class Filter
    {
        private static void ProcessActions(Event e, Expressions exp)
        {
            var line = new Line(e.ChatLogEntry)
            {
                EventDirection = e.Direction,
                EventSubject = e.Subject,
                EventType = e.Type
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
                                line.Source = You;
                                UpdateActionsParty(actions, line, exp, FilterType.You);
                            }
                            break;
                    }
                    break;
                case EventSubject.Pet:
                    switch (e.Direction)
                    {
                            // casts/uses
                        case EventDirection.Self:
                            actions = exp.pActions;
                            if (actions.Success)
                            {
                                line.Source = Convert.ToString(actions.Groups["source"].Value);
                                _lastNamePet = line.Source;
                                line.Source = ParseHelper.GetPetFromPlayer(line.Source, exp, TimelineType.You);
                                UpdateActionsParty(actions, line, exp, FilterType.Pet);
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
                                _lastNamePartyFrom = line.Source;
                                UpdateActionsParty(actions, line, exp, FilterType.Party);
                            }
                            break;
                    }
                    break;
                case EventSubject.PetParty:
                    switch (e.Direction)
                    {
                            // casts/uses
                        case EventDirection.Self:
                            actions = exp.pActions;
                            if (actions.Success)
                            {
                                line.Source = Convert.ToString(actions.Groups["source"].Value);
                                line.Source = ParseHelper.GetPetFromPlayer(line.Source, exp, TimelineType.Party);
                                UpdateActionsParty(actions, line, exp, FilterType.PetParty);
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
                                _lastNameMonster = StringHelper.TitleCase(Convert.ToString(actions.Groups["source"].Value));
                                _lastActionMonster = StringHelper.TitleCase(Convert.ToString(actions.Groups["action"].Value));
                                UpdateActionsPartyMonster(actions, line, exp, FilterType.MonsterParty);
                            }
                            break;
                    }
                    break;
            }
            if (actions.Success)
            {
                return;
            }
            ParsingLogHelper.Log(LogManager.GetCurrentClassLogger(), "Action", e, exp);
        }

        private static void UpdateActionsParty(Match actions, Line line, Expressions exp, FilterType type)
        {
            _type = type;
            _lastActionYouIsAttack = false;
            _lastActionPetIsAttack = false;
            _lastActionPartyIsAttack = false;
            _lastActionPetPartyIsAttack = false;
            _lastActionAllianceIsAttack = false;
            _lastActionPetAllianceIsAttack = false;
            try
            {
                var isHealingSkill = false;
                ParseControl.Instance.Timeline.GetSetPlayer(line.Source, TimelineType.Party);
                var action = StringHelper.TitleCase(Convert.ToString(actions.Groups["action"].Value));
                foreach (var healingAction in ParseHelper.HealingActions.Where(healingAction => String.Equals(healingAction, action, Constants.InvariantComparer)))
                {
                    isHealingSkill = true;
                }
                switch (type)
                {
                    case FilterType.You:
                        _lastActionYou = action;
                        break;
                    case FilterType.Pet:
                        _lastActionPet = action;
                        break;
                    case FilterType.Party:
                        if (isHealingSkill)
                        {
                            _lastActionPartyHealingFrom = action;
                            _lastNamePartyHealingFrom = line.Source;
                        }
                        else
                        {
                            _lastActionPartyFrom = action;
                            _lastNamePartyFrom = line.Source;
                        }
                        break;
                    case FilterType.PetParty:
                        if (isHealingSkill)
                        {
                            _lastActionPetPartyHealingFrom = action;
                            _lastNamePetPartyHealingFrom = line.Source;
                        }
                        else
                        {
                            _lastActionPetPartyFrom = action;
                            _lastNamePetPartyFrom = line.Source;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ParsingLogHelper.Error(LogManager.GetCurrentClassLogger(), "Action", exp.Event, ex);
            }
        }

        private static void UpdateActionsPartyMonster(Match actions, Line line, Expressions exp, FilterType type)
        {
            _type = type;
            _lastActionYouIsAttack = false;
            _lastActionPetIsAttack = false;
            _lastActionPartyIsAttack = false;
            _lastActionPetPartyIsAttack = false;
            _lastActionAllianceIsAttack = false;
            _lastActionPetAllianceIsAttack = false;
            try
            {
            }
            catch (Exception ex)
            {
                ParsingLogHelper.Error(LogManager.GetCurrentClassLogger(), "Action", exp.Event, ex);
            }
        }
    }
}
