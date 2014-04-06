// FFXIVAPP.Client
// Filter.Failed.cs
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
            ParsingLogHelper.Log(Logger, "Failed", e, exp);
        }

        private static void UpdateFailed(Match failed, Line line, Expressions exp, FilterType type)
        {
            _type = type;
            try
            {
                line.Miss = true;
                if (String.IsNullOrWhiteSpace(line.Source))
                {
                    line.Source = Convert.ToString(failed.Groups["source"].Value);
                }
                if (String.IsNullOrWhiteSpace(line.Target))
                {
                    line.Target = Convert.ToString(failed.Groups["target"].Value);
                }
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
                ParsingLogHelper.Error(Logger, "Failed", exp.Event, ex);
            }
        }

        private static void UpdateFailedMonster(Match failed, Line line, Expressions exp, FilterType type)
        {
            _type = type;
            try
            {
                line.Miss = true;
                if (String.IsNullOrWhiteSpace(line.Source))
                {
                    line.Source = Convert.ToString(failed.Groups["source"].Value);
                }
                if (String.IsNullOrWhiteSpace(line.Target))
                {
                    line.Target = Convert.ToString(failed.Groups["target"].Value);
                }
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
                ParsingLogHelper.Error(Logger, "Failed", exp.Event, ex);
            }
        }
    }
}
