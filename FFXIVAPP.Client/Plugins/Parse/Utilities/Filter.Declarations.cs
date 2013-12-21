// FFXIVAPP.Client
// Filter.Declarations.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Client.Plugins.Parse.Models.Events;

namespace FFXIVAPP.Client.Plugins.Parse.Utilities
{
    public static partial class Filter
    {
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
        private static string _lastNamePartyHealingFrom = "";
        private static string _lastActionPartyHealingFrom = "";
        private static string _lastNamePartyTo = "";
        private static bool _lastActionPartyIsAttack;

        // setup party pet info
        private static Event _lastEventPetParty;
        private static string _lastNamePetPartyFrom = "";
        private static string _lastActionPetPartyFrom = "";
        private static string _lastNamePetPartyHealingFrom = "";
        private static string _lastActionPetPartyHealingFrom = "";
        private static string _lastNamePetPartyTo = "";
        private static bool _lastActionPetPartyIsAttack;

        // setup alliance info
        private static Event _lastEventAlliance;
        private static string _lastNameAllianceFrom = "";
        private static string _lastActionAllianceFrom = "";
        private static string _lastNameAllianceHealingFrom = "";
        private static string _lastActionAllianceHealingFrom = "";
        private static string _lastNameAllianceTo = "";
        private static bool _lastActionAllianceIsAttack;

        // setup alliancepet  info
        private static Event _lastEventPetAlliance;
        private static string _lastNamePetAllianceFrom = "";
        private static string _lastActionPetAllianceFrom = "";
        private static string _lastNamePetAllianceHealingFrom = "";
        private static string _lastActionPetAllianceHealingFrom = "";
        private static string _lastNamePetAllianceTo = "";
        private static bool _lastActionPetAllianceIsAttack;

        // setup monster info
        private static string _lastNameMonster = "";
        private static string _lastActionMonster = "";
        private static bool _lastActionMonsterIsAttack;
    }
}
