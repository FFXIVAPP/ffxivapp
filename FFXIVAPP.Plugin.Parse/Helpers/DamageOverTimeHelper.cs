// FFXIVAPP.Plugin.Parse
// DamageOverTimeHelper.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Collections.Generic;
using FFXIVAPP.Plugin.Parse.Models;

#endregion

namespace FFXIVAPP.Plugin.Parse.Helpers
{
    public static class DamageOverTimeHelper
    {
        public static readonly List<string> Thunders = new List<string>
        {
            "thunder",
            "blitz",
            "foudre",
            "サンダ"
        };

        private static Dictionary<string, DamageOverTimeAction> _playerActions;
        private static Dictionary<string, DamageOverTimeAction> _monsterActions;

        public static Dictionary<string, DamageOverTimeAction> PlayerActions
        {
            get
            {
                if (_playerActions != null)
                {
                    return _playerActions;
                }
                _playerActions = new Dictionary<string, DamageOverTimeAction>();

                _playerActions.Add("circle of scorn", new DamageOverTimeAction
                {
                    ActionPotency = 100,
                    DamageOverTimePotency = 30,
                    Duration = 15,
                    ZeroBaseDamageDOT = false
                });
                _playerActions.Add("kreis der verachtung", _playerActions["circle of scorn"]);
                _playerActions.Add("cercle du destin", _playerActions["circle of scorn"]);
                _playerActions.Add("サークル・オブ・ドゥーム", _playerActions["circle of scorn"]);

                _playerActions.Add("touch of death", new DamageOverTimeAction
                {
                    ActionPotency = 20,
                    DamageOverTimePotency = 25,
                    Duration = 30,
                    ZeroBaseDamageDOT = false
                });
                _playerActions.Add("hauch des todes", _playerActions["touch of death"]);
                _playerActions.Add("toucher mortel", _playerActions["touch of death"]);
                _playerActions.Add("秘孔拳", _playerActions["touch of death"]);

                _playerActions.Add("demolish", new DamageOverTimeAction
                {
                    ActionPotency = 100,
                    DamageOverTimePotency = 40,
                    Duration = 18,
                    ZeroBaseDamageDOT = true
                });
                _playerActions.Add("demolieren", _playerActions["demolish"]);
                _playerActions.Add("démolition", _playerActions["demolish"]);
                _playerActions.Add("破砕拳", _playerActions["demolish"]);

                _playerActions.Add("fracture", new DamageOverTimeAction
                {
                    ActionPotency = 100,
                    DamageOverTimePotency = 20,
                    Duration = 30,
                    ZeroBaseDamageDOT = false
                });
                _playerActions.Add("knochenbrecher", _playerActions["fracture"]);
                //_playerActions.Add("fracture", _playerActions["fracture"]);
                _playerActions.Add("フラクチャー", _playerActions["fracture"]);

                _playerActions.Add("phlebotomize", new DamageOverTimeAction
                {
                    ActionPotency = 170,
                    DamageOverTimePotency = 20,
                    Duration = 18,
                    ZeroBaseDamageDOT = false
                });
                _playerActions.Add("phlebotomie", _playerActions["phlebotomize"]);
                _playerActions.Add("double percée", _playerActions["phlebotomize"]);
                _playerActions.Add("二段突き", _playerActions["phlebotomize"]);

                _playerActions.Add("chaos thrust", new DamageOverTimeAction
                {
                    ActionPotency = 160,
                    DamageOverTimePotency = 20,
                    Duration = 30,
                    ZeroBaseDamageDOT = false
                });
                _playerActions.Add("chaotischer tjost", _playerActions["chaos thrust"]);
                _playerActions.Add("percée chaotique", _playerActions["chaos thrust"]);
                _playerActions.Add("桜華狂咲", _playerActions["chaos thrust"]);

                _playerActions.Add("venomous bite", new DamageOverTimeAction
                {
                    ActionPotency = 100,
                    DamageOverTimePotency = 35,
                    Duration = 18,
                    ZeroBaseDamageDOT = false
                });
                _playerActions.Add("giftbiss", _playerActions["venomous bite"]);
                _playerActions.Add("morsure venimeuse", _playerActions["venomous bite"]);
                _playerActions.Add("ベノムバイト", _playerActions["venomous bite"]);

                _playerActions.Add("windbite", new DamageOverTimeAction
                {
                    ActionPotency = 60,
                    DamageOverTimePotency = 45,
                    Duration = 18,
                    ZeroBaseDamageDOT = false
                });
                _playerActions.Add("beißender Wind", _playerActions["windbite"]);
                _playerActions.Add("morsure du vent", _playerActions["windbite"]);
                _playerActions.Add("ウィンドバイト", _playerActions["windbite"]);

                _playerActions.Add("aero", new DamageOverTimeAction
                {
                    ActionPotency = 50,
                    DamageOverTimePotency = 25,
                    Duration = 18,
                    ZeroBaseDamageDOT = false
                });
                _playerActions.Add("wind", _playerActions["aero"]);
                _playerActions.Add("vent", _playerActions["aero"]);
                _playerActions.Add("エアロ", _playerActions["aero"]);

                _playerActions.Add("aero ii", new DamageOverTimeAction
                {
                    ActionPotency = 50,
                    DamageOverTimePotency = 40,
                    Duration = 12,
                    ZeroBaseDamageDOT = false
                });
                _playerActions.Add("windra", _playerActions["aero ii"]);
                _playerActions.Add("extra vent", _playerActions["aero ii"]);
                _playerActions.Add("エアロラ", _playerActions["aero ii"]);

                _playerActions.Add("thunder", new DamageOverTimeAction
                {
                    ActionPotency = 30,
                    DamageOverTimePotency = 35,
                    Duration = 18,
                    ZeroBaseDamageDOT = false
                });
                _playerActions.Add("blitz", _playerActions["thunder"]);
                _playerActions.Add("foudre", _playerActions["thunder"]);
                _playerActions.Add("サンダー", _playerActions["thunder"]);

                _playerActions.Add("thunder ii", new DamageOverTimeAction
                {
                    ActionPotency = 50,
                    DamageOverTimePotency = 35,
                    Duration = 21,
                    ZeroBaseDamageDOT = false
                });
                _playerActions.Add("blitzra", _playerActions["thunder ii"]);
                _playerActions.Add("extra foudre", _playerActions["thunder ii"]);
                _playerActions.Add("サンダラ", _playerActions["thunder ii"]);

                _playerActions.Add("thunder iii", new DamageOverTimeAction
                {
                    ActionPotency = 60,
                    DamageOverTimePotency = 35,
                    Duration = 24,
                    ZeroBaseDamageDOT = false
                });
                _playerActions.Add("blitzga", _playerActions["thunder iii"]);
                _playerActions.Add("méga foudre", _playerActions["thunder iii"]);
                _playerActions.Add("サンダガ", _playerActions["thunder iii"]);

                _playerActions.Add("bio", new DamageOverTimeAction
                {
                    ActionPotency = 5,
                    DamageOverTimePotency = 40,
                    Duration = 18,
                    ZeroBaseDamageDOT = true
                });
                //_playerActions.Add("bio", _playerActions["bio"]);
                _playerActions.Add("bactérie", _playerActions["bio"]);
                _playerActions.Add("バイオ", _playerActions["bio"]);

                _playerActions.Add("bio ii", new DamageOverTimeAction
                {
                    ActionPotency = 5,
                    DamageOverTimePotency = 35,
                    Duration = 30,
                    ZeroBaseDamageDOT = true
                });
                _playerActions.Add("biora", _playerActions["bio ii"]);
                _playerActions.Add("extra bactérie", _playerActions["bio ii"]);
                _playerActions.Add("バイオラ", _playerActions["bio ii"]);

                _playerActions.Add("miasma", new DamageOverTimeAction
                {
                    ActionPotency = 20,
                    DamageOverTimePotency = 35,
                    Duration = 24,
                    ZeroBaseDamageDOT = false
                });
                //_playerActions.Add("miasma", _playerActions["miasma"]);
                _playerActions.Add("miasmes", _playerActions["miasma"]);
                _playerActions.Add("ミアズマ", _playerActions["miasma"]);

                _playerActions.Add("miasma ii", new DamageOverTimeAction
                {
                    ActionPotency = 20,
                    DamageOverTimePotency = 10,
                    Duration = 15,
                    ZeroBaseDamageDOT = false
                });
                //_playerActions.Add("miasma ii", _playerActions["miasma ii"]);
                _playerActions.Add("extra miasmes", _playerActions["miasma ii"]);
                _playerActions.Add("ミアズラ", _playerActions["miasma ii"]);
                return _playerActions;
            }
            set { _playerActions = value; }
        }

        public static Dictionary<string, DamageOverTimeAction> MonsterActions
        {
            get { return _monsterActions ?? (_monsterActions = new Dictionary<string, DamageOverTimeAction>()); }
            set { _monsterActions = value; }
        }
    }
}
