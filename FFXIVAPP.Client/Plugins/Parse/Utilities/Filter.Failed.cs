// FFXIVAPP.Client
// Filter.Failed.cs
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
        private static void ProcessFailed(Event e, Expressions exp)
        {
            var line = new Line(e.ChatLogEntry)
            {
                EventDirection = e.Direction,
                EventSubject = e.Subject,
                EventType = e.Type
            };
            var failed = Regex.Match("ph", @"^\.$");
            switch (e.Subject)
            {
                case EventSubject.You:
                    switch (e.Direction)
                    {
                        case EventDirection.Engaged:
                        case EventDirection.UnEngaged:
                            failed = exp.pFailed;
                            switch (failed.Success)
                            {
                                case true:
                                    line.Source = You;
                                    UpdateFailedParty(failed, line, exp, FilterType.You);
                                    break;
                                case false:
                                    failed = exp.pFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = You;
                                        UpdateFailedParty(failed, line, exp, FilterType.You);
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case EventSubject.Pet:
                    switch (e.Direction)
                    {
                        case EventDirection.Engaged:
                        case EventDirection.UnEngaged:
                            failed = exp.pFailed;
                            switch (failed.Success)
                            {
                                case true:
                                    line.Source = _lastNamePet;
                                    line.Source = ParseHelper.GetPetFromPlayer(line.Source, exp, TimelineType.You);
                                    UpdateFailedParty(failed, line, exp, FilterType.Pet);
                                    break;
                                case false:
                                    failed = exp.pFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        line.Source = ParseHelper.GetPetFromPlayer(line.Source, exp, TimelineType.You);
                                        UpdateFailedParty(failed, line, exp, FilterType.Pet);
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case EventSubject.Party:
                    switch (e.Direction)
                    {
                        case EventDirection.Engaged:
                        case EventDirection.UnEngaged:
                            failed = exp.pFailed;
                            switch (failed.Success)
                            {
                                case true:
                                    line.Source = _lastNamePartyFrom;
                                    UpdateFailedParty(failed, line, exp, FilterType.Party);
                                    break;
                                case false:
                                    failed = exp.pFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        UpdateFailedParty(failed, line, exp, FilterType.Party);
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case EventSubject.PetParty:
                    switch (e.Direction)
                    {
                        case EventDirection.Engaged:
                        case EventDirection.UnEngaged:
                            failed = exp.pFailed;
                            switch (failed.Success)
                            {
                                case true:
                                    line.Source = _lastNamePetPartyFrom;
                                    line.Source = ParseHelper.GetPetFromPlayer(line.Source, exp, TimelineType.Party);
                                    UpdateFailedParty(failed, line, exp, FilterType.PetParty);
                                    break;
                                case false:
                                    failed = exp.pFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        line.Source = ParseHelper.GetPetFromPlayer(line.Source, exp, TimelineType.Party);
                                        UpdateFailedParty(failed, line, exp, FilterType.PetParty);
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case EventSubject.Engaged:
                case EventSubject.UnEngaged:
                    switch (e.Direction)
                    {
                        case EventDirection.You:
                            failed = exp.mFailed;
                            switch (failed.Success)
                            {
                                case true:
                                    line.Source = _lastNameMonster;
                                    line.Target = You;
                                    UpdateFailedPartyMonster(failed, line, exp, FilterType.You);
                                    break;
                                case false:
                                    failed = exp.mFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        line.Target = You;
                                        UpdateFailedPartyMonster(failed, line, exp, FilterType.You);
                                    }
                                    break;
                            }
                            break;
                        case EventDirection.Pet:
                            failed = exp.mFailed;
                            switch (failed.Success)
                            {
                                case true:
                                    line.Source = _lastNameMonster;
                                    line.Target = _lastNamePet;
                                    line.Target = ParseHelper.GetPetFromPlayer(line.Target, exp, TimelineType.You);
                                    UpdateFailedPartyMonster(failed, line, exp, FilterType.Pet);
                                    break;
                                case false:
                                    failed = exp.mFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        line.Target = Convert.ToString(failed.Groups["target"].Value);
                                        line.Target = ParseHelper.GetPetFromPlayer(line.Target, exp, TimelineType.You);
                                        UpdateFailedPartyMonster(failed, line, exp, FilterType.Pet);
                                    }
                                    break;
                            }
                            break;
                        case EventDirection.Party:
                            failed = exp.mFailed;
                            switch (failed.Success)
                            {
                                case true:
                                    line.Source = _lastNameMonster;
                                    line.Target = _lastNamePartyTo;
                                    UpdateFailedPartyMonster(failed, line, exp, FilterType.Party);
                                    break;
                                case false:
                                    failed = exp.mFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        line.Target = Convert.ToString(failed.Groups["target"].Value);
                                        UpdateFailedPartyMonster(failed, line, exp, FilterType.Party);
                                    }
                                    break;
                            }
                            break;
                        case EventDirection.PetParty:
                            failed = exp.mFailed;
                            switch (failed.Success)
                            {
                                case true:
                                    line.Source = _lastNameMonster;
                                    line.Target = _lastNamePetPartyTo;
                                    line.Target = ParseHelper.GetPetFromPlayer(line.Target, exp, TimelineType.Party);
                                    UpdateFailedPartyMonster(failed, line, exp, FilterType.PetParty);
                                    break;
                                case false:
                                    failed = exp.mFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        line.Target = Convert.ToString(failed.Groups["target"].Value);
                                        line.Target = ParseHelper.GetPetFromPlayer(line.Target, exp, TimelineType.Party);
                                        UpdateFailedPartyMonster(failed, line, exp, FilterType.PetParty);
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
            }
            if (failed.Success)
            {
                return;
            }
            ParsingLogHelper.Log(LogManager.GetCurrentClassLogger(), "Failed", e, exp);
        }

        private static void UpdateFailedParty(Match failed, Line line, Expressions exp, FilterType type)
        {
            _type = type;
            try
            {
                line.Miss = true;
                switch (failed.Groups["source"].Success)
                {
                    case true:
                        line.Action = exp.Attack;
                        break;
                    case false:
                        switch (type)
                        {
                            case FilterType.You:
                                line.Action = _lastActionYou;
                                break;
                            case FilterType.Pet:
                                line.Action = _lastActionPet;
                                break;
                            case FilterType.Party:
                                line.Action = _lastActionPartyFrom;
                                break;
                            case FilterType.PetParty:
                                line.Action = _lastActionPetPartyFrom;
                                break;
                        }
                        break;
                }
                switch (type)
                {
                    case FilterType.Pet:
                        _lastNamePet = line.Source;
                        break;
                    case FilterType.Party:
                        _lastNamePartyTo = line.Source;
                        break;
                    case FilterType.PetParty:
                        _lastNamePetPartyTo = line.Source;
                        break;
                }
                if (line.IsEmpty())
                {
                    return;
                }
                ParseControl.Instance.Timeline.PublishTimelineEvent(TimelineEventType.PartyMonsterFighting, line.Target);
                ParseControl.Instance.Timeline.GetSetMonster(line.Target, TimelineType.Party)
                            .SetDamageTaken(line);
                ParseControl.Instance.Timeline.GetSetPlayer(line.Source, TimelineType.Party)
                            .SetDamage(line);
            }
            catch (Exception ex)
            {
                ParsingLogHelper.Error(LogManager.GetCurrentClassLogger(), "Failed", exp.Event, ex);
            }
        }

        private static void UpdateFailedPartyMonster(Match failed, Line line, Expressions exp, FilterType type)
        {
            _type = type;
            try
            {
                line.Miss = true;
                switch (failed.Groups["source"].Success)
                {
                    case true:
                        line.Action = exp.Attack;
                        break;
                    case false:
                        line.Action = _lastActionMonster;
                        break;
                }
                switch (type)
                {
                    case FilterType.Pet:
                        _lastNamePet = line.Target;
                        break;
                    case FilterType.Party:
                        _lastNamePartyTo = line.Target;
                        break;
                    case FilterType.PetParty:
                        _lastNamePetPartyTo = line.Target;
                        break;
                }
                if (line.IsEmpty())
                {
                    return;
                }
                ParseControl.Instance.Timeline.PublishTimelineEvent(TimelineEventType.PartyMonsterFighting, line.Source);
                ParseControl.Instance.Timeline.GetSetPlayer(line.Target, TimelineType.Party)
                            .SetDamageTaken(line);
                ParseControl.Instance.Timeline.GetSetMonster(line.Source, TimelineType.Party)
                            .SetDamage(line);
            }
            catch (Exception ex)
            {
                ParsingLogHelper.Error(LogManager.GetCurrentClassLogger(), "Failed", exp.Event, ex);
            }
        }
    }
}
