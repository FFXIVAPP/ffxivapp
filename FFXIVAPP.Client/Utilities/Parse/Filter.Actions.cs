// FFXIVAPP.Client
// Filter.Actions.cs
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
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Delegates;
using FFXIVAPP.Client.Enums.Parse;
using FFXIVAPP.Client.Helpers.Parse;
using FFXIVAPP.Client.Models.Parse;
using FFXIVAPP.Client.Models.Parse.Events;
using FFXIVAPP.Common.Helpers;

namespace FFXIVAPP.Client.Utilities.Parse
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
            LineHelper.SetTimelineTypes(ref line);
            if (LineHelper.IsIgnored(line))
            {
                return;
            }
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
                                UpdateActions(actions, line, exp, FilterType.You);
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
                                UpdateActions(actions, line, exp, FilterType.Pet);
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
                                UpdateActions(actions, line, exp, FilterType.Party);
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
                                UpdateActions(actions, line, exp, FilterType.PetParty);
                            }
                            break;
                    }
                    break;
                case EventSubject.Alliance:
                    switch (e.Direction)
                    {
                            // casts/uses
                        case EventDirection.Self:
                            actions = exp.pActions;
                            if (actions.Success)
                            {
                                UpdateActions(actions, line, exp, FilterType.Alliance);
                            }
                            break;
                    }
                    break;
                case EventSubject.PetAlliance:
                    switch (e.Direction)
                    {
                            // casts/uses
                        case EventDirection.Self:
                            actions = exp.pActions;
                            if (actions.Success)
                            {
                                UpdateActions(actions, line, exp, FilterType.PetAlliance);
                            }
                            break;
                    }
                    break;
                case EventSubject.Other:
                    switch (e.Direction)
                    {
                            // casts/uses
                        case EventDirection.Self:
                            actions = exp.pActions;
                            if (actions.Success)
                            {
                                UpdateActions(actions, line, exp, FilterType.Other);
                            }
                            break;
                    }
                    break;
                case EventSubject.PetOther:
                    switch (e.Direction)
                    {
                            // casts/uses
                        case EventDirection.Self:
                            actions = exp.pActions;
                            if (actions.Success)
                            {
                                UpdateActions(actions, line, exp, FilterType.PetOther);
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
                                UpdateActionsMonster(actions, line, exp, FilterType.MonsterParty);
                            }
                            break;
                    }
                    break;
            }
            if (actions.Success)
            {
                return;
            }
            ParsingLogHelper.Log(Logger, "Action", e, exp);
        }

        private static void UpdateActions(Match actions, Line line, Expressions exp, FilterType type)
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
                if (String.IsNullOrWhiteSpace(line.Source))
                {
                    line.Source = Convert.ToString(actions.Groups["source"].Value);
                }
                var isHealingSkill = false;
                var player = ParseControl.Instance.Timeline.GetSetPlayer(line.Source);
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
                        _lastNamePet = line.Source;
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
                    case FilterType.Alliance:
                        if (isHealingSkill)
                        {
                            _lastActionAllianceHealingFrom = action;
                            _lastNameAllianceHealingFrom = line.Source;
                        }
                        else
                        {
                            _lastActionAllianceFrom = action;
                            _lastNameAllianceFrom = line.Source;
                        }
                        break;
                    case FilterType.PetAlliance:
                        if (isHealingSkill)
                        {
                            _lastActionPetAllianceHealingFrom = action;
                            _lastNamePetAllianceHealingFrom = line.Source;
                        }
                        else
                        {
                            _lastActionPetAllianceFrom = action;
                            _lastNamePetAllianceFrom = line.Source;
                        }
                        break;
                    case FilterType.Other:
                        if (isHealingSkill)
                        {
                            _lastActionOtherHealingFrom = action;
                            _lastNameOtherHealingFrom = line.Source;
                        }
                        else
                        {
                            _lastActionOtherFrom = action;
                            _lastNameOtherFrom = line.Source;
                        }
                        break;
                    case FilterType.PetOther:
                        if (isHealingSkill)
                        {
                            _lastActionPetOtherHealingFrom = action;
                            _lastNamePetOtherHealingFrom = line.Source;
                        }
                        else
                        {
                            _lastActionPetOtherFrom = action;
                            _lastNamePetOtherFrom = line.Source;
                        }
                        break;
                }
                player.LastActionTime = DateTime.Now;
                try
                {
                    var players = PCWorkerDelegate.GetNPCEntities();
                    if (!players.Any())
                    {
                        return;
                    }
                    foreach (var actorEntity in players)
                    {
                        var playerName = actorEntity.Name;
                        ParseControl.Instance.Timeline.TrySetPlayerCurable(playerName, actorEntity.HPMax - actorEntity.HPCurrent);
                    }
                }
                catch (Exception ex)
                {
                }
            }
            catch (Exception ex)
            {
                ParsingLogHelper.Error(Logger, "Action", exp.Event, ex);
            }
        }

        private static void UpdateActionsMonster(Match actions, Line line, Expressions exp, FilterType type)
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
                ParsingLogHelper.Error(Logger, "Action", exp.Event, ex);
            }
        }
    }
}
