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
            LineHelper.SetTimelineTypes(ref line);
            if (LineHelper.IsIgnored(line))
            {
                return;
            }
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
                                    UpdateFailed(failed, line, exp, FilterType.You);
                                    break;
                                case false:
                                    failed = exp.pFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = You;
                                        UpdateFailed(failed, line, exp, FilterType.You);
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
                                    UpdateFailed(failed, line, exp, FilterType.Pet);
                                    break;
                                case false:
                                    failed = exp.pFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        UpdateFailed(failed, line, exp, FilterType.Pet);
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
                                    UpdateFailed(failed, line, exp, FilterType.Party);
                                    break;
                                case false:
                                    failed = exp.pFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        UpdateFailed(failed, line, exp, FilterType.Party);
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
                                    UpdateFailed(failed, line, exp, FilterType.PetParty);
                                    break;
                                case false:
                                    failed = exp.pFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        UpdateFailed(failed, line, exp, FilterType.PetParty);
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case EventSubject.Alliance:
                    switch (e.Direction)
                    {
                        case EventDirection.Engaged:
                        case EventDirection.UnEngaged:
                            failed = exp.pFailed;
                            switch (failed.Success)
                            {
                                case true:
                                    line.Source = _lastNameAllianceFrom;
                                    UpdateFailed(failed, line, exp, FilterType.Alliance);
                                    break;
                                case false:
                                    failed = exp.pFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        UpdateFailed(failed, line, exp, FilterType.Alliance);
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case EventSubject.PetAlliance:
                    switch (e.Direction)
                    {
                        case EventDirection.Engaged:
                        case EventDirection.UnEngaged:
                            failed = exp.pFailed;
                            switch (failed.Success)
                            {
                                case true:
                                    line.Source = _lastNamePetAllianceFrom;
                                    UpdateFailed(failed, line, exp, FilterType.PetAlliance);
                                    break;
                                case false:
                                    failed = exp.pFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        UpdateFailed(failed, line, exp, FilterType.PetAlliance);
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case EventSubject.Other:
                    switch (e.Direction)
                    {
                        case EventDirection.Engaged:
                        case EventDirection.UnEngaged:
                            failed = exp.pFailed;
                            switch (failed.Success)
                            {
                                case true:
                                    line.Source = _lastNameOtherFrom;
                                    UpdateFailed(failed, line, exp, FilterType.Other);
                                    break;
                                case false:
                                    failed = exp.pFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        UpdateFailed(failed, line, exp, FilterType.Other);
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case EventSubject.PetOther:
                    switch (e.Direction)
                    {
                        case EventDirection.Engaged:
                        case EventDirection.UnEngaged:
                            failed = exp.pFailed;
                            switch (failed.Success)
                            {
                                case true:
                                    line.Source = _lastNamePetOtherFrom;
                                    UpdateFailed(failed, line, exp, FilterType.PetOther);
                                    break;
                                case false:
                                    failed = exp.pFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        UpdateFailed(failed, line, exp, FilterType.PetOther);
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
                                    UpdateFailedMonster(failed, line, exp, FilterType.You);
                                    break;
                                case false:
                                    failed = exp.mFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        line.Target = You;
                                        UpdateFailedMonster(failed, line, exp, FilterType.You);
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
                                    UpdateFailedMonster(failed, line, exp, FilterType.Pet);
                                    break;
                                case false:
                                    failed = exp.mFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        line.Target = Convert.ToString(failed.Groups["target"].Value);
                                        UpdateFailedMonster(failed, line, exp, FilterType.Pet);
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
                                    UpdateFailedMonster(failed, line, exp, FilterType.Party);
                                    break;
                                case false:
                                    failed = exp.mFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        line.Target = Convert.ToString(failed.Groups["target"].Value);
                                        UpdateFailedMonster(failed, line, exp, FilterType.Party);
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
                                    UpdateFailedMonster(failed, line, exp, FilterType.PetParty);
                                    break;
                                case false:
                                    failed = exp.mFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        line.Target = Convert.ToString(failed.Groups["target"].Value);
                                        UpdateFailedMonster(failed, line, exp, FilterType.PetParty);
                                    }
                                    break;
                            }
                            break;
                        case EventDirection.Alliance:
                            failed = exp.mFailed;
                            switch (failed.Success)
                            {
                                case true:
                                    line.Source = _lastNameMonster;
                                    line.Target = _lastNameAllianceTo;
                                    UpdateFailedMonster(failed, line, exp, FilterType.Alliance);
                                    break;
                                case false:
                                    failed = exp.mFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        line.Target = Convert.ToString(failed.Groups["target"].Value);
                                        UpdateFailedMonster(failed, line, exp, FilterType.Alliance);
                                    }
                                    break;
                            }
                            break;
                        case EventDirection.PetAlliance:
                            failed = exp.mFailed;
                            switch (failed.Success)
                            {
                                case true:
                                    line.Source = _lastNameMonster;
                                    line.Target = _lastNamePetAllianceTo;
                                    UpdateFailedMonster(failed, line, exp, FilterType.PetAlliance);
                                    break;
                                case false:
                                    failed = exp.mFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        line.Target = Convert.ToString(failed.Groups["target"].Value);
                                        UpdateFailedMonster(failed, line, exp, FilterType.PetAlliance);
                                    }
                                    break;
                            }
                            break;
                        case EventDirection.Other:
                            failed = exp.mFailed;
                            switch (failed.Success)
                            {
                                case true:
                                    line.Source = _lastNameMonster;
                                    line.Target = _lastNameOtherTo;
                                    UpdateFailedMonster(failed, line, exp, FilterType.Other);
                                    break;
                                case false:
                                    failed = exp.mFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        line.Target = Convert.ToString(failed.Groups["target"].Value);
                                        UpdateFailedMonster(failed, line, exp, FilterType.Other);
                                    }
                                    break;
                            }
                            break;
                        case EventDirection.PetOther:
                            failed = exp.mFailed;
                            switch (failed.Success)
                            {
                                case true:
                                    line.Source = _lastNameMonster;
                                    line.Target = _lastNamePetOtherTo;
                                    UpdateFailedMonster(failed, line, exp, FilterType.PetOther);
                                    break;
                                case false:
                                    failed = exp.mFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        line.Target = Convert.ToString(failed.Groups["target"].Value);
                                        UpdateFailedMonster(failed, line, exp, FilterType.PetOther);
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

        private static void UpdateFailed(Match failed, Line line, Expressions exp, FilterType type)
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
                            case FilterType.Alliance:
                                line.Action = _lastActionAllianceFrom;
                                break;
                            case FilterType.PetAlliance:
                                line.Action = _lastActionPetAllianceFrom;
                                break;
                            case FilterType.Other:
                                line.Action = _lastActionOtherFrom;
                                break;
                            case FilterType.PetOther:
                                line.Action = _lastActionPetOtherFrom;
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
                    case FilterType.Alliance:
                        _lastNameAllianceTo = line.Source;
                        break;
                    case FilterType.PetAlliance:
                        _lastNamePetAllianceTo = line.Source;
                        break;
                    case FilterType.Other:
                        _lastNameOtherTo = line.Source;
                        break;
                    case FilterType.PetOther:
                        _lastNamePetOtherTo = line.Source;
                        break;
                }
                if (line.IsEmpty())
                {
                    return;
                }
                switch (type)
                {
                    default:
                        ParseControl.Instance.Timeline.PublishTimelineEvent(TimelineEventType.PartyMonsterFighting, line.Target);
                        break;
                }
                ParseControl.Instance.Timeline.GetSetMonster(line.Target)
                            .SetDamageTaken(line);
                ParseControl.Instance.Timeline.GetSetPlayer(line.Source)
                            .SetDamage(line);
            }
            catch (Exception ex)
            {
                ParsingLogHelper.Error(LogManager.GetCurrentClassLogger(), "Failed", exp.Event, ex);
            }
        }

        private static void UpdateFailedMonster(Match failed, Line line, Expressions exp, FilterType type)
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
                    case FilterType.Alliance:
                        _lastNameAllianceTo = line.Target;
                        break;
                    case FilterType.PetAlliance:
                        _lastNamePetAllianceTo = line.Target;
                        break;
                    case FilterType.Other:
                        _lastNameOtherTo = line.Target;
                        break;
                    case FilterType.PetOther:
                        _lastNamePetOtherTo = line.Target;
                        break;
                }
                if (line.IsEmpty())
                {
                    return;
                }
                switch (type)
                {
                    default:
                        ParseControl.Instance.Timeline.PublishTimelineEvent(TimelineEventType.PartyMonsterFighting, line.Source);
                        break;
                }
                ParseControl.Instance.Timeline.GetSetPlayer(line.Target)
                            .SetDamageTaken(line);
                ParseControl.Instance.Timeline.GetSetMonster(line.Source)
                            .SetDamage(line);
            }
            catch (Exception ex)
            {
                ParsingLogHelper.Error(LogManager.GetCurrentClassLogger(), "Failed", exp.Event, ex);
            }
        }
    }
}
