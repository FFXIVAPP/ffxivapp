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
        // setup self info

        // setup you
        private static Event _lastEventYou;
        private static string _lastActionYou = "";
        private static bool _lastActionYouIsAttack;

        // setup you pet
        private static Event _lastEventPet;
        private static string _lastNamePet = "";
        private static string _lastActionPet = "";
        private static bool _lastActionPetIsAttack;

        // setup party info
        private static Event _lastEventParty;
        private static string _lastNamePartyFrom = "";
        private static string _lastActionPartyFrom = "";
        private static string _lastNamePartyTo = "";
        private static bool _lastActionPartyIsAttack;

        // setup party pet info
        private static Event _lastEventPetParty;
        private static string _lastNamePetPartyFrom = "";
        private static string _lastActionPetPartyFrom = "";
        private static string _lastNamePetPartyTo = "";
        private static bool _lastActionPetPartyIsAttack;

        // setup alliance info
        private static Event _lastEventAlliance;
        private static string _lastNameAllianceFrom = "";
        private static string _lastActionAllianceFrom = "";
        private static string _lastNameAllianceTo = "";
        private static bool _lastActionAllianceIsAttack;

        // setup alliancepet  info
        private static Event _lastEventPetAlliance;
        private static string _lastNamePetAllianceFrom = "";
        private static string _lastActionPetAllianceFrom = "";
        private static string _lastNamePetAllianceTo = "";
        private static bool _lastActionPetAllianceIsAttack;

        // setup monster info
        private static string _lastNameMonster = "";
        private static string _lastActionMonster = "";
        private static bool _lastActionMonsterIsAttack;

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
