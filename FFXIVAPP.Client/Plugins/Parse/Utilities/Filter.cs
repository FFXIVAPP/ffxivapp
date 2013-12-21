// FFXIVAPP.Client
// Filter.cs
// 
// © 2013 Ryan Wilson

using System;
using FFXIVAPP.Client.Plugins.Parse.Enums;
using FFXIVAPP.Client.Plugins.Parse.Models;
using FFXIVAPP.Client.Plugins.Parse.Models.Events;
using FFXIVAPP.Client.Properties;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Plugins.Parse.Utilities
{
    [DoNotObfuscate]
    public static partial class Filter
    {
        private static FilterType _type;

        private static string You
        {
            get { return String.IsNullOrWhiteSpace(Constants.CharacterName.Trim()) ? "You" : Constants.CharacterName; }
        }

        public static void Process(Event e)
        {
            if (!ParseControl.Instance.FirstActionFound)
            {
                ParseControl.Instance.Reset();
            }

            _lastEventYou = _lastEventYou ?? new Event();
            _lastEventPet = _lastEventPet ?? new Event();
            _lastEventParty = _lastEventParty ?? new Event();
            _lastEventPetParty = _lastEventPetParty ?? new Event();
            _lastEventAlliance = _lastEventAlliance ?? new Event();
            _lastEventPetAlliance = _lastEventPetAlliance ?? new Event();

            _type = FilterType.Unknown;

            var expressions = new Expressions(e);

            switch (Settings.Default.StoreHistoryEvent)
            {
                case "Any":
                    ParseControl.Instance.Timeline.StoreHistoryTimer.Stop();
                    break;
                case "Damage Only":
                    if (e.Type == EventType.Damage)
                    {
                        ParseControl.Instance.Timeline.StoreHistoryTimer.Stop();
                    }
                    break;
            }

            if (e.Type == EventType.Damage)
            {
                ParseControl.Instance.Timeline.FightingRightNow = true;
                ParseControl.Instance.Timeline.FightingTimer.Stop();
            }

            switch (e.Type)
            {
                case EventType.Damage:
                    ProcessDamage(e, expressions);
                    break;
                case EventType.Failed:
                    ProcessFailed(e, expressions);
                    break;
                case EventType.Actions:
                    ProcessActions(e, expressions);
                    break;
                case EventType.Items:
                    ProcessItems(e, expressions);
                    break;
                case EventType.Cure:
                    ProcessCure(e, expressions);
                    break;
                case EventType.Beneficial:
                    ProcessBeneficial(e, expressions);
                    break;
                case EventType.Detrimental:
                    ProcessDetrimental(e, expressions);
                    break;
            }

            switch (_type)
            {
                case FilterType.You:
                    _lastEventYou = e;
                    break;
                case FilterType.Pet:
                    _lastEventPet = e;
                    break;
                case FilterType.Party:
                    _lastEventParty = e;
                    break;
                case FilterType.PetParty:
                    _lastEventPetParty = e;
                    break;
                case FilterType.Alliance:
                    _lastEventAlliance = e;
                    break;
                case FilterType.PetAlliance:
                    _lastEventPetAlliance = e;
                    break;
            }

            switch (Settings.Default.StoreHistoryEvent)
            {
                case "Any":
                    ParseControl.Instance.Timeline.StoreHistoryTimer.Start();
                    break;
                case "Damage Only":
                    if (e.Type == EventType.Damage)
                    {
                        ParseControl.Instance.Timeline.StoreHistoryTimer.Start();
                    }
                    break;
            }

            if (e.Type == EventType.Damage)
            {
                ParseControl.Instance.Timeline.FightingTimer.Start();
            }
        }
    }
}
