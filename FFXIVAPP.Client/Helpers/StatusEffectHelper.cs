// FFXIVAPP.Client
// StatusEffectHelper.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using System.Linq;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Helpers
{
    [DoNotObfuscate]
    public static class StatusEffectHelper
    {
        private static Dictionary<int, StatusItem> _statusEffects;

        private static Dictionary<int, StatusItem> StatusEffects
        {
            get { return _statusEffects ?? (_statusEffects = new Dictionary<int, StatusItem>()); }
            set
            {
                if (_statusEffects == null)
                {
                    _statusEffects = new Dictionary<int, StatusItem>();
                }
                _statusEffects = value;
            }
        }

        public static StatusItem StatusInfo(int id)
        {
            if (!StatusEffects.Any())
            {
                Generate();
            }
            if (StatusEffects.ContainsKey(id))
            {
                return StatusEffects[id];
            }
            return new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "???",
                    French = "???",
                    German = "???",
                    Japanese = "???"
                },
                CompanyAction = false
            };
        }

        private static void Generate()
        {
            StatusEffects.Add(1, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Petrification",
                    French = "Pétrification",
                    German = "Stein",
                    Japanese = "石化"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(2, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Stun",
                    French = "Étourdissement",
                    German = "Betäubung",
                    Japanese = "スタン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(3, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Sleep",
                    French = "Sommeil",
                    German = "Schlaf",
                    Japanese = "睡眠"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(4, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Daze",
                    French = "Évanouissement",
                    German = "Benommenheit",
                    Japanese = "気絶"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(5, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Amnesia",
                    French = "Amnésie",
                    German = "Amnesie",
                    Japanese = "アビリティ不可"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(6, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Pacification",
                    French = "Pacification",
                    German = "Pacem",
                    Japanese = "ＷＳ不可"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(7, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Silence",
                    French = "Silence",
                    German = "Stumm",
                    Japanese = "沈黙"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(8, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Haste",
                    French = "Hâte",
                    German = "Hast",
                    Japanese = "ヘイスト"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(9, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Slow",
                    French = "Lenteur",
                    German = "Gemach",
                    Japanese = "スロウ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(10, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Slow",
                    French = "Lenteur",
                    German = "Gemach",
                    Japanese = "拘束装置：スロウ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(13, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Bind",
                    French = "Entrave",
                    German = "Fessel",
                    Japanese = "バインド"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(14, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Heavy",
                    French = "Pesanteur",
                    German = "Gewicht",
                    Japanese = "ヘヴィ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(15, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Blind",
                    French = "Cécité",
                    German = "Blind",
                    Japanese = "暗闇"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(17, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Paralysis",
                    French = "Paralysie",
                    German = "Paralyse",
                    Japanese = "麻痺"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(18, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Poison",
                    French = "Poison",
                    German = "Gift",
                    Japanese = "毒"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(19, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Pollen",
                    French = "Poison violent",
                    German = "Giftpollen",
                    Japanese = "猛毒"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(20, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "TP Bleed",
                    French = "Saignée de PT",
                    German = "TP-Verlust",
                    Japanese = "ＴＰ継続ダメージ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(21, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "HP Boost",
                    French = "Bonus de PV",
                    German = "LP-Bonus",
                    Japanese = "ＨＰmaxアップ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(22, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "HP Penalty",
                    French = "Malus de PV",
                    German = "LP-Malus",
                    Japanese = "ＨＰmaxダウン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(23, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "MP Boost",
                    French = "Bonus de PM",
                    German = "MP-Bonus",
                    Japanese = "ＭＰmaxアップ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(24, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "MP Penalty",
                    French = "Malus de PM",
                    German = "MP-Malus",
                    Japanese = "ＭＰmaxダウン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(25, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Attack Up",
                    French = "Bonus d'attaque",
                    German = "Attacke-Bonus",
                    Japanese = "物理攻撃力アップ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(26, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Attack Down",
                    French = "Malus d'attaque",
                    German = "Attacke-Malus",
                    Japanese = "物理攻撃力ダウン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(27, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Accuracy Up",
                    French = "Bonus de précision",
                    German = "Präzisions-Bonus",
                    Japanese = "命中率アップ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(28, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Accuracy Down",
                    French = "Malus de précision",
                    German = "Präzisions-Malus",
                    Japanese = "命中率ダウン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(29, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Defense Up",
                    French = "Bonus de défense",
                    German = "Verteidigungs-Bonus",
                    Japanese = "物理防御力アップ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(30, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Defense Down",
                    French = "Malus de défense",
                    German = "Verteidigungs-Malus",
                    Japanese = "物理防御力ダウン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(31, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Evasion Up",
                    French = "Bonus d'esquive",
                    German = "Ausweich-Bonus",
                    Japanese = "回避力アップ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(32, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Evasion Down",
                    French = "Malus d'esquive",
                    German = "Ausweich-Malus",
                    Japanese = "回避力ダウン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(33, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Magic Potency Up",
                    French = "Bonus de puissance magique",
                    German = "Offensivmagie-Bonus",
                    Japanese = "魔法攻撃力アップ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(34, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Magic Potency Down",
                    French = "Malus de puissance magique",
                    German = "Offensivmagie-Malus",
                    Japanese = "魔法攻撃力ダウン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(35, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Healing Potency Up",
                    French = "Bonus de magie curative",
                    German = "Heilmagie-Bonus",
                    Japanese = "魔法回復力アップ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(36, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Healing Potency Down",
                    French = "Malus de magie curative",
                    German = "Heilmagie-Malus",
                    Japanese = "魔法回復力ダウン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(37, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Magic Defense Up",
                    French = "Bonus de défense magique",
                    German = "Magieabwehr-Bonus",
                    Japanese = "魔法防御力アップ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(38, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Magic Defense Down",
                    French = "Malus de défense magique",
                    German = "Magieabwehr-Malus",
                    Japanese = "魔法防御力ダウン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(43, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Weakness",
                    French = "Affaiblissement",
                    German = "Schwäche",
                    Japanese = "衰弱"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(44, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Brink of Death",
                    French = "Mourant",
                    German = "Sterbenselend",
                    Japanese = "衰弱［強］"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(45, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Crafter's Grace",
                    French = "Grâce de l'artisan",
                    German = "Sternstunde der Handwerker",
                    Japanese = "経験値アップ（クラフター専用）"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(46, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Gatherer's Grace",
                    French = "Grâce du récolteur",
                    German = "Sternstunde der Sammler",
                    Japanese = "経験値アップ（ギャザラー専用）"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(47, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Stealth",
                    French = "Furtivité",
                    German = "Coeurl-Pfoten",
                    Japanese = "ステルス"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(48, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Well Fed",
                    French = "Repu",
                    German = "Gut gesättigt",
                    Japanese = "食事"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(49, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Medicated",
                    French = "Médicamenté",
                    German = "Stärkung",
                    Japanese = "強化薬"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(50, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Sprint",
                    French = "Sprint",
                    German = "Sprint",
                    Japanese = "スプリント"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(51, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Strength Down",
                    French = "Malus de force",
                    German = "Stärke-Malus",
                    Japanese = "ＳＴＲダウン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(52, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Vitality Down",
                    French = "Malus de vitalité",
                    German = "Konstitutions-Malus",
                    Japanese = "ＶＩＴダウン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(53, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Physical Damage Up",
                    French = "Bonus de dégâts physiques",
                    German = "Schadenswert +",
                    Japanese = "物理ダメージ上昇"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(54, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Physical Damage Down",
                    French = "Malus de dégâts physiques",
                    German = "Schadenswert -",
                    Japanese = "物理ダメージ低下"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(55, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Physical Vulnerability Down",
                    French = "Vulnérabilité physique diminuée",
                    German = "Verringerte physische Verwundbarkeit",
                    Japanese = "被物理ダメージ軽減"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(56, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Physical Vulnerability Up",
                    French = "Vulnérabilité physique augmentée",
                    German = "Erhöhte physische Verwundbarkeit",
                    Japanese = "被物理ダメージ増加"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(57, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Magic Damage Up",
                    French = "Bonus de dégâts magiques",
                    German = "Magieschaden +",
                    Japanese = "魔法ダメージ上昇"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(58, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Magic Damage Down",
                    French = "Malus de dégâts magiques",
                    German = "Magieschaden -",
                    Japanese = "魔法ダメージ低下"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(59, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Magic Vulnerability Down",
                    French = "Vulnérabilité magique diminuée",
                    German = "Verringerte Magie-Verwundbarkeit",
                    Japanese = "被魔法ダメージ軽減"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(60, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Magic Vulnerability Up",
                    French = "Vulnérabilité magique augmentée",
                    German = "Erhöhte Magie-Verwundbarkeit",
                    Japanese = "被魔法ダメージ増加"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(61, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Determination Up",
                    French = "Bonus de dégâts",
                    German = "Schaden +",
                    Japanese = "ダメージ上昇"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(62, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Determination Down",
                    French = "Malus de dégâts",
                    German = "Schaden -",
                    Japanese = "ダメージ低下"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(63, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Vulnerability Down",
                    French = "Vulnérabilité diminuée",
                    German = "Verringerte Verwundbarkeit",
                    Japanese = "被ダメージ低下"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(64, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Vulnerability Up",
                    French = "Vulnérabilité augmentée",
                    German = "Erhöhte Verwundbarkeit",
                    Japanese = "被ダメージ上昇"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(65, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Critical Skill",
                    French = "Maîtrise critique",
                    German = "Kritisches Potenzial",
                    Japanese = "ウェポンスキル強化：クリティカル"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(66, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Terror",
                    French = "Terreur",
                    German = "Terror",
                    Japanese = "恐怖"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(67, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Leaden",
                    French = "Plombé",
                    German = "Bleischwere",
                    Japanese = "ヘヴィ[強]"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(68, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Drainstrikes",
                    French = "Coups drainants",
                    German = "Auszehren",
                    Japanese = "オートアタック強化：ＨＰ吸収"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(69, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Aspirstrikes",
                    French = "Coups aspirants",
                    German = "Auslaugen",
                    Japanese = "オートアタック強化：ＴＰ吸収"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(70, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Stunstrikes",
                    French = "Coups étourdissants",
                    German = "Ausschalten",
                    Japanese = "オートアタック強化：スタン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(71, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Rampart",
                    French = "Rempart",
                    German = "Schutzwall",
                    Japanese = "ランパート"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(72, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Convalescence",
                    French = "Convalescence",
                    German = "Konvaleszenz",
                    Japanese = "コンバレセンス"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(73, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Awareness",
                    French = "Diligence",
                    German = "Achtsamkeit",
                    Japanese = "アウェアネス"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(74, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Sentinel",
                    French = "Sentinelle",
                    German = "Sentinel",
                    Japanese = "センチネル"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(75, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Tempered Will",
                    French = "Volonté d'acier",
                    German = "Eherner Wille",
                    Japanese = "鋼の意志"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(76, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Fight or Flight",
                    French = "Combat acharné",
                    German = "Verwegenheit",
                    Japanese = "ファイト・オア・フライト"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(77, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Bulwark",
                    French = "Forteresse",
                    German = "Bollwerk",
                    Japanese = "ブルワーク"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(78, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Sword Oath",
                    French = "Serment de l'épée",
                    German = "Schwert-Eid",
                    Japanese = "忠義の剣"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(79, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Shield Oath",
                    French = "Serment du bouclier",
                    German = "Schild-Eid",
                    Japanese = "忠義の盾"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(80, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Cover",
                    French = "Couverture",
                    German = "Deckung",
                    Japanese = "かばう"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(81, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Covered",
                    French = "Couvert",
                    German = "Gedeckt",
                    Japanese = "被かばう"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(82, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Hallowed Ground",
                    French = "Invincible",
                    German = "Heiliger Boden",
                    Japanese = "インビンシブル"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(83, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Foresight",
                    French = "Aguet",
                    German = "Vorahnung",
                    Japanese = "フォーサイト"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(84, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Bloodbath",
                    French = "Bain de sang",
                    German = "Blutbad",
                    Japanese = "ブラッドバス"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(85, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Maim",
                    French = "Mutilation",
                    German = "Verstümmelung",
                    Japanese = "メイム"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(86, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Berserk",
                    French = "Berserk",
                    German = "Tollwut",
                    Japanese = "バーサク"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(87, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Thrill of Battle",
                    French = "Frisson de la bataille",
                    German = "Kampfrausch",
                    Japanese = "スリル・オブ・バトル"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(88, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Holmgang",
                    French = "Holmgang",
                    German = "Holmgang",
                    Japanese = "ホルムギャング"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(89, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Vengeance",
                    French = "Représailles",
                    German = "Rache",
                    Japanese = "ヴェンジェンス"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(90, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Storm's Eye",
                    French = "Œil de la tempête",
                    German = "Sturmbrecher",
                    Japanese = "シュトルムブレハ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(91, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Defiance",
                    French = "Défi",
                    German = "Verteidiger",
                    Japanese = "ディフェンダー"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(92, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Unchained",
                    French = "Affranchissement",
                    German = "Entfesselt",
                    Japanese = "アンチェインド"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(93, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Wrath",
                    French = "Rage",
                    German = "Zorn",
                    Japanese = "ラース"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(94, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Wrath II",
                    French = "Rage II",
                    German = "Zorn II",
                    Japanese = "ラースII"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(95, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Wrath III",
                    French = "Rage III",
                    German = "Zorn III",
                    Japanese = "ラースIII"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(96, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Wrath IV",
                    French = "Rage IV",
                    German = "Zorn IV",
                    Japanese = "ラースIV"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(97, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Infuriated",
                    French = "Rage V",
                    German = "Zorn V",
                    Japanese = "ラースV"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(98, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Dragon Kick",
                    French = "Tacle du dragon",
                    German = "Drachentritt",
                    Japanese = "双竜脚"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(99, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Featherfoot",
                    French = "Pieds légers",
                    German = "Leichtfuß",
                    Japanese = "フェザーステップ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(100, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Internal Release",
                    French = "Relâchement intérieur",
                    German = "Innere Gelöstheit",
                    Japanese = "発勁"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(101, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Twin Snakes",
                    French = "Serpents jumeaux",
                    German = "Doppelviper",
                    Japanese = "双掌打"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(102, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Mantra",
                    French = "Mantra",
                    German = "Mantra",
                    Japanese = "マントラ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(103, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Fists of Fire",
                    French = "Poings de feu",
                    German = "Sengende Aura",
                    Japanese = "紅蓮の構え"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(104, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Fists of Earth",
                    French = "Poings de terre",
                    German = "Steinerne Aura",
                    Japanese = "金剛の構え"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(105, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Fists of Wind",
                    French = "Poings de vent",
                    German = "Beflügelnde Aura",
                    Japanese = "疾風の構え"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(106, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Touch of Death",
                    French = "Toucher mortel",
                    German = "Hauch des Todes",
                    Japanese = "秘孔拳"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(107, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Opo-opo Form",
                    French = "Posture de l'opo-opo",
                    German = "Opo-Opo-Form",
                    Japanese = "壱の型：魔猿"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(108, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Raptor Form",
                    French = "Posture du raptor",
                    German = "Raptor-Form",
                    Japanese = "弐の型：走竜"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(109, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Coeurl Form",
                    French = "Posture du coeurl",
                    German = "Coeurl-Form",
                    Japanese = "参の型：猛虎"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(110, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Perfect Balance",
                    French = "Équilibre parfait",
                    German = "Improvisation",
                    Japanese = "踏鳴"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(111, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Greased Lightning",
                    French = "Vitesse de l'éclair",
                    German = "Geölter Blitz",
                    Japanese = "疾風迅雷"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(112, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Greased Lightning II",
                    French = "Vitesse de l'éclair II",
                    German = "Geölter Blitz II",
                    Japanese = "疾風迅雷II"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(113, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Greased Lightning III",
                    French = "Vitesse de l'éclair III",
                    German = "Geölter Blitz III",
                    Japanese = "疾風迅雷III"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(114, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Keen Flurry",
                    French = "Volée défensive",
                    German = "Auge des Sturms",
                    Japanese = "キーンフラーリ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(115, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Heavy Thrust",
                    French = "Percée puissante",
                    German = "Gewaltiger Stoß",
                    Japanese = "ヘヴィスラスト"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(116, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Life Surge",
                    French = "Souffle de vie",
                    German = "Vitalwallung",
                    Japanese = "ライフサージ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(117, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Blood for Blood",
                    French = "Du sang pour du sang",
                    German = "Zahn um Zahn",
                    Japanese = "捨身"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(118, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Chaos Thrust",
                    French = "Percée chaotique",
                    German = "Chaotischer Tjost",
                    Japanese = "桜花狂咲"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(119, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Phlebotomize",
                    French = "Double percée",
                    German = "Phlebotomie",
                    Japanese = "二段突き"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(120, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Power Surge",
                    French = "Souffle de puissance",
                    German = "Drachenklaue",
                    Japanese = "竜槍"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(121, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Disembowel",
                    French = "Éventration",
                    German = "Drachengriff",
                    Japanese = "ディセムボウル"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(122, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Straighter Shot",
                    French = "Tir à l'arc surpuissant",
                    German = "Direkter Schuss +",
                    Japanese = "ストレートショット効果アップ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(123, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Hawk's Eye",
                    French = "Œil de faucon",
                    German = "Falkenauge",
                    Japanese = "ホークアイ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(124, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Venomous Bite",
                    French = "Morsure venimeuse",
                    German = "Infizierte Wunde",
                    Japanese = "ベノムバイト"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(125, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Raging Strikes",
                    French = "Tir furieux",
                    German = "Wütende Attacke",
                    Japanese = "猛者の撃"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(126, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Freeshot",
                    French = "Tir en cloche",
                    German = "Weitschuss +",
                    Japanese = "ファーショット効果アップ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(127, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Quelling Strikes",
                    French = "Frappe silencieuse",
                    German = "Heimliche Attacke",
                    Japanese = "静者の撃"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(128, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Barrage",
                    French = "Rafale de coups",
                    German = "Sperrfeuer",
                    Japanese = "乱れ撃ち"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(129, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Windbite",
                    French = "Morsure du vent",
                    German = "Beißender Wind",
                    Japanese = "ウィンドバイト"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(130, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Straight Shot",
                    French = "Tir droit",
                    German = "Direkter Schuss",
                    Japanese = "ストレートショット"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(131, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Downpour of Death",
                    French = "Déluge mortel",
                    German = "Tödlicher Regen +",
                    Japanese = "レイン・オブ・デス効果アップ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(132, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Quicker Nock",
                    French = "Salve fulgurante améliorée",
                    German = "Pfeilsalve +",
                    Japanese = "クイックノック効果アップ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(133, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Swiftsong",
                    French = "Chant rapide",
                    German = "Beschwingt",
                    Japanese = "スウィフトソング"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(134, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Swiftsong",
                    French = "Chant rapide",
                    German = "Beschwingt",
                    Japanese = "スウィフトソング：効果"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(135, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Mage's Ballad",
                    French = "Ballade du mage",
                    German = "Ballade des Weisen",
                    Japanese = "賢人のバラード"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(136, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Mage's Ballad",
                    French = "Ballade du mage",
                    German = "Ballade des Weisen",
                    Japanese = "賢人のバラード：効果"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(137, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Army's Paeon",
                    French = "Péan martial",
                    German = "Hymne der Krieger",
                    Japanese = "軍神のパイオン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(138, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Army's Paeon",
                    French = "Péan martial",
                    German = "Hymne der Krieger",
                    Japanese = "軍神のパイオン：効果"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(139, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Foe Requiem",
                    French = "Requiem ennemi",
                    German = "Requiem der Feinde",
                    Japanese = "魔人のレクイエム"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(140, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Foe Requiem",
                    French = "Requiem ennemi",
                    German = "Requiem der Feinde",
                    Japanese = "魔人のレクイエム：効果"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(141, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Battle Voice",
                    French = "Voix de combat",
                    German = "Ode an die Seele",
                    Japanese = "バトルボイス"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(142, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Chameleon",
                    French = "Caméléon",
                    German = "Chamäleon",
                    Japanese = "カメレオン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(143, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Aero",
                    French = "Vent",
                    German = "Wind",
                    Japanese = "エアロ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(144, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Aero II",
                    French = "Extra Vent",
                    German = "Windra",
                    Japanese = "エアロラ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(145, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Cleric Stance",
                    French = "Prestance du prêtre",
                    German = "Bußprediger",
                    Japanese = "クルセードスタンス"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(146, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Protect",
                    French = "Bouclier",
                    German = "Protes",
                    Japanese = "プロテス"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(147, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Protect",
                    French = "Bouclier",
                    German = "Protes",
                    Japanese = "プロテス"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(148, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Raise",
                    French = "Vie",
                    German = "Wiederbeleben",
                    Japanese = "蘇生"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(149, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Rebirth",
                    French = "Étourdissement",
                    German = "Schutzengel",
                    Japanese = "スタン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(150, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Medica II",
                    French = "Extra Médica",
                    German = "Resedra",
                    Japanese = "メディカラ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(151, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Stoneskin",
                    French = "Cuirasse",
                    German = "Steinhaut",
                    Japanese = "ストンスキン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(152, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "ストンスキン（物理攻撃）",
                    French = "ストンスキン（物理攻撃）",
                    German = "Steinhaut (physisch)",
                    Japanese = "ストンスキン（物理攻撃）"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(153, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "ストンスキン（魔法攻撃）",
                    French = "ストンスキン（魔法攻撃）",
                    German = "Steinhaut (magisch)",
                    Japanese = "ストンスキン（魔法攻撃）"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(154, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Shroud of Saints",
                    French = "Voile des saints",
                    German = "Fispelstimme",
                    Japanese = "女神の加護"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(155, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Freecure",
                    French = "Extra Soin amélioré",
                    German = "Vitra +",
                    Japanese = "ケアルラ効果アップ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(156, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Overcure",
                    French = "Méga Soin amélioré",
                    German = "Vitaga +",
                    Japanese = "ケアルガ効果アップ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(157, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Presence of Mind",
                    French = "Présence d'esprit",
                    German = "Geistesgegenwart",
                    Japanese = "神速魔"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(158, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Regen",
                    French = "Récup",
                    German = "Regena",
                    Japanese = "リジェネ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(159, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Divine Seal",
                    French = "Sceau divin",
                    German = "Barmherzigkeit",
                    Japanese = "ディヴァインシール"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(160, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Surecast",
                    French = "Stoïcisme",
                    German = "Unbeirrbarkeit",
                    Japanese = "堅実魔"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(161, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Thunder",
                    French = "Foudre",
                    German = "Blitz",
                    Japanese = "サンダー"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(162, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Thunder II",
                    French = "Extra Foudre",
                    German = "Blitzra",
                    Japanese = "サンダラ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(163, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Thunder III",
                    French = "Méga Foudre",
                    German = "Blitzga",
                    Japanese = "サンダガ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(164, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Thundercloud",
                    French = "Nuage d'orage",
                    German = "Blitz +",
                    Japanese = "サンダー系魔法効果アップ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(165, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Firestarter",
                    French = "Pyromane",
                    German = "Feuga +",
                    Japanese = "ファイガ効果アップ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(166, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Succor",
                    French = "Dogme de survie",
                    German = "Kurieren +",
                    Japanese = "士気高揚の策効果アップ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(167, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Swiftcast",
                    French = "Magie prompte",
                    German = "Spontaneität",
                    Japanese = "迅速魔"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(168, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Manaward",
                    French = "Barrière de mana",
                    German = "Mana-Schild",
                    Japanese = "マバリア"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(169, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Manawall",
                    French = "Mur de mana",
                    German = "Mana-Wand",
                    Japanese = "ウォール"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(170, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Apocatastasis",
                    French = "Apocatastase",
                    German = "Apokatastasis",
                    Japanese = "アポカタスタシス"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(171, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Ekpyrosis",
                    French = "Ekpyrosis",
                    German = "Ekpyrosis",
                    Japanese = "アポカタスタシス不可"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(172, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Infirmity",
                    French = "Infirmité",
                    German = "Gebrechlichkeit",
                    Japanese = "虚弱"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(173, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Astral Fire",
                    French = "Feu astral",
                    German = "Lichtfeuer",
                    Japanese = "アストラルファイア"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(174, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Astral Fire II",
                    French = "Feu astral II",
                    German = "Lichtfeuer II",
                    Japanese = "アストラルファイアII"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(175, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Astral Fire III",
                    French = "Feu astral III",
                    German = "Lichtfeuer III",
                    Japanese = "アストラルファイアIII"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(176, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Umbral Ice",
                    French = "Glace ombrale",
                    German = "Schatteneis",
                    Japanese = "アンブラルブリザード"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(177, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Umbral Ice II",
                    French = "Glace ombrale II",
                    German = "Schatteneis II",
                    Japanese = "アンブラルブリザードII"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(178, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Umbral Ice III",
                    French = "Glace ombrale III",
                    German = "Schatteneis III",
                    Japanese = "アンブラルブリザードIII"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(179, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Bio",
                    French = "Bactérie",
                    German = "Bio",
                    Japanese = "バイオ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(180, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Miasma",
                    French = "Miasmes",
                    German = "Miasma",
                    Japanese = "ミアズマ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(181, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Disease",
                    French = "Maladie",
                    German = "Krankheit",
                    Japanese = "病気"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(182, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Virus",
                    French = "Virus",
                    German = "Virus",
                    Japanese = "ウイルス"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(183, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Fever",
                    French = "Virus de l'esprit",
                    German = "Geistesvirus",
                    Japanese = "マインドウイルス"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(184, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Sustain",
                    French = "Transfusion",
                    German = "Erhaltung",
                    Japanese = "サステイン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(185, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Eye for an Eye",
                    French = "Garde-corps",
                    German = "Auge um Auge",
                    Japanese = "アイ・フォー・アイ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(186, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Eye for an Eye",
                    French = "Garde-corps",
                    German = "Auge um Auge",
                    Japanese = "アイ・フォー・アイ：効果"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(187, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Rouse",
                    French = "Stimulation",
                    German = "Aufmuntern",
                    Japanese = "ラウズ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(188, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Miasma II",
                    French = "Extra Miasmes",
                    German = "Miasra",
                    Japanese = "ミアズラ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(189, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Bio II",
                    French = "Extra Bactérie",
                    German = "Biora",
                    Japanese = "バイオラ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(190, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Shadow Flare",
                    French = "Éruption ténébreuse",
                    German = "Schattenflamme",
                    Japanese = "シャドウフレア"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(191, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Tri-disaster",
                    French = "Tri-désastre",
                    German = "Trisaster",
                    Japanese = "トライディザスター"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(192, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Spur",
                    French = "Encouragement",
                    German = "Ansporn",
                    Japanese = "スパー"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(193, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Slow",
                    French = "Lenteur",
                    German = "Gemach",
                    Japanese = "スロウ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(194, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Shield Wall",
                    French = "Mur protecteur",
                    German = "Schutzschild",
                    Japanese = "シールドウォール"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(195, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Mighty Guard",
                    French = "Garde puissante",
                    German = "Totalabwehr",
                    Japanese = "マイティガード"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(196, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Last Bastion",
                    French = "Dernier bastion",
                    German = "Letzte Bastion",
                    Japanese = "ラストバスティオン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(197, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Blaze Spikes",
                    French = "Pointes de feu",
                    German = "Feuerstachel",
                    Japanese = "ブレイズスパイク"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(198, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Ice Spikes",
                    French = "Pointes de glace",
                    German = "Eisstachel",
                    Japanese = "アイススパイク"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(199, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Shock Spikes",
                    French = "Pointes de foudre",
                    German = "Schockstachel",
                    Japanese = "ショックスパイク"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(200, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Physical Vulnerability Up",
                    French = "Vulnérabilité physique augmentée",
                    German = "Erhöhte physische Verwundbarkeit",
                    Japanese = "被物理ダメージ増加"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(201, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Stun",
                    French = "Étourdissement",
                    German = "Betäubung",
                    Japanese = "スタン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(202, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Vulnerability Up",
                    French = "Vulnérabilité augmentée",
                    German = "Erhöhte Verwundbarkeit",
                    Japanese = "被ダメージ上昇"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(203, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Boost",
                    French = "Accumulation",
                    German = "Akkumulation",
                    Japanese = "力溜め"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(204, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Enfire",
                    French = "EndoFeu",
                    German = "Runenwaffe: Feuer",
                    Japanese = "魔法剣・火"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(205, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Enblizzard",
                    French = "EndoGlace",
                    German = "Runenwaffe: Eis",
                    Japanese = "魔法剣・氷"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(206, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Enaero",
                    French = "EndoVent",
                    German = "Runenwaffe: Wind",
                    Japanese = "魔法剣・風"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(207, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Enstone",
                    French = "EndoPierre",
                    German = "Runenwaffe: Erde",
                    Japanese = "魔法剣・土"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(208, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Enthunder",
                    French = "EndoFoudre",
                    German = "Runenwaffe: Blitz",
                    Japanese = "魔法剣・雷"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(209, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Enwater",
                    French = "EndoEau",
                    German = "Runenwaffe: Wasser",
                    Japanese = "魔法剣・水"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(210, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Doom",
                    French = "Glas",
                    German = "Todesurteil",
                    Japanese = "死の宣告"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(211, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Sharpened Knife",
                    French = "Couteau aiguisé",
                    German = "Gewetztes Messer",
                    Japanese = "研がれた包丁"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(212, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "True Sight",
                    French = "Vision véritable",
                    German = "Wahre Gestalt",
                    Japanese = "見破り"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(213, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Pacification",
                    French = "Pacification",
                    German = "Besänftigung",
                    Japanese = "懐柔状態"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(214, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Agitation",
                    French = "Énervement",
                    German = "Aufstachelung",
                    Japanese = "懐柔失敗"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(215, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Determination Down",
                    French = "Malus de dégâts",
                    German = "Schaden -",
                    Japanese = "ダメージ低下"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(216, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Paralysis",
                    French = "Paralysie",
                    German = "Paralyse",
                    Japanese = "麻痺"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(217, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Triangulate",
                    French = "Forestier",
                    German = "Geodäsie",
                    Japanese = "トライアングレート"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(218, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Gathering Rate Up",
                    French = "Récolte améliorée",
                    German = "Sammelrate erhöht",
                    Japanese = "採集獲得率アップ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(219, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Gathering Yield Up",
                    French = "Récolte abondante",
                    German = "Sammelgewinn erhöht",
                    Japanese = "採集獲得数アップ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(220, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Gathering Fortune Up",
                    French = "Récolte de qualité",
                    German = "Sammelglück erhöht",
                    Japanese = "採集HQ獲得率アップ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(221, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Truth of Forests",
                    French = "Science des végétaux",
                    German = "Flurenthüllung",
                    Japanese = "トゥルー・オブ・フォレスト"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(222, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Truth of Mountains",
                    French = "Science des minéraux",
                    German = "Tellurische Enthüllung",
                    Japanese = "トゥルー・オブ・ミネラル"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(223, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Byregot's Ward",
                    French = "Grâce de Byregot",
                    German = "Byregots Segen",
                    Japanese = "ビエルゴの加護"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(224, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Nophica's Ward",
                    French = "Grâce de Nophica",
                    German = "Nophicas Segen",
                    Japanese = "ノフィカの加護"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(225, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Prospect",
                    French = "Prospecteur",
                    German = "Prospektion",
                    Japanese = "プロスペクト"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(226, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Haste",
                    French = "Hâte",
                    German = "Hast",
                    Japanese = "ヘイスト"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(228, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Menphina's Ward",
                    French = "Grâce de Menphina",
                    German = "Menphinas Segen",
                    Japanese = "メネフィナの加護"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(229, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Nald'thal's Ward",
                    French = "Grâce de Nald'thal",
                    German = "Nald'thals Segen",
                    Japanese = "ナルザルの加護"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(230, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Llymlaen's Ward",
                    French = "Grâce de Llymlaen",
                    German = "Llymlaens Segen",
                    Japanese = "リムレーンの加護"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(231, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Thaliak's Ward",
                    French = "Grâce de Thaliak",
                    German = "Thaliaks Segen",
                    Japanese = "サリャクの加護"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(232, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Preparation",
                    French = "Préparation",
                    German = "Vorausplanung",
                    Japanese = "プレパレーション"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(233, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Arbor Call",
                    French = "Dendrologie",
                    German = "Ruf des Waldes",
                    Japanese = "アーバーコール"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(234, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Lay of the Land",
                    French = "Géologie",
                    German = "Bodenbefund",
                    Japanese = "ランドサーベイ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(236, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Choco Beak",
                    French = "Choco-bec",
                    German = "Chocobo-Schnabel",
                    Japanese = "チョコビーク"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(237, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Choco Regen",
                    French = "Choco-récup",
                    German = "Chocobo-Regena",
                    Japanese = "チョコリジェネ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(238, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Choco Surge",
                    French = "Choco-ardeur",
                    German = "Chocobo-Quelle",
                    Japanese = "チョコサージ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(239, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "The Echo",
                    French = "L'Écho",
                    German = "Kraft des Transzendierens",
                    Japanese = "超える力"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(241, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Blessing of Light",
                    French = "Bénédiction de la Lumière",
                    German = "Gnade des Lichts",
                    Japanese = "光の加護"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(242, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Arbor Call II",
                    French = "Dendrologie II",
                    German = "Ruf des Waldes II",
                    Japanese = "アーバーコールII"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(243, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Lay of the Land II",
                    French = "Géologie II",
                    German = "Bodenbefund II",
                    Japanese = "ランドサーベイII"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(244, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Fracture",
                    French = "Fracture",
                    German = "Knochenbrecher",
                    Japanese = "フラクチャー"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(245, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Sanction",
                    French = "Sanction",
                    German = "Ermächtigung",
                    Japanese = "サンクション"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(246, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Demolish",
                    French = "Démolition",
                    German = "Demolieren",
                    Japanese = "破砕拳"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(247, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Rain of Death",
                    French = "Pluie mortelle",
                    German = "Tödlicher Regen",
                    Japanese = "レイン・オブ・デス"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(248, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Circle of Scorn",
                    French = "Cercle du destin",
                    German = "Kreis der Verachtung",
                    Japanese = "サークル・オブ・ドゥーム"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(249, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Flaming Arrow",
                    French = "Flèche enflammée",
                    German = "Flammenpfeil",
                    Japanese = "フレイミングアロー"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(250, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Burns",
                    French = "Brûlure",
                    German = "Brandwunde",
                    Japanese = "火傷"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(251, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Inner Quiet",
                    French = "Calme intérieur",
                    German = "Innere Ruhe",
                    Japanese = "インナークワイエット"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(252, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Waste Not",
                    French = "Parcimonie",
                    German = "Nachhaltigkeit",
                    Japanese = "倹約"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(253, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Steady Hand",
                    French = "Main sûre",
                    German = "Ruhige Hand",
                    Japanese = "ステディハンド"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(254, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Great Strides",
                    French = "Grands progrès",
                    German = "Große Schritte",
                    Japanese = "グレートストライド"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(255, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Ingenuity",
                    French = "Ingéniosité",
                    German = "Einfallsreichtum",
                    Japanese = "工面算段"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(256, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Ingenuity II",
                    French = "Ingéniosité II",
                    German = "Einfallsreichtum II",
                    Japanese = "工面算段II"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(257, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Waste Not II",
                    French = "Parcimonie II",
                    German = "Nachhaltigkeit II",
                    Japanese = "倹約II"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(258, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Manipulation",
                    French = "Manipulation",
                    German = "Manipulation",
                    Japanese = "マニピュレーション"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(259, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Innovation",
                    French = "Innovation",
                    German = "Innovation",
                    Japanese = "イノベーション"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(260, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Reclaim",
                    French = "Récupération",
                    German = "Reklamation",
                    Japanese = "リクレイム"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(261, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Comfort Zone",
                    French = "Zone de confort",
                    German = "Komfortzone",
                    Japanese = "コンファートゾーン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(262, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Steady Hand II",
                    French = "Main sûre II",
                    German = "Ruhige Hand II",
                    Japanese = "ステディハンドII"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(263, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Vulnerability Down",
                    French = "Vulnérabilité diminuée",
                    German = "Verringerte Verwundbarkeit",
                    Japanese = "ダメージ上昇"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(264, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Flesh Wound",
                    French = "Blessure physique",
                    German = "Fleischwunde",
                    Japanese = "切傷"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(265, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Stab Wound",
                    French = "Perforation",
                    German = "Stichwunde",
                    Japanese = "刺傷"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(266, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Concussion",
                    French = "Concussion",
                    German = "Prellung",
                    Japanese = "打撲傷"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(267, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Burns",
                    French = "Brûlure",
                    German = "Brandwunde",
                    Japanese = "火傷"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(268, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Frostbite",
                    French = "Gelure",
                    German = "Erfrierung",
                    Japanese = "凍傷"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(269, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Windburn",
                    French = "Brûlure du vent",
                    German = "Beißender Wind",
                    Japanese = "裂傷"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(270, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Sludge",
                    French = "Emboué",
                    German = "Schlamm",
                    Japanese = "汚泥"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(271, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Electrocution",
                    French = "Électrocution",
                    German = "Stromschlag",
                    Japanese = "感電"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(272, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Dropsy",
                    French = "Œdème",
                    German = "Wassersucht",
                    Japanese = "水毒"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(273, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Bleeding",
                    French = "Saignant",
                    German = "Blutung",
                    Japanese = "ペイン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(274, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Recuperation",
                    French = "Récupération",
                    German = "Segnung",
                    Japanese = "治癒"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(275, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Poison +1",
                    French = "Poison",
                    German = "Gift +1",
                    Japanese = "毒"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(276, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Voice of Valor",
                    French = "Voix de la valeur",
                    German = "Lob des Kämpen",
                    Japanese = "勇戦の誉れ：効果"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(277, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "堅忍の誉れ：効果",
                    French = "堅忍の誉れ：効果",
                    German = "堅忍の誉れ：効果",
                    Japanese = "堅忍の誉れ：効果"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(279, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Rehabilitation",
                    French = "Recouvrement",
                    German = "Rehabilitation",
                    Japanese = "徐々にHP回復"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(280, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Bind",
                    French = "Entrave",
                    German = "Fessel",
                    Japanese = "バインド"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(281, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Physical Damage Down",
                    French = "Malus de dégâts physiques",
                    German = "Schadenswert -",
                    Japanese = "物理ダメージ低下"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(282, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Mana Modulation",
                    French = "Anormalité magique",
                    German = "Magieschaden -",
                    Japanese = "魔力変調"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(283, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Dropsy",
                    French = "Œdème",
                    German = "Wassersucht",
                    Japanese = "水毒"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(284, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Burns",
                    French = "Brûlure",
                    German = "Brandwunde",
                    Japanese = "火傷"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(285, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Frostbite",
                    French = "Gelure",
                    German = "Erfrierung",
                    Japanese = "凍傷"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(286, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Windburn",
                    French = "Brûlure du vent",
                    German = "Beißender Wind",
                    Japanese = "裂傷"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(287, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Sludge",
                    French = "Emboué",
                    German = "Schlamm",
                    Japanese = "汚泥"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(288, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Electrocution",
                    French = "Électrocution",
                    German = "Stromschlag",
                    Japanese = "感電"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(289, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Dropsy",
                    French = "Œdème",
                    German = "Wassersucht",
                    Japanese = "水毒"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(290, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Determination Up",
                    French = "Bonus de dégâts",
                    German = "Schaden +",
                    Japanese = "ダメージ上昇"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(291, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Hundred Fists",
                    French = "Cent poings",
                    German = "100 Fäuste",
                    Japanese = "百烈拳"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(292, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Fetters",
                    French = "Attache",
                    German = "Granitgefängnis",
                    Japanese = "拘束"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(293, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Skill Speed Up",
                    French = "Bonus de vivacité",
                    German = "Schnelligkeit +",
                    Japanese = "スキルスピード上昇"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(294, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Spell Speed Up",
                    French = "Bonus de célérité",
                    German = "Zaubertempo +",
                    Japanese = "スペルスピード上昇"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(295, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Goldbile",
                    French = "Eau bilieuse",
                    German = "Goldlunge",
                    Japanese = "黄毒沼"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(296, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Hysteria",
                    French = "Hystérie",
                    German = "Panik",
                    Japanese = "恐慌"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(297, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Adloquium",
                    French = "Traité du réconfort",
                    German = "Adloquium",
                    Japanese = "鼓舞"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(298, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Sacred Soil",
                    French = "Dogme de survie",
                    German = "Geweihte Erde",
                    Japanese = "野戦治療の陣"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(299, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Sacred Soil",
                    French = "Dogme de survie",
                    German = "Geweihte Erde",
                    Japanese = "野戦治療の陣：効果"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(300, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Determination Up",
                    French = "Dégâts augmentés",
                    German = "Schaden +",
                    Japanese = "ダメージ上昇"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(301, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Critical Strikes",
                    French = "Coups critiques",
                    German = "Kritische Attacke",
                    Japanese = "クリティカル攻撃"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(302, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Gold Lung",
                    French = "Poumons bilieux",
                    German = "Galle",
                    Japanese = "黄毒"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(303, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Burrs",
                    French = "Bardanes",
                    German = "Klettenpilz",
                    Japanese = "粘菌"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(304, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Aetherflow",
                    French = "Flux d'éther",
                    German = "Ätherfluss",
                    Japanese = "エーテルフロー"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(305, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "The Dragon's Curse",
                    French = "Malédiction du dragon",
                    German = "Bann des Drachen",
                    Japanese = "竜の呪縛"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(306, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Inner Dragon",
                    French = "Dragon intérieur",
                    German = "Kraft des Drachen",
                    Japanese = "竜の力"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(307, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Voice of Valor",
                    French = "Voix de la valeur",
                    German = "Lob des Kämpen",
                    Japanese = "勇戦の誉れ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(308, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "堅忍の誉れ",
                    French = "堅忍の誉れ",
                    German = "堅忍の誉れ",
                    Japanese = "堅忍の誉れ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(310, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Curl",
                    French = "Pelotonnement",
                    German = "Einrollen",
                    Japanese = "かたまり"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(311, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Earthen Ward",
                    French = "Barrière terrestre",
                    German = "Erdengewahrsam",
                    Japanese = "大地の守り"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(312, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Earthen Fury",
                    French = "Fureur tellurique",
                    German = "Gaias Zorn",
                    Japanese = "大地の怒り"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(313, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Radiant Shield",
                    French = "Bouclier radiant",
                    German = "Glühender Schild",
                    Japanese = "光輝の盾"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(314, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Inferno",
                    French = "Flammes de l'enfer",
                    German = "Inferno",
                    Japanese = "地獄の火炎"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(315, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Whispering Dawn",
                    French = "Murmure de l'aurore",
                    German = "Erhebendes Flüstern",
                    Japanese = "光の囁き"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(316, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Fey Covenant",
                    French = "Alliance féérique",
                    German = "Feenverheißung",
                    Japanese = "フェイコヴナント"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(317, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Fey Illumination",
                    French = "Illumination féérique",
                    German = "Illumination",
                    Japanese = "フェイイルミネーション"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(318, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Fey Glow",
                    French = "Lueur féérique",
                    German = "Sprühender Glanz",
                    Japanese = "フェイグロウ"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(319, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Fey Light",
                    French = "Lumière féérique",
                    German = "Feenlicht",
                    Japanese = "フェイライト"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(320, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Bleeding",
                    French = "Saignant",
                    German = "Blutung",
                    Japanese = "ペイン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(321, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Gungnir",
                    French = "Gungnir",
                    German = "Gugnir",
                    Japanese = "グングニルの槍"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(322, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Crystal Veil",
                    French = "Œil sur ça",
                    German = "Kristallschleier",
                    Japanese = "クリスタルヴェール"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(323, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Reduced Immunity",
                    French = "Immunité réduite",
                    German = "Schwache Immunabwehr",
                    Japanese = "免疫低下"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(324, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Greenwrath",
                    French = "Ire de la forêt",
                    German = "Sintmal",
                    Japanese = "森の悲憤"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(325, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Invincibility",
                    French = "Invulnérable",
                    German = "Unverwundbar",
                    Japanese = "無敵"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(326, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Lightning Charge",
                    French = "Charge électrique",
                    German = "Statische Ladung",
                    Japanese = "帯電"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(327, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Ice Charge",
                    French = "Charge glacée",
                    German = "Eisige Ladung",
                    Japanese = "帯氷"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(328, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Heart of the Mountain",
                    French = "Cœur de la montagne",
                    German = "Herz des Felsgotts",
                    Japanese = "岩神の心石"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(329, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Modification",
                    French = "Récupération robotique",
                    German = "Fortifikationsprogramm 1",
                    Japanese = "自己強化プログラム"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(330, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Haste",
                    French = "Hâte",
                    German = "Hast",
                    Japanese = "ヘイスト"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(331, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Magic Vulnerability Down",
                    French = "Vulnérabilité magique diminuée",
                    German = "Verringerte Magie-Verwundbarkeit",
                    Japanese = "被魔法ダメージ軽減"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(332, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Physical Damage Up",
                    French = "Bonus de dégâts",
                    German = "Schaden +",
                    Japanese = "ダメージ上昇"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(333, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Allagan Rot",
                    French = "Pourriture allagoise",
                    German = "Allagische Fäulnis",
                    Japanese = "アラガンロット"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(334, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Allagan Immunity",
                    French = "Anticorps allagois",
                    German = "Allagische Immunität",
                    Japanese = "アラガンロット抗体"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(335, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Firestream",
                    French = "Courants de feu",
                    German = "Feuerstrahlen",
                    Japanese = "ファイアストリーム"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(336, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Sequence AB1",
                    French = "Séquence AB1",
                    German = "Sequenz AB1",
                    Japanese = "対打撃プログラム"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(337, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Sequence AP1",
                    French = "Séquence AP1",
                    German = "Sequenz AP1",
                    Japanese = "対突撃プログラム"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(338, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Sequence AS1",
                    French = "Séquence AS1",
                    German = "Sequenz AS1",
                    Japanese = "対斬撃プログラム"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(339, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Bleeding",
                    French = "Saignant",
                    German = "Blutung",
                    Japanese = "ペイン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(340, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Physical Field",
                    French = "Champ physique",
                    German = "Physisches Feld",
                    Japanese = "対物理障壁"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(341, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Aetherial Field",
                    French = "Champ éthéré",
                    German = "Magisches Feld",
                    Japanese = "対魔法障壁"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(342, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Repelling Spray",
                    French = "Réplique",
                    German = "Reflektorschild",
                    Japanese = "応射"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(343, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Bleeding",
                    French = "Saignant",
                    German = "Blutung",
                    Japanese = "ペイン"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(344, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Neurolink",
                    French = "Neurolien",
                    German = "Neurolink",
                    Japanese = "拘束装置"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(345, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Recharge",
                    French = "Recharge",
                    German = "Aufladung",
                    Japanese = "魔力供給"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(346, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Waxen Flesh",
                    French = "Chair fondue",
                    German = "Wächserne Haut",
                    Japanese = "帯炎"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(347, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Pox",
                    French = "Vérole",
                    German = "Pocken",
                    Japanese = "ポックス"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(348, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Disseminate",
                    French = "Dissémination",
                    German = "Aussäen",
                    Japanese = "ディスセミネイト"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(349, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Steel Scales",
                    French = "Écailles d'acier",
                    German = "Stahlschuppen",
                    Japanese = "スチールスケール"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(350, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Vulnerability Down",
                    French = "Vulnérabilité diminuée",
                    German = "Verringerte Verwundbarkeit",
                    Japanese = "被ダメージ低下"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(351, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Rancor",
                    French = "Rancune",
                    German = "Groll",
                    Japanese = "怨み"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(352, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Spjot",
                    French = "Spjot",
                    German = "Gugnirs Zauber",
                    Japanese = "グングニルの魔力"
                },
                CompanyAction = false,
            });
            StatusEffects.Add(353, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Brave New World",
                    French = "Un nouveau monde",
                    German = "Startbonus",
                    Japanese = "カンパニーアクション：ビギナーボーナス"
                },
                CompanyAction = true,
            });
            StatusEffects.Add(354, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Live off the Land",
                    French = "Vivre de la terre",
                    German = "Sammelgeschick-Bonus",
                    Japanese = "カンパニーアクション：獲得力アップ"
                },
                CompanyAction = true,
            });
            StatusEffects.Add(355, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "What You See",
                    French = "Avoir le coup d'œil",
                    German = "Wahrnehmungsbonus",
                    Japanese = "カンパニーアクション：識質力アップ"
                },
                CompanyAction = true,
            });
            StatusEffects.Add(356, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Eat from the Hand",
                    French = "La main qui nourrit",
                    German = "Kunstfertigkeits-Bonus",
                    Japanese = "カンパニーアクション：作業精度アップ"
                },
                CompanyAction = true,
            });
            StatusEffects.Add(357, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "In Control",
                    French = "Passer maître",
                    German = "Kontrolle-Bonus",
                    Japanese = "カンパニーアクション：加工精度アップ"
                },
                CompanyAction = true,
            });
            StatusEffects.Add(360, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Meat and Mead",
                    French = "À boire et à manger",
                    German = "Verlängerte Nahrungseffekte",
                    Japanese = "カンパニーアクション：食事効果時間延長"
                },
                CompanyAction = true,
            });
            StatusEffects.Add(361, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "That Which Binds Us",
                    French = "Union parfaite",
                    German = "Bindungsbonus",
                    Japanese = "カンパニーアクション：錬精度上昇値アップ"
                },
                CompanyAction = true,
            });
            StatusEffects.Add(362, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Proper Care",
                    French = "Protections protégées",
                    German = "Verminderter Verschleiß",
                    Japanese = "カンパニーアクション：装備品劣化低減"
                },
                CompanyAction = true,
            });
            StatusEffects.Add(363, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Back on Your Feet",
                    French = "Prompt rétablissement",
                    German = "Verkürzte Schwäche",
                    Japanese = "カンパニーアクション：衰弱時間短縮"
                },
                CompanyAction = true,
            });
            StatusEffects.Add(364, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Reduced Rates",
                    French = "Prix d'ami",
                    German = "Teleport-Vergünstigung",
                    Japanese = "カンパニーアクション：テレポ割引"
                },
                CompanyAction = true,
            });
            StatusEffects.Add(365, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "The Heat of Battle",
                    French = "Feu du combat",
                    German = "Kampfroutine-Bonus",
                    Japanese = "カンパニーアクション：討伐経験値アップ"
                },
                CompanyAction = true,
            });
            StatusEffects.Add(366, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "A Man's Best Friend",
                    French = "Meilleur ami de l'homme",
                    German = "Mitstreiterroutine-Bonus",
                    Japanese = "カンパニーアクション：バディ経験値アップ"
                },
                CompanyAction = true,
            });
            StatusEffects.Add(367, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Earth and Water",
                    French = "Terre et eau",
                    German = "Sammelroutine-Bonus",
                    Japanese = "カンパニーアクション：採集経験値アップ"
                },
                CompanyAction = true,
            });
            StatusEffects.Add(368, new StatusItem
            {
                Name = new StatusLocalization
                {
                    English = "Helping Hand",
                    French = "Être en bonnes mains",
                    German = "Syntheseroutine-Bonus",
                    Japanese = "カンパニーアクション：製作経験値アップ"
                },
                CompanyAction = true,
            });
        }
    }
}
