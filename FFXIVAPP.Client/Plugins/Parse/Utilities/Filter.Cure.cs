// FFXIVAPP.Client
// Filter.Cure.cs
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
        private static void ProcessCure(Event e, Expressions exp)
        {
            var line = new Line(e.ChatLogEntry)
            {
                EventDirection = e.Direction,
                EventSubject = e.Subject,
                EventType = e.Type
            };
            var cure = Regex.Match("ph", @"^\.$");
            switch (e.Subject)
            {
                case EventSubject.You:
                    switch (e.Direction)
                    {
                        case EventDirection.You:
                        case EventDirection.Pet:
                        case EventDirection.Party:
                        case EventDirection.PetParty:
                            cure = exp.pCure;
                            if (cure.Success)
                            {
                                line.Source = You;
                                UpdateHealingParty(cure, line, exp, FilterType.You);
                            }
                            break;
                    }
                    break;
                case EventSubject.Pet:
                    switch (e.Direction)
                    {
                        case EventDirection.You:
                        case EventDirection.Pet:
                        case EventDirection.Party:
                        case EventDirection.PetParty:
                            cure = exp.pCure;
                            if (cure.Success)
                            {
                                line.Source = _lastNamePet;
                                line.Source = ParseHelper.GetPetFromPlayer(line.Source, exp, TimelineType.You);
                                UpdateHealingParty(cure, line, exp, FilterType.Pet);
                            }
                            break;
                    }
                    break;
                case EventSubject.Party:
                    switch (e.Direction)
                    {
                        case EventDirection.You:
                        case EventDirection.Pet:
                        case EventDirection.Party:
                        case EventDirection.PetParty:
                            cure = exp.pCure;
                            if (cure.Success)
                            {
                                line.Source = _lastNamePartyHealingFrom;
                                UpdateHealingParty(cure, line, exp, FilterType.Party);
                            }
                            break;
                    }
                    break;
                case EventSubject.PetParty:
                    switch (e.Direction)
                    {
                        case EventDirection.You:
                        case EventDirection.Pet:
                        case EventDirection.Party:
                        case EventDirection.PetParty:
                            cure = exp.pCure;
                            if (cure.Success)
                            {
                                line.Source = _lastNamePetPartyHealingFrom;
                                line.Source = ParseHelper.GetPetFromPlayer(line.Source, exp, TimelineType.Party);
                                UpdateHealingParty(cure, line, exp, FilterType.PetParty);
                            }
                            break;
                    }
                    break;
            }
            if (cure.Success)
            {
                return;
            }
            ParsingLogHelper.Log(LogManager.GetCurrentClassLogger(), "Cure", e, exp);
        }

        private static void UpdateHealingParty(Match cure, Line line, Expressions exp, FilterType type)
        {
            _type = type;
            try
            {
                switch (type)
                {
                    case FilterType.You:
                        line.Action = _lastActionYou;
                        break;
                    case FilterType.Pet:
                        line.Action = _lastActionPet;
                        break;
                    case FilterType.Party:
                        line.Action = _lastActionPartyHealingFrom;
                        break;
                    case FilterType.PetParty:
                        line.Action = _lastActionPetPartyHealingFrom;
                        break;
                }
                line.Amount = cure.Groups["amount"].Success ? Convert.ToDecimal(cure.Groups["amount"].Value) : 0m;
                line.Crit = cure.Groups["crit"].Success;
                line.Modifier = cure.Groups["modifier"].Success ? Convert.ToDecimal(cure.Groups["modifier"].Value) / 100 : 0m;
                line.Target = exp.Event.Direction == EventDirection.You ? You : Convert.ToString(cure.Groups["target"].Value);
                line.RecLossType = Convert.ToString(cure.Groups["type"].Value.ToUpperInvariant());
                if (line.IsEmpty())
                {
                    return;
                }
                if (line.RecLossType == exp.HealingType)
                {
                    ParseControl.Instance.Timeline.GetSetPlayer(line.Source, TimelineType.Party)
                                .SetHealing(line);
                }
            }
            catch (Exception ex)
            {
                ParsingLogHelper.Error(LogManager.GetCurrentClassLogger(), "Cure", exp.Event, ex);
            }
        }
    }
}
