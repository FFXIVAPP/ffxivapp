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
            var culture = Common.Constants.CultureInfo.TwoLetterISOLanguageName;
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
                        "生命の鼓動",
                        // arc
                        "ワイドボレー",
                        "フレイミングアロー",
                        "スウィフトソング",
                        "クイックノック",
                        // brd
                        "レイン・オブ・デス",
                        "軍神のパイオン",
                        "魔人のレクイエム",
                        "賢人のバラード",
                        // thm
                        "ファイラ",
                        "ブリザラ",
                        "スリプル",
                        // blm
                        "フレア",
                        "アポカタスタシス",
                        "フリーズ",
                        // cnj
                        "メディカラ",
                        "ケアルガ",
                        "メディカ",
                        "プロテス",
                        // whm
                        "ホーリー",
                        // lnc
                        "リング・オブ・ソーン",
                        "ドゥームスパイク",
                        // drg
                        "ドラゴンダイブ",
                        // mrd
                        "オーバーパワー",
                        // war
                        "スチールサイクロン",
                        // pug
                        "空鳴拳",
                        "壊神衝",
                        // mnk
                        "地烈斬",
                        // gla
                        "サークル・オブ・ドゥーム",
                        "フラッシュ"
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
                        "lebenspuls",
                        // arc
                        "sternenhagel",
                        "flammenpfeil",
                        "schmissiger song",
                        "pfeilsalve",
                        // brd
                        "tödlicher regen",
                        "hymne der krieger",
                        "requiem der feinde",
                        "ballade des weisen",
                        // thm
                        "feura",
                        "eisra",
                        "schlaf",
                        // blm
                        "flare",
                        "apokatastasis",
                        "einfrieren",
                        // cnj
                        "resedra",
                        "heilga",
                        "medica",
                        "protes",
                        // whm
                        "sanctus",
                        // lnc
                        "dornenkranz",
                        "schicksalsdorn",
                        // drg
                        "wyrm-odem",
                        // mrd
                        "kahlrodung",
                        // war
                        "kreiselklinge",
                        // pug
                        "heulende faust",
                        "arm des zerstörers",
                        // mnk
                        "erdspaltung",
                        // gla
                        "kreis der verachtung",
                        "blitzlicht"
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
                        "pulsation vitale",
                        // arc
                        "rafale impitoyable",
                        "flèche enflammée",
                        "chant rapide",
                        "salve fulgurante",
                        // brd
                        "pluie mortelle",
                        "péan martial",
                        "requiem ennemi",
                        "ballade du mage",
                        // thm
                        "extra feu",
                        "extra glace",
                        "sommeil",
                        // blm
                        "brasier",
                        "apocatastase",
                        "gel",
                        // cnj
                        "extra médica",
                        "méga soin",
                        "médica",
                        "bouclier",
                        // whm
                        "miracle",
                        // lnc
                        "anneau de ronces",
                        "pointe du destin",
                        // drg
                        "piqué du dragon",
                        // mrd
                        "domination",
                        // war
                        "cyclone de fer",
                        // pug
                        "poing hurlant",
                        "frappe du destructeur",
                        // mnk
                        "briseur de rocs",
                        // gla
                        "cercle du destin",
                        //"flash"
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
                "pulse of life",
                // arc
                "wide volley",
                "flaming arrow",
                "swiftsong",
                "quick knock",
                // brd
                "rain of death",
                "army's paeon",
                "foe requiem",
                "mage's ballad",
                // thm
                "fire ii",
                "blizzard ii",
                "sleep",
                // blm
                "flare",
                "apocatastasis",
                "freeze",
                // cnj
                "medica ii",
                "cure iii",
                "medica",
                "protect",
                // whm
                "holy",
                // lnc
                "ring of thorns",
                "doom spike",
                "dragonfire dive",
                // mrd
                "overpower",
                // war
                "steel cyclone",
                // pug
                "howling fist",
                "arm of the destroyer",
                // mnk
                "rockbreaker",
                // gla
                "circle of scorn",
                "flash"
            };
        }
    }
}
