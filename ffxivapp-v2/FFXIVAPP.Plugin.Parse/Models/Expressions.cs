// FFXIVAPP.Plugin.Parse
// Expressions.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using FFXIVAPP.Plugin.Parse.Helpers;
using FFXIVAPP.Plugin.Parse.Models.Events;
using FFXIVAPP.Plugin.Parse.RegularExpressions;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models
{
    internal class Expressions : INotifyPropertyChanged
    {
        private Match _mDamage;
        private Match _mFailed;
        private Match _mActions;
        private Match _pDamage;
        private Match _pFailed;
        private Match _pActions;

        public Expressions(Event e, string cleaned)
        {
            Event = e;
            Cleaned = cleaned;
            Initialize();
        }

        public Event Event { get; set; }
        public string Cleaned { get; set; }
        public string Counter { get; private set; }
        public string Added { get; private set; }
        public string Type { get; private set; }
        public string RAttack { get; private set; }
        public string Attack { get; private set; }
        public string You { get; private set; }

        public Match mDamage
        {
            get { return _mDamage ?? (_mDamage = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _mDamage = value;
                RaisePropertyChanged();
            }
        }

        public Match mFailed
        {
            get { return _mFailed ?? (_mFailed = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _mFailed = value;
                RaisePropertyChanged();
            }
        }

        public Match mActions
        {
            get { return _mActions ?? (_mActions = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _mActions = value;
                RaisePropertyChanged();
            }
        }

        public Match pDamage
        {
            get { return _pDamage ?? (_pDamage = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _pDamage = value;
                RaisePropertyChanged();
            }
        }

        public Match pFailed
        {
            get { return _pFailed ?? (_pFailed = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _pFailed = value;
                RaisePropertyChanged();
            }
        }

        public Match pActions
        {
            get { return _pActions ?? (_pActions = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _pActions = value;
                RaisePropertyChanged();
            }
        }

        private void Initialize()
        {
            switch (Common.Constants.GameLanguage)
            {
                case "French":
                    //pDamage = PlayerRegEx.DamageFr.Match(Cleaned);
                    //pFailed = PlayerRegEx.FailedFr.Match(Cleaned);
                    //pActions = PlayerRegEx.ActionsFr.Match(Cleaned);
                    Counter = "Contre";
                    Added = "Effet Supplémentaire";
                    Type = "PV";
                    RAttack = "D'Attaque À Distance";
                    Attack = "Attaque";
                    You = @"^[Vv]ous$";
                    break;
                case "Japanese":
                    //pDamage = PlayerRegEx.DamageJa.Match(Cleaned);
                    //pFailed = PlayerRegEx.FailedJa.Match(Cleaned);
                    //pActions = PlayerRegEx.ActionsJa.Match(Cleaned);
                    Counter = "カウンター";
                    Added = "追加効果";
                    Type = "ＨＰ";
                    RAttack = "Ranged Attack";
                    Attack = "Attack";
                    You = @"^\.$";
                    break;
                case "German":
                    //pDamage = PlayerRegEx.DamageDe.Match(Cleaned);
                    //pFailed = PlayerRegEx.FailedDe.Match(Cleaned);
                    //pActions = PlayerRegEx.ActionsDe.Match(Cleaned);
                    Counter = "Counter";
                    Added = "Zusatzefeckt";
                    Type = "HP";
                    RAttack = "Ranged Attack";
                    Attack = "Attack";
                    You = @"^[Dd]u$";
                    break;
                default:
                    pDamage = PlayerRegEx.DamageEn.Match(Cleaned);
                    pFailed = PlayerRegEx.FailedEn.Match(Cleaned);
                    pActions = PlayerRegEx.ActionsEn.Match(Cleaned);
                    mDamage = MonsterRegEx.DamageEn.Match(Cleaned);
                    mFailed = MonsterRegEx.FailedEn.Match(Cleaned);
                    mActions = MonsterRegEx.ActionsEn.Match(Cleaned);
                    Counter = "Counter";
                    Added = "Additional Effect";
                    Type = "HP";
                    RAttack = "Ranged Attack";
                    Attack = "Attack";
                    You = @"^[Yy]our?$";
                    break;
            }
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
