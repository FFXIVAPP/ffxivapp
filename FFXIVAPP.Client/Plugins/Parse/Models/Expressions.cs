// FFXIVAPP.Client
// Expressions.cs
// 
// © 2013 Ryan Wilson

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Plugins.Parse.Models.Events;
using FFXIVAPP.Client.RegularExpressions;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Plugins.Parse.Models
{
    [DoNotObfuscate]
    public class Expressions : INotifyPropertyChanged
    {
        private Match _mActions;
        private Match _mBeneficialGain;
        private Match _mBeneficialLose;
        private Match _mCure;
        private Match _mDamage;
        private Match _mDamageAuto;
        private Match _mDetrimentalGain;
        private Match _mDetrimentalLose;
        private Match _mFailed;
        private Match _mFailedAuto;
        private Match _mItems;
        private Match _pActions;
        private Match _pBeneficialGain;
        private Match _pBeneficialLose;
        private Match _pCure;
        private Match _pDamage;
        private Match _pDamageAuto;
        private Match _pDetrimentalGain;
        private Match _pDetrimentalLose;
        private Match _pFailed;
        private Match _pFailedAuto;
        private Match _pItems;

        public Expressions(Event e)
        {
            Event = e;
            Cleaned = e.ChatLogEntry == null ? "" : e.ChatLogEntry.Line;
            Initialize();
        }

        public Event Event { get; set; }
        public string Cleaned { get; set; }
        public string Counter { get; private set; }
        public string Added { get; private set; }
        public string HealingType { get; private set; }
        public string RAttack { get; private set; }
        public string Attack { get; private set; }
        public string You { get; private set; }
        public string YouString { get; private set; }
        public string Mitigated { get; private set; }

        #region Monster

        public Match mDamage
        {
            get { return _mDamage ?? (_mDamage = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _mDamage = value;
                RaisePropertyChanged();
            }
        }

        public Match mDamageAuto
        {
            get { return _mDamageAuto ?? (_mDamageAuto = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _mDamageAuto = value;
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

        public Match mFailedAuto
        {
            get { return _mFailedAuto ?? (_mFailedAuto = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _mFailedAuto = value;
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

        public Match mItems
        {
            get { return _mItems ?? (_mItems = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _mItems = value;
                RaisePropertyChanged();
            }
        }

        public Match mCure
        {
            get { return _mCure ?? (_mCure = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _mCure = value;
                RaisePropertyChanged();
            }
        }

        public Match mBeneficialGain
        {
            get { return _mBeneficialGain ?? (_mBeneficialGain = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _mBeneficialGain = value;
                RaisePropertyChanged();
            }
        }

        public Match mBeneficialLose
        {
            get { return _mBeneficialLose ?? (_mBeneficialLose = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _mBeneficialLose = value;
                RaisePropertyChanged();
            }
        }

        public Match mDetrimentalGain
        {
            get { return _mDetrimentalGain ?? (_mDetrimentalGain = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _mDetrimentalGain = value;
                RaisePropertyChanged();
            }
        }

        public Match mDetrimentalLose
        {
            get { return _mDetrimentalLose ?? (_mDetrimentalLose = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _mDetrimentalLose = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Player

        public Match pDamage
        {
            get { return _pDamage ?? (_pDamage = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _pDamage = value;
                RaisePropertyChanged();
            }
        }

        public Match pDamageAuto
        {
            get { return _pDamageAuto ?? (_pDamageAuto = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _pDamageAuto = value;
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

        public Match pFailedAuto
        {
            get { return _pFailedAuto ?? (_pFailedAuto = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _pFailedAuto = value;
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

        public Match pItems
        {
            get { return _pItems ?? (_pItems = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _pItems = value;
                RaisePropertyChanged();
            }
        }

        public Match pCure
        {
            get { return _pCure ?? (_pCure = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _pCure = value;
                RaisePropertyChanged();
            }
        }

        public Match pBeneficialGain
        {
            get { return _pBeneficialGain ?? (_pBeneficialGain = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _pBeneficialGain = value;
                RaisePropertyChanged();
            }
        }

        public Match pBeneficialLose
        {
            get { return _pBeneficialLose ?? (_pBeneficialLose = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _pBeneficialLose = value;
                RaisePropertyChanged();
            }
        }

        public Match pDetrimentalGain
        {
            get { return _pDetrimentalGain ?? (_pDetrimentalGain = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _pDetrimentalGain = value;
                RaisePropertyChanged();
            }
        }

        public Match pDetrimentalLose
        {
            get { return _pDetrimentalLose ?? (_pDetrimentalLose = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _pDetrimentalLose = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        private void Initialize()
        {
            switch (Constants.GameLanguage)
            {
                case "French":
                    pDamage = PlayerRegEx.DamageFr.Match(Cleaned);
                    pDamageAuto = PlayerRegEx.DamageAutoFr.Match(Cleaned);
                    pFailed = PlayerRegEx.FailedFr.Match(Cleaned);
                    pFailedAuto = PlayerRegEx.FailedAutoFr.Match(Cleaned);
                    pActions = PlayerRegEx.ActionsFr.Match(Cleaned);
                    pItems = PlayerRegEx.ItemsFr.Match(Cleaned);
                    pCure = PlayerRegEx.CureFr.Match(Cleaned);
                    pBeneficialGain = PlayerRegEx.BeneficialGainFr.Match(Cleaned);
                    pBeneficialLose = PlayerRegEx.BeneficialLoseFr.Match(Cleaned);
                    pDetrimentalGain = PlayerRegEx.DetrimentalGainFr.Match(Cleaned);
                    pDetrimentalLose = PlayerRegEx.DetrimentalLoseFr.Match(Cleaned);
                    mDamage = MonsterRegEx.DamageFr.Match(Cleaned);
                    mDamageAuto = MonsterRegEx.DamageAutoFr.Match(Cleaned);
                    mFailed = MonsterRegEx.FailedFr.Match(Cleaned);
                    mFailedAuto = MonsterRegEx.FailedAutoFr.Match(Cleaned);
                    mActions = MonsterRegEx.ActionsFr.Match(Cleaned);
                    mItems = MonsterRegEx.ItemsFr.Match(Cleaned);
                    mCure = MonsterRegEx.CureFr.Match(Cleaned);
                    mBeneficialGain = MonsterRegEx.BeneficialGainFr.Match(Cleaned);
                    mBeneficialLose = MonsterRegEx.BeneficialLoseFr.Match(Cleaned);
                    mDetrimentalGain = MonsterRegEx.DetrimentalGainFr.Match(Cleaned);
                    mDetrimentalLose = MonsterRegEx.DetrimentalLoseFr.Match(Cleaned);
                    Counter = "Contre";
                    Added = "Effet Supplémentaire";
                    HealingType = "PV";
                    RAttack = "D'Attaque À Distance";
                    Attack = "Attaque";
                    You = @"^[Vv]ous$";
                    YouString = "Vous";
                    Mitigated = "Dommages Atténué (Bouclier Magique)";
                    break;
                case "Japanese":
                    pDamage = PlayerRegEx.DamageJa.Match(Cleaned);
                    pDamageAuto = PlayerRegEx.DamageAutoJa.Match(Cleaned);
                    pFailed = PlayerRegEx.FailedJa.Match(Cleaned);
                    pFailedAuto = PlayerRegEx.FailedAutoJa.Match(Cleaned);
                    pActions = PlayerRegEx.ActionsJa.Match(Cleaned);
                    pItems = PlayerRegEx.ItemsJa.Match(Cleaned);
                    pCure = PlayerRegEx.CureJa.Match(Cleaned);
                    pBeneficialGain = PlayerRegEx.BeneficialGainJa.Match(Cleaned);
                    pBeneficialLose = PlayerRegEx.BeneficialLoseJa.Match(Cleaned);
                    pDetrimentalGain = PlayerRegEx.DetrimentalGainJa.Match(Cleaned);
                    pDetrimentalLose = PlayerRegEx.DetrimentalLoseJa.Match(Cleaned);
                    mDamage = MonsterRegEx.DamageJa.Match(Cleaned);
                    mDamageAuto = MonsterRegEx.DamageAutoJa.Match(Cleaned);
                    mFailed = MonsterRegEx.FailedJa.Match(Cleaned);
                    mFailedAuto = MonsterRegEx.FailedAutoJa.Match(Cleaned);
                    mActions = MonsterRegEx.ActionsJa.Match(Cleaned);
                    mItems = MonsterRegEx.ItemsJa.Match(Cleaned);
                    mCure = MonsterRegEx.CureJa.Match(Cleaned);
                    mBeneficialGain = MonsterRegEx.BeneficialGainJa.Match(Cleaned);
                    mBeneficialLose = MonsterRegEx.BeneficialLoseJa.Match(Cleaned);
                    mDetrimentalGain = MonsterRegEx.DetrimentalGainJa.Match(Cleaned);
                    mDetrimentalLose = MonsterRegEx.DetrimentalLoseJa.Match(Cleaned);
                    Counter = "カウンター";
                    Added = "追加効果";
                    HealingType = "ＨＰ";
                    RAttack = "Ranged Attack";
                    Attack = "Attack";
                    You = @"^\.$";
                    YouString = "君";
                    Mitigated = "軽減ダメージ（魔法の盾）";
                    break;
                case "German":
                    pDamage = PlayerRegEx.DamageDe.Match(Cleaned);
                    pDamageAuto = PlayerRegEx.DamageAutoDe.Match(Cleaned);
                    pFailed = PlayerRegEx.FailedDe.Match(Cleaned);
                    pFailedAuto = PlayerRegEx.FailedAutoDe.Match(Cleaned);
                    pActions = PlayerRegEx.ActionsDe.Match(Cleaned);
                    pItems = PlayerRegEx.ItemsDe.Match(Cleaned);
                    pCure = PlayerRegEx.CureDe.Match(Cleaned);
                    pBeneficialGain = PlayerRegEx.BeneficialGainDe.Match(Cleaned);
                    pBeneficialLose = PlayerRegEx.BeneficialLoseDe.Match(Cleaned);
                    pDetrimentalGain = PlayerRegEx.DetrimentalGainDe.Match(Cleaned);
                    pDetrimentalLose = PlayerRegEx.DetrimentalLoseDe.Match(Cleaned);
                    mDamage = MonsterRegEx.DamageDe.Match(Cleaned);
                    mDamageAuto = MonsterRegEx.DamageAutoDe.Match(Cleaned);
                    mFailed = MonsterRegEx.FailedDe.Match(Cleaned);
                    mFailedAuto = MonsterRegEx.FailedAutoDe.Match(Cleaned);
                    mActions = MonsterRegEx.ActionsDe.Match(Cleaned);
                    mItems = MonsterRegEx.ItemsDe.Match(Cleaned);
                    mCure = MonsterRegEx.CureDe.Match(Cleaned);
                    mBeneficialGain = MonsterRegEx.BeneficialGainDe.Match(Cleaned);
                    mBeneficialLose = MonsterRegEx.BeneficialLoseDe.Match(Cleaned);
                    mDetrimentalGain = MonsterRegEx.DetrimentalGainDe.Match(Cleaned);
                    mDetrimentalLose = MonsterRegEx.DetrimentalLoseDe.Match(Cleaned);
                    Counter = "Counter";
                    Added = "Zusatzefeckt";
                    HealingType = "LP";
                    RAttack = "Ranged Attack";
                    Attack = "Attack";
                    You = @"^[Dd](ich|ie|u)$";
                    YouString = "Du";
                    Mitigated = "Schäden Gemildert (Zauberschild)";
                    break;
                default:
                    pDamage = PlayerRegEx.DamageEn.Match(Cleaned);
                    pDamageAuto = PlayerRegEx.DamageAutoEn.Match(Cleaned);
                    pFailed = PlayerRegEx.FailedEn.Match(Cleaned);
                    pFailedAuto = PlayerRegEx.FailedAutoEn.Match(Cleaned);
                    pActions = PlayerRegEx.ActionsEn.Match(Cleaned);
                    pItems = PlayerRegEx.ItemsEn.Match(Cleaned);
                    pCure = PlayerRegEx.CureEn.Match(Cleaned);
                    pBeneficialGain = PlayerRegEx.BeneficialGainEn.Match(Cleaned);
                    pBeneficialLose = PlayerRegEx.BeneficialLoseEn.Match(Cleaned);
                    pDetrimentalGain = PlayerRegEx.DetrimentalGainEn.Match(Cleaned);
                    pDetrimentalLose = PlayerRegEx.DetrimentalLoseEn.Match(Cleaned);
                    mDamage = MonsterRegEx.DamageEn.Match(Cleaned);
                    mDamageAuto = MonsterRegEx.DamageAutoEn.Match(Cleaned);
                    mFailed = MonsterRegEx.FailedEn.Match(Cleaned);
                    mFailedAuto = MonsterRegEx.FailedAutoEn.Match(Cleaned);
                    mActions = MonsterRegEx.ActionsEn.Match(Cleaned);
                    mItems = MonsterRegEx.ItemsEn.Match(Cleaned);
                    mCure = MonsterRegEx.CureEn.Match(Cleaned);
                    mBeneficialGain = MonsterRegEx.BeneficialGainEn.Match(Cleaned);
                    mBeneficialLose = MonsterRegEx.BeneficialLoseEn.Match(Cleaned);
                    mDetrimentalGain = MonsterRegEx.DetrimentalGainEn.Match(Cleaned);
                    mDetrimentalLose = MonsterRegEx.DetrimentalLoseEn.Match(Cleaned);
                    Counter = "Counter";
                    Added = "Additional Effect";
                    HealingType = "HP";
                    RAttack = "Ranged Attack";
                    Attack = "Attack";
                    You = @"^[Yy]ou?$";
                    YouString = "You";
                    Mitigated = "Mitigated Damage (Magic Shield)";
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
