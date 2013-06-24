// FFXIVAPP.Plugin.Parse
// MultiTarget.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Collections.Generic;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models
{
    public static class MultiTarget
    {
        private static List<string> _multiTargetSkills;

        private static List<string> MultiTargetSkills
        {
            get { return _multiTargetSkills ?? (_multiTargetSkills = GetMultiTargetList()); }
            set { _multiTargetSkills = value; }
        }

        public static bool IsMulti(string action)
        {
            return MultiTargetSkills.Contains(action.ToLower());
        }

        private static List<string> GetMultiTargetList()
        {
            return new List<string>
            {
                "shield wall",
                "might guard",
                "last bastion",
                "skyshard",
                "starstorm",
                "meteor",
                "healing wind",
                "breath of earth",
                "wide volley",
                "flaming arrow",
                "swiftsong",
                "quick knock",
                "rain of death",
                "army's paeon",
                "foe requiem",
                "mage's ballad",
                "fire ii",
                "blizzard ii",
                "sleep",
                "flare",
                "apocatastasis",
                "freeze",
                "medica ii",
                "cure iii",
                "medica",
                "protect",
                "holy",
                "ring of thorns",
                "doom spike",
                "dragonfire dive",
                "overpower",
                "steel cyclone",
                "howling fist",
                "arm of the destroyer",
                "rockbreaker",
                "pulse of life",
                "circle of scorn",
                "flash"
            };
        }
    }
}
