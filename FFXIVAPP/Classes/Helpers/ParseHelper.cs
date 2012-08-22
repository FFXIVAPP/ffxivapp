// FFXIVAPP
// ParseHelper.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.Collections;
using NLog;

namespace FFXIVAPP.Classes.Helpers
{
    public static class ParseHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public struct LineData
        {
            public string Job;
            public string Source;
            public string Target;
            public string Action;
            public string Direction;
            public string Part;
            public decimal Amount;
            public string Type;
            public bool Hit;
            public bool Miss;
            public bool Crit;
            public bool Counter;
            public bool Block;
            public bool Parry;
            public bool Resist;
            public bool Evade;
            public bool Partial;
        }

        #region Job Info

        public static readonly string[] MultiEn = new[] {"Simian Thrash", "Rage of Halone", "Godsbane", "Dragon Kick", "Quick Nock", "Chaos Thrust"};
        public static readonly string[] MultiFr = new[] {"Déchaînement", "Rage de Halone", "Fléau des dieux", "Tacle du dragon", "Salve fulgurante", "Percée chaotique"};
        public static readonly string[] MultiJa = new[] {"猿猴九連掌", "レイジオブハルオーネ", "ゴッズベーン", "双竜脚", "クイックノック", "桜華狂咲"};
        public static readonly string[] MultiDe = new[] {"Ekajatis Zorn", "Halones Groll", "Götterbann", "Zwillingsdrache", "Pfeilsalve", "Chaotischer Tjost"};

        public static Hashtable GetJob()
        {
            Hashtable offsets;
            switch (MainWindow.Lang)
            {
                case "ja":
                case "de":
                case "fr":
                default:
                    offsets = new Hashtable {{"phalanx", "gladiator"}, {"aegis boon", "gladiator"}, {"riot blade", "gladiator"}, {"war drum", "gladiator"}, {"tempered will", "gladiator"}, {"rage of halone", "gladiator"}, {"goring blade", "gladiator"}, {"cover", "paladin"}, {"divine veil", "paladin"}, {"hallowed ground", "paladin"}, {"holy succor", "paladin"}, {"spirits within", "paladin"}, {"pounce", "puglist"}, {"haymaker", "puglist"}, {"fists of earth", "puglist"}, {"fists of fire", "puglist"}, {"aura pulse", "puglist"}, {"taunt", "puglist"}, {"howling fist", "puglist"}, {"simian thrash", "puglist"}, {"shoulder tackle", "monk"}, {"spinning heal", "monk"}, {"fists of wind", "monk"}, {"dragon kick", "monk"}, {"hundred fists", "monk"}, {"fracture", "marauder"}, {"berserk", "marauder"}, {"rampage", "marauder"}, {"path of the storm", "marauder"}, {"enduring march", "marauder"}, {"whirlwind", "marauder"}, {"godsbane", "marauder"}, {"vengeance", "warrior"}, {"antagonize", "warrior"}, {"collusion", "warrior"}, {"mighty strikes", "warrior"}, {"steel cyclone", "warrior"}, {"life surge", "lancer"}, {"power surge", "lancer"}, {"full thrust", "lancer"}, {"dread spike", "lancer"}, {"doom spike", "lancer"}, {"chaos thrust", "lancer"}, {"jump", "dragoon"}, {"elusive jump", "dragoon"}, {"dragonfire dive", "dragoon"}, {"disembowel", "dragoon"}, {"ring of talons", "dragoon"}, {"light shot", "archer"}, {"raging strike", "archer"}, {"shadowbind", "archer"}, {"swiftsong", "archer"}, {"barrage", "archer"}, {"quick nock", "archer"}, {"bloodletter", "archer"}, {"wide volley", "archer"}, {"battle voice", "bard"}, {"rain of death", "bard"}, {"ballad of magi", "bard"}, {"paeon of war", "bard"}, {"minuet of rigor", "bard"}, {"cleric stance", "conjurer"}, {"blissful mind", "conjurer"}, {"stonera", "conjurer"}, {"cura", "conjurer"}, {"shroud of saints", "conjurer"}, {"aerora", "conjurer"}, {"curaga", "conjurer"}, {"repose", "conjurer"}, {"presence of mind", "white mage"}, {"benediction", "white mage"}, {"esuna", "white mage"}, {"regen", "white mage"}, {"holy", "white mage"}, {"parsimony", "thaumaturgy"}, {"blizzard", "thaumaturgy"}, {"thundara", "thaumaturgy"}, {"blizzara", "thaumaturgy"}, {"excruciate", "thaumaturgy"}, {"sleep", "thaumaturgy"}, {"thundaga", "thaumaturgy"}, {"firaga", "thaumaturgy"}, {"convert", "black mage"}, {"burst", "black mage"}, {"sleepga", "black mage"}, {"flare", "black mage"}, {"freeze", "black mage"}};
                    break;
            }
            return offsets;
        }

        #endregion
    }
}