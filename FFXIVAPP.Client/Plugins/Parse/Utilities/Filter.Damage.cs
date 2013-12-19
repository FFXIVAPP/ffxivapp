// FFXIVAPP.Client
// Filter.Damage.cs
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
        private static void ProcessDamage(Event e, Expressions exp)
        {
            var line = new Line(e.ChatLogEntry)
            {
                EventDirection = e.Direction,
                EventSubject = e.Subject,
                EventType = e.Type
            };
            var damage = Regex.Match("ph", @"^\.$");
            switch (e.Subject)
            {
                case EventSubject.You:
                    switch (e.Direction)
                    {
                        case EventDirection.Engaged:
                        case EventDirection.UnEngaged:
                            damage = exp.pDamage;
                            switch (damage.Success)
                            {
                                case true:
                                    line.Source = You;
                                    UpdateDamageParty(damage, line, exp, FilterType.You);
                                    break;
                                case false:
                                    damage = exp.pDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionYouIsAttack = true;
                                        line.Source = You;
                                        UpdateDamageParty(damage, line, exp, FilterType.You);
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
                            damage = exp.pDamage;
                            switch (damage.Success)
                            {
                                case true:
                                    line.Source = _lastNamePet;
                                    line.Source = ParseHelper.GetPetFromPlayer(line.Source, exp, TimelineType.You);
                                    UpdateDamageParty(damage, line, exp, FilterType.Pet);
                                    break;
                                case false:
                                    damage = exp.pDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionPetIsAttack = true;
                                        line.Source = Convert.ToString(damage.Groups["source"].Value);
                                        line.Source = ParseHelper.GetPetFromPlayer(line.Source, exp, TimelineType.You);
                                        UpdateDamageParty(damage, line, exp, FilterType.Pet);
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
                            damage = exp.pDamage;
                            switch (damage.Success)
                            {
                                case true:
                                    line.Source = _lastNamePartyFrom;
                                    UpdateDamageParty(damage, line, exp, FilterType.Party);
                                    break;
                                case false:
                                    damage = exp.pDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionPartyIsAttack = true;
                                        line.Source = Convert.ToString(damage.Groups["source"].Value);
                                        UpdateDamageParty(damage, line, exp, FilterType.Party);
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
                            damage = exp.pDamage;
                            switch (damage.Success)
                            {
                                case true:
                                    line.Source = _lastNamePetPartyFrom;
                                    line.Source = ParseHelper.GetPetFromPlayer(line.Source, exp, TimelineType.Party);
                                    UpdateDamageParty(damage, line, exp, FilterType.PetParty);
                                    break;
                                case false:
                                    damage = exp.pDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionPetPartyIsAttack = true;
                                        line.Source = Convert.ToString(damage.Groups["source"].Value);
                                        line.Source = ParseHelper.GetPetFromPlayer(line.Source, exp, TimelineType.Party);
                                        UpdateDamageParty(damage, line, exp, FilterType.PetParty);
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
                            damage = exp.mDamage;
                            switch (damage.Success)
                            {
                                case true:
                                    line.Source = _lastNameMonster;
                                    line.Target = You;
                                    UpdateDamagePartyMonster(damage, line, exp, FilterType.You);
                                    break;
                                case false:
                                    damage = exp.mDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionYouIsAttack = true;
                                        line.Source = Convert.ToString(damage.Groups["source"].Value);
                                        line.Target = You;
                                        UpdateDamagePartyMonster(damage, line, exp, FilterType.You);
                                    }
                                    break;
                            }
                            break;
                        case EventDirection.Pet:
                            damage = exp.mDamage;
                            switch (damage.Success)
                            {
                                case true:
                                    line.Source = _lastNameMonster;
                                    line.Target = Convert.ToString(damage.Groups["target"].Value);
                                    line.Target = ParseHelper.GetPetFromPlayer(line.Target, exp, TimelineType.You);
                                    UpdateDamagePartyMonster(damage, line, exp, FilterType.Pet);
                                    break;
                                case false:
                                    damage = exp.mDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionPetIsAttack = true;
                                        line.Source = Convert.ToString(damage.Groups["source"].Value);
                                        line.Target = Convert.ToString(damage.Groups["target"].Value);
                                        line.Target = ParseHelper.GetPetFromPlayer(line.Target, exp, TimelineType.You);
                                        UpdateDamagePartyMonster(damage, line, exp, FilterType.Pet);
                                    }
                                    break;
                            }
                            break;
                        case EventDirection.Party:
                            damage = exp.mDamage;
                            switch (damage.Success)
                            {
                                case true:
                                    line.Source = _lastNameMonster;
                                    line.Target = Convert.ToString(damage.Groups["target"].Value);
                                    UpdateDamagePartyMonster(damage, line, exp, FilterType.Party);
                                    break;
                                case false:
                                    damage = exp.mDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionPartyIsAttack = true;
                                        line.Source = Convert.ToString(damage.Groups["source"].Value);
                                        line.Target = Convert.ToString(damage.Groups["target"].Value);
                                        UpdateDamagePartyMonster(damage, line, exp, FilterType.Party);
                                    }
                                    break;
                            }
                            break;
                        case EventDirection.PetParty:
                            damage = exp.mDamage;
                            switch (damage.Success)
                            {
                                case true:
                                    line.Source = _lastNameMonster;
                                    line.Target = Convert.ToString(damage.Groups["target"].Value);
                                    line.Target = ParseHelper.GetPetFromPlayer(line.Target, exp, TimelineType.Party);
                                    UpdateDamagePartyMonster(damage, line, exp, FilterType.PetParty);
                                    break;
                                case false:
                                    damage = exp.mDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionPetPartyIsAttack = true;
                                        line.Source = Convert.ToString(damage.Groups["source"].Value);
                                        line.Target = Convert.ToString(damage.Groups["target"].Value);
                                        line.Target = ParseHelper.GetPetFromPlayer(line.Target, exp, TimelineType.Party);
                                        UpdateDamagePartyMonster(damage, line, exp, FilterType.PetParty);
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
            }
            if (damage.Success)
            {
                return;
            }
            ParsingLogHelper.Log(LogManager.GetCurrentClassLogger(), "Damage", e, exp);
        }

        private static void UpdateDamageParty(Match damage, Line line, Expressions exp, FilterType type)
        {
            _type = type;
            try
            {
                line.Hit = true;
                switch (damage.Groups["source"].Success)
                {
                    case true:
                        line.Action = exp.Attack;
                        break;
                    case false:
                        var lastActionIsAttack = false;
                        switch (type)
                        {
                            case FilterType.You:
                                lastActionIsAttack = _lastActionYouIsAttack;
                                line.Action = _lastActionYou;
                                break;
                            case FilterType.Pet:
                                lastActionIsAttack = _lastActionPetIsAttack;
                                line.Action = _lastActionPet;
                                break;
                            case FilterType.Party:
                                lastActionIsAttack = _lastActionPartyIsAttack;
                                line.Action = _lastActionPartyFrom;
                                break;
                            case FilterType.PetParty:
                                lastActionIsAttack = _lastActionPetPartyIsAttack;
                                line.Action = _lastActionPetPartyFrom;
                                break;
                        }
                        line.Action = lastActionIsAttack ? String.Format("{0} [+]", exp.Attack) : line.Action;
                        break;
                }
                line.Amount = damage.Groups["amount"].Success ? Convert.ToDecimal(damage.Groups["amount"].Value) : 0m;
                line.Block = damage.Groups["block"].Success;
                line.Crit = damage.Groups["crit"].Success;
                line.Modifier = damage.Groups["modifier"].Success ? Convert.ToDecimal(damage.Groups["modifier"].Value) / 100 : 0m;
                line.Parry = damage.Groups["parry"].Success;
                line.Target = Convert.ToString(damage.Groups["target"].Value);
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
                ParsingLogHelper.Error(LogManager.GetCurrentClassLogger(), "Damage", exp.Event, ex);
            }
        }

        private static void UpdateDamagePartyMonster(Match damage, Line line, Expressions exp, FilterType type)
        {
            _type = type;
            try
            {
                line.Hit = true;
                switch (damage.Groups["source"].Success)
                {
                    case true:
                        var lastActionIsAttack = false;
                        switch (type)
                        {
                            case FilterType.You:
                                lastActionIsAttack = _lastActionYouIsAttack;
                                break;
                            case FilterType.Pet:
                                lastActionIsAttack = _lastActionPetIsAttack;
                                break;
                            case FilterType.Party:
                                lastActionIsAttack = _lastActionPartyIsAttack;
                                break;
                            case FilterType.PetParty:
                                lastActionIsAttack = _lastActionPetPartyIsAttack;
                                break;
                        }
                        line.Action = lastActionIsAttack ? String.Format("{0} [+]", exp.Attack) : exp.Attack;
                        break;
                    case false:
                        line.Action = _lastActionMonster;
                        break;
                }
                line.Amount = damage.Groups["amount"].Success ? Convert.ToDecimal(damage.Groups["amount"].Value) : 0m;
                line.Block = damage.Groups["block"].Success;
                line.Crit = damage.Groups["crit"].Success;
                line.Modifier = damage.Groups["modifier"].Success ? Convert.ToDecimal(damage.Groups["modifier"].Value) / 100 : 0m;
                line.Parry = damage.Groups["parry"].Success;
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
                ParsingLogHelper.Error(LogManager.GetCurrentClassLogger(), "Damage", exp.Event, ex);
            }
        }
    }
}
