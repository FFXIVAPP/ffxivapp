// FFXIVAPP.Client
// LimitBreaks.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Linq;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Plugins.Parse.Models
{
    [DoNotObfuscate]
    public static class LimitBreaks
    {
        private static List<string> _limitBreakSkills;

        private static List<string> LimitBreakSkills
        {
            get { return _limitBreakSkills ?? (_limitBreakSkills = GetLimitBreakList()); }
            set { _limitBreakSkills = value; }
        }

        public static bool IsLimit(string action)
        {
            return LimitBreakSkills.Any(lb => String.Equals(lb, action, Constants.InvariantComparer));
        }

        private static List<string> GetLimitBreakList()
        {
            var culture = Constants.CultureInfo.TwoLetterISOLanguageName;
            switch (culture)
            {
                case "ja":
                    return new List<string>
                    {
                        // limit break
                        "シールドウォール",
                        "マイティガード",
                        "ラストバスティオン",
                        "スカイシャード",
                        "プチメテオ",
                        "メテオ",
                        "癒しの風",
                        "大地の息吹",
                        "生命の鼓動"
                    };
                case "de":
                    return new List<string>
                    {
                        // limit break
                        "schutzschild",
                        "totalabwehr",
                        "letzte bastion",
                        "himmelsscherbe",
                        "sternensturm",
                        //"meteor",
                        "heilender wind",
                        "atem der erde",
                        "lebenspuls"
                    };
                case "fr":
                    return new List<string>
                    {
                        // limit break
                        "mur protecteur",
                        "garde puissante",
                        "dernier bastion",
                        "éclat de ciel",
                        "tempête d'étoiles",
                        "météore",
                        "vent curateur",
                        "souffle de la terre",
                        "pulsation vitale"
                    };
            }
            return new List<string>
            {
                // limit break
                "shield wall",
                "might guard",
                "last bastion",
                "skyshard",
                "starstorm",
                "meteor",
                "healing wind",
                "breath of earth",
                "pulse of life"
            };
        }
    }
}
