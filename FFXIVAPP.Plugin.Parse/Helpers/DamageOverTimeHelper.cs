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

        public static readonly List<string> ZeroBaseDamageDOT = new List<string>
        {
            "demolish",
            "demolieren",
            "démolition",
            "破砕拳"
        };

        public static Dictionary<string, DamageOverTimeAction> PlayerActions()
        {
            var damageOverTimeActions = new Dictionary<string, DamageOverTimeAction>();

            damageOverTimeActions.Add("circle of scorn", new DamageOverTimeAction
            {
                ActionPotency = 30,
                DamageOverTimePotency = 30,
                Duration = 15
            });
            damageOverTimeActions.Add("kreis der verachtung", damageOverTimeActions["circle of scorn"]);
            damageOverTimeActions.Add("cercle du destin", damageOverTimeActions["circle of scorn"]);
            damageOverTimeActions.Add("サークル・オブ・ドゥーム", damageOverTimeActions["circle of scorn"]);

            damageOverTimeActions.Add("touch of death", new DamageOverTimeAction
            {
                ActionPotency = 20,
                DamageOverTimePotency = 25,
                Duration = 30
            });
            damageOverTimeActions.Add("hauch des todes", damageOverTimeActions["touch of death"]);
            damageOverTimeActions.Add("toucher mortel", damageOverTimeActions["touch of death"]);
            damageOverTimeActions.Add("秘孔拳", damageOverTimeActions["touch of death"]);

            damageOverTimeActions.Add("demolish", new DamageOverTimeAction
            {
                ActionPotency = 100,
                DamageOverTimePotency = 40,
                Duration = 18
            });
            damageOverTimeActions.Add("demolieren", damageOverTimeActions["demolish"]);
            damageOverTimeActions.Add("démolition", damageOverTimeActions["demolish"]);
            damageOverTimeActions.Add("破砕拳", damageOverTimeActions["demolish"]);

            damageOverTimeActions.Add("fracture", new DamageOverTimeAction
            {
                ActionPotency = 100,
                DamageOverTimePotency = 20,
                Duration = 30
            });
            damageOverTimeActions.Add("knochenbrecher", damageOverTimeActions["fracture"]);
            //damageOverTimeActions.Add("fracture", damageOverTimeActions["fracture"]);
            damageOverTimeActions.Add("フラクチャー", damageOverTimeActions["fracture"]);

            damageOverTimeActions.Add("phlebotomize", new DamageOverTimeAction
            {
                ActionPotency = 170,
                DamageOverTimePotency = 10,
                Duration = 18
            });
            damageOverTimeActions.Add("phlebotomie", damageOverTimeActions["phlebotomize"]);
            damageOverTimeActions.Add("double percée", damageOverTimeActions["phlebotomize"]);
            damageOverTimeActions.Add("二段突き", damageOverTimeActions["phlebotomize"]);

            damageOverTimeActions.Add("chaos thrust", new DamageOverTimeAction
            {
                ActionPotency = 160,
                DamageOverTimePotency = 20,
                Duration = 30
            });
            damageOverTimeActions.Add("chaotischer tjost", damageOverTimeActions["chaos thrust"]);
            damageOverTimeActions.Add("percée chaotique", damageOverTimeActions["chaos thrust"]);
            damageOverTimeActions.Add("桜華狂咲", damageOverTimeActions["chaos thrust"]);

            damageOverTimeActions.Add("venomous bite", new DamageOverTimeAction
            {
                ActionPotency = 100,
                DamageOverTimePotency = 35,
                Duration = 18
            });
            damageOverTimeActions.Add("giftbiss", damageOverTimeActions["venomous bite"]);
            damageOverTimeActions.Add("morsure venimeuse", damageOverTimeActions["venomous bite"]);
            damageOverTimeActions.Add("ベノムバイト", damageOverTimeActions["venomous bite"]);

            damageOverTimeActions.Add("windbite", new DamageOverTimeAction
            {
                ActionPotency = 60,
                DamageOverTimePotency = 40,
                Duration = 18
            });
            damageOverTimeActions.Add("beißender Wind", damageOverTimeActions["windbite"]);
            damageOverTimeActions.Add("morsure du vent", damageOverTimeActions["windbite"]);
            damageOverTimeActions.Add("ウィンドバイト", damageOverTimeActions["windbite"]);

            damageOverTimeActions.Add("aero", new DamageOverTimeAction
            {
                ActionPotency = 50,
                DamageOverTimePotency = 25,
                Duration = 18
            });
            damageOverTimeActions.Add("wind", damageOverTimeActions["aero"]);
            damageOverTimeActions.Add("vent", damageOverTimeActions["aero"]);
            damageOverTimeActions.Add("エアロ", damageOverTimeActions["aero"]);

            damageOverTimeActions.Add("aero ii", new DamageOverTimeAction
            {
                ActionPotency = 60,
                DamageOverTimePotency = 40,
                Duration = 12
            });
            damageOverTimeActions.Add("windra", damageOverTimeActions["aero ii"]);
            damageOverTimeActions.Add("extra vent", damageOverTimeActions["aero ii"]);
            damageOverTimeActions.Add("エアロラ", damageOverTimeActions["aero ii"]);

            damageOverTimeActions.Add("thunder", new DamageOverTimeAction
            {
                ActionPotency = 30,
                DamageOverTimePotency = 40,
                Duration = 12
            });
            damageOverTimeActions.Add("blitz", damageOverTimeActions["thunder"]);
            damageOverTimeActions.Add("foudre", damageOverTimeActions["thunder"]);
            damageOverTimeActions.Add("サンダー", damageOverTimeActions["thunder"]);

            damageOverTimeActions.Add("thunder ii", new DamageOverTimeAction
            {
                ActionPotency = 50,
                DamageOverTimePotency = 40,
                Duration = 15
            });
            damageOverTimeActions.Add("blitzra", damageOverTimeActions["thunder ii"]);
            damageOverTimeActions.Add("extra foudre", damageOverTimeActions["thunder ii"]);
            damageOverTimeActions.Add("サンダラ", damageOverTimeActions["thunder ii"]);

            damageOverTimeActions.Add("thunder iii", new DamageOverTimeAction
            {
                ActionPotency = 60,
                DamageOverTimePotency = 40,
                Duration = 18
            });
            damageOverTimeActions.Add("blitzga", damageOverTimeActions["thunder iii"]);
            damageOverTimeActions.Add("méga foudre", damageOverTimeActions["thunder iii"]);
            damageOverTimeActions.Add("サンダガ", damageOverTimeActions["thunder iii"]);

            return damageOverTimeActions;
        }

        public static Dictionary<string, DamageOverTimeAction> MonsterActions()
        {
            var damageOverTimeActions = new Dictionary<string, DamageOverTimeAction>();
            return damageOverTimeActions;
        }
    }
}
