// FFXIVAPP.Client
// Filter.Damage.cs
// 
// Copyright © 2007 - 2014 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Enums.Parse;
using FFXIVAPP.Client.Helpers.Parse;
using FFXIVAPP.Client.Models.Parse;
using FFXIVAPP.Client.Models.Parse.Events;

namespace FFXIVAPP.Client.Utilities.Parse
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
            LineHelper.SetTimelineTypes(ref line);
            if (LineHelper.IsIgnored(line))
            {
                return;
            }
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
                                    UpdateDamage(damage, line, exp, FilterType.You);
                                    break;
                                case false:
                                    damage = exp.pDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionYouIsAttack = true;
                                        line.Source = You;
                                        UpdateDamage(damage, line, exp, FilterType.You);
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
                                    UpdateDamage(damage, line, exp, FilterType.Pet);
                                    break;
                                case false:
                                    damage = exp.pDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionPetIsAttack = true;
                                        UpdateDamage(damage, line, exp, FilterType.Pet);
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
                                    UpdateDamage(damage, line, exp, FilterType.Party);
                                    break;
                                case false:
                                    damage = exp.pDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionPartyIsAttack = true;
                                        UpdateDamage(damage, line, exp, FilterType.Party);
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
                                    UpdateDamage(damage, line, exp, FilterType.PetParty);
                                    break;
                                case false:
                                    damage = exp.pDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionPetPartyIsAttack = true;
                                        UpdateDamage(damage, line, exp, FilterType.PetParty);
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
                            damage = exp.pDamage;
                            switch (damage.Success)
                            {
                                case true:
                                    line.Source = _lastNameAllianceFrom;
                                    UpdateDamage(damage, line, exp, FilterType.Alliance);
                                    break;
                                case false:
                                    damage = exp.pDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionAllianceIsAttack = true;
                                        UpdateDamage(damage, line, exp, FilterType.Alliance);
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
                            damage = exp.pDamage;
                            switch (damage.Success)
                            {
                                case true:
                                    line.Source = _lastNamePetAllianceFrom;
                                    UpdateDamage(damage, line, exp, FilterType.PetAlliance);
                                    break;
                                case false:
                                    damage = exp.pDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionPetAllianceIsAttack = true;
                                        UpdateDamage(damage, line, exp, FilterType.PetAlliance);
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
                            damage = exp.pDamage;
                            switch (damage.Success)
                            {
                                case true:
                                    line.Source = _lastNameOtherFrom;
                                    UpdateDamage(damage, line, exp, FilterType.Other);
                                    break;
                                case false:
                                    damage = exp.pDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionOtherIsAttack = true;
                                        UpdateDamage(damage, line, exp, FilterType.Other);
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
                            damage = exp.pDamage;
                            switch (damage.Success)
                            {
                                case true:
                                    line.Source = _lastNamePetOtherFrom;
                                    UpdateDamage(damage, line, exp, FilterType.PetOther);
                                    break;
                                case false:
                                    damage = exp.pDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionPetOtherIsAttack = true;
                                        UpdateDamage(damage, line, exp, FilterType.PetOther);
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
                                    UpdateDamageMonster(damage, line, exp, FilterType.You);
                                    break;
                                case false:
                                    damage = exp.mDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionYouIsAttack = true;
                                        line.Target = You;
                                        UpdateDamageMonster(damage, line, exp, FilterType.You);
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
                                    line.Target = _lastNamePet;
                                    UpdateDamageMonster(damage, line, exp, FilterType.Pet);
                                    break;
                                case false:
                                    damage = exp.mDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionPetIsAttack = true;
                                        UpdateDamageMonster(damage, line, exp, FilterType.Pet);
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
                                    line.Target = _lastNamePartyTo;
                                    UpdateDamageMonster(damage, line, exp, FilterType.Party);
                                    break;
                                case false:
                                    damage = exp.mDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionPartyIsAttack = true;
                                        UpdateDamageMonster(damage, line, exp, FilterType.Party);
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
                                    line.Target = _lastNamePetPartyTo;
                                    UpdateDamageMonster(damage, line, exp, FilterType.PetParty);
                                    break;
                                case false:
                                    damage = exp.mDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionPetPartyIsAttack = true;
                                        UpdateDamageMonster(damage, line, exp, FilterType.PetParty);
                                    }
                                    break;
                            }
                            break;
                        case EventDirection.Alliance:
                            damage = exp.mDamage;
                            switch (damage.Success)
                            {
                                case true:
                                    line.Source = _lastNameMonster;
                                    line.Target = _lastNameAllianceTo;
                                    UpdateDamageMonster(damage, line, exp, FilterType.Alliance);
                                    break;
                                case false:
                                    damage = exp.mDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionAllianceIsAttack = true;
                                        UpdateDamageMonster(damage, line, exp, FilterType.Alliance);
                                    }
                                    break;
                            }
                            break;
                        case EventDirection.PetAlliance:
                            damage = exp.mDamage;
                            switch (damage.Success)
                            {
                                case true:
                                    line.Source = _lastNameMonster;
                                    line.Target = _lastNamePetAllianceTo;
                                    UpdateDamageMonster(damage, line, exp, FilterType.PetAlliance);
                                    break;
                                case false:
                                    damage = exp.mDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionPetAllianceIsAttack = true;
                                        UpdateDamageMonster(damage, line, exp, FilterType.PetAlliance);
                                    }
                                    break;
                            }
                            break;
                        case EventDirection.Other:
                            damage = exp.mDamage;
                            switch (damage.Success)
                            {
                                case true:
                                    line.Source = _lastNameMonster;
                                    line.Target = _lastNameOtherTo;
                                    UpdateDamageMonster(damage, line, exp, FilterType.Other);
                                    break;
                                case false:
                                    damage = exp.mDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionOtherIsAttack = true;
                                        UpdateDamageMonster(damage, line, exp, FilterType.Other);
                                    }
                                    break;
                            }
                            break;
                        case EventDirection.PetOther:
                            damage = exp.mDamage;
                            switch (damage.Success)
                            {
                                case true:
                                    line.Source = _lastNameMonster;
                                    line.Target = _lastNamePetOtherTo;
                                    UpdateDamageMonster(damage, line, exp, FilterType.PetOther);
                                    break;
                                case false:
                                    damage = exp.mDamageAuto;
                                    if (damage.Success)
                                    {
                                        _lastActionPetOtherIsAttack = true;
                                        UpdateDamageMonster(damage, line, exp, FilterType.PetOther);
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
            ParsingLogHelper.Log(Logger, "Damage", e, exp);
        }

        private static void UpdateDamage(Match damage, Line line, Expressions exp, FilterType type)
        {
            _type = type;
            try
            {
                line.Hit = true;
                if (String.IsNullOrWhiteSpace(line.Source))
                {
                    line.Source = Convert.ToString(damage.Groups["source"].Value);
                }
                if (String.IsNullOrWhiteSpace(line.Target))
                {
                    line.Target = Convert.ToString(damage.Groups["target"].Value);
                }
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
                            case FilterType.Alliance:
                                lastActionIsAttack = _lastActionAllianceIsAttack;
                                line.Action = _lastActionAllianceFrom;
                                break;
                            case FilterType.PetAlliance:
                                lastActionIsAttack = _lastActionPetAllianceIsAttack;
                                line.Action = _lastActionPetAllianceFrom;
                                break;
                            case FilterType.Other:
                                lastActionIsAttack = _lastActionOtherIsAttack;
                                line.Action = _lastActionOtherFrom;
                                break;
                            case FilterType.PetOther:
                                lastActionIsAttack = _lastActionPetOtherIsAttack;
                                line.Action = _lastActionPetOtherFrom;
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
                ParsingLogHelper.Error(Logger, "Damage", exp.Event, ex);
            }
        }

        private static void UpdateDamageMonster(Match damage, Line line, Expressions exp, FilterType type)
        {
            _type = type;
            try
            {
                line.Hit = true;
                if (String.IsNullOrWhiteSpace(line.Source))
                {
                    line.Source = Convert.ToString(damage.Groups["source"].Value);
                }
                if (String.IsNullOrWhiteSpace(line.Target))
                {
                    line.Target = Convert.ToString(damage.Groups["target"].Value);
                }
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
                            case FilterType.Alliance:
                                lastActionIsAttack = _lastActionAllianceIsAttack;
                                break;
                            case FilterType.PetAlliance:
                                lastActionIsAttack = _lastActionPetAllianceIsAttack;
                                break;
                            case FilterType.Other:
                                lastActionIsAttack = _lastActionOtherIsAttack;
                                break;
                            case FilterType.PetOther:
                                lastActionIsAttack = _lastActionPetOtherIsAttack;
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
                ParsingLogHelper.Error(Logger, "Damage", exp.Event, ex);
            }
        }
    }
}
