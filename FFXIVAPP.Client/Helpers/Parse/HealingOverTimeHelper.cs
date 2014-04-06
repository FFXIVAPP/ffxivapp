// FFXIVAPP.Client
// HealingOverTimeHelper.cs
// 
// Copyright © 2007 - 2014 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System.Collections.Generic;
using FFXIVAPP.Client.Models.Parse;

namespace FFXIVAPP.Client.Helpers.Parse
{
    public static class HealingOverTimeHelper
    {
        private static Dictionary<string, XOverTimeAction> _playerActions;
        private static Dictionary<string, XOverTimeAction> _monsterActions;
        private static Dictionary<string, List<string>> _cureActions;
        private static Dictionary<string, List<string>> _medicaActions;

        public static Dictionary<string, List<string>> CureActions
        {
            get
            {
                if (_cureActions != null)
                {
                    return _cureActions;
                }
                _cureActions = new Dictionary<string, List<string>>();

                _cureActions.Add("I", new List<string>
                {
                    "cure",
                    "vita",
                    "soin",
                    "ケアル"
                });
                _cureActions.Add("II", new List<string>
                {
                    "cure ii",
                    "vitra",
                    "extra soin",
                    "ケアルラ"
                });
                _cureActions.Add("III", new List<string>
                {
                    "cure iii",
                    "vitaga",
                    "méga soin",
                    "ケアルガ"
                });
                return _cureActions;
            }
        }

        public static Dictionary<string, List<string>> MedicaActions
        {
            get
            {
                if (_medicaActions != null)
                {
                    return _medicaActions;
                }
                _medicaActions = new Dictionary<string, List<string>>();

                _medicaActions.Add("I", new List<string>
                {
                    "reseda",
                    "médica",
                    "メディカ"
                });
                _medicaActions.Add("II", new List<string>
                {
                    "medica ii",
                    "resedra",
                    "extra médica",
                    "メディカラ"
                });
                return _medicaActions;
            }
        }

        public static Dictionary<string, XOverTimeAction> PlayerActions
        {
            get
            {
                if (_playerActions != null)
                {
                    return _playerActions;
                }
                _playerActions = new Dictionary<string, XOverTimeAction>();

                _playerActions.Add("medica ii", new XOverTimeAction
                {
                    ActionPotency = 200,
                    ActionOverTimePotency = 50,
                    Duration = 30,
                    HasNoInitialResult = false
                });
                _playerActions.Add("resedra", _playerActions["medica ii"]);
                _playerActions.Add("extra médica", _playerActions["medica ii"]);
                _playerActions.Add("メディカラ", _playerActions["medica ii"]);

                _playerActions.Add("regen", new XOverTimeAction
                {
                    ActionPotency = 0,
                    ActionOverTimePotency = 150,
                    Duration = 21,
                    HasNoInitialResult = true
                });
                _playerActions.Add("regena", _playerActions["regen"]);
                _playerActions.Add("récup", _playerActions["regen"]);
                _playerActions.Add("リジェネ", _playerActions["regen"]);

                _playerActions.Add("chocobo regen", new XOverTimeAction
                {
                    ActionPotency = 0,
                    ActionOverTimePotency = 150,
                    Duration = 21,
                    HasNoInitialResult = true
                });
                _playerActions.Add("choco-regena", _playerActions["chocobo regen"]);
                _playerActions.Add("choco-récup", _playerActions["chocobo regen"]);
                _playerActions.Add("チョコリジェネ", _playerActions["chocobo regen"]);

                _playerActions.Add("whispering dawn", new XOverTimeAction
                {
                    ActionPotency = 0,
                    ActionOverTimePotency = 100,
                    Duration = 21,
                    HasNoInitialResult = true
                });
                _playerActions.Add("erhebendes flüstern", _playerActions["whispering dawn"]);
                _playerActions.Add("murmure de l'aurore", _playerActions["whispering dawn"]);
                _playerActions.Add("光の囁き", _playerActions["whispering dawn"]);

                return _playerActions;
            }
            set { _playerActions = value; }
        }

        public static Dictionary<string, XOverTimeAction> MonsterActions
        {
            get { return _monsterActions ?? (_monsterActions = new Dictionary<string, XOverTimeAction>()); }
            set { _monsterActions = value; }
        }
    }
}
