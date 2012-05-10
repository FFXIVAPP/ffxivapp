// ParseModXIV
// MainTabControlViewModel.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using AppModXIV.Commands;
using ParseModXIV.View;

namespace ParseModXIV.ViewModel
{
    internal class MainTabControlViewModel : INotifyPropertyChanged
    {
        private DelegateCommand _command;

        #region " COMMAND FUNCTIONS "

        public ICommand AbilityPlayerDetailCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(AbilityPlayerDetail);

                return _command;
            }
        }

        public ICommand AbilityPlayerMonsterCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(AbilityPlayerMonster);

                return _command;
            }
        }

        public ICommand AbilityPlayerMonsterDetailsCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(AbilityPlayerMonsterDetails);

                return _command;
            }
        }

        public ICommand HealingAbiltiesCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(HealingAbilties);

                return _command;
            }
        }

        public ICommand HealingPlayersCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(HealingPlayers);

                return _command;
            }
        }

        public ICommand HealingDetailsCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(HealingDetails);

                return _command;
            }
        }

        public ICommand DamageDetailMonsterCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(DamageDetailMonster);

                return _command;
            }
        }

        public ICommand DamageDetailAbiltiesCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(DamageDetailAbilties);

                return _command;
            }
        }

        public ICommand MonsterDetailAbilitiesCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(MonsterDetailAbilities);

                return _command;
            }
        }

        public ICommand MonsterDetailsDropsCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(MonsterDetailsDrops);

                return _command;
            }
        }

        #endregion

        #region " GUI FUNCTIONS "

        private static void AbilityPlayerDetail()
        {
            MainTabControlView.View.AbilityPlayerDetail.Visibility = (MainTabControlView.View.AbilityPlayerDetail.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
        }

        private static void AbilityPlayerMonster()
        {
            MainTabControlView.View.AbilityPlayerMonster.Visibility = (MainTabControlView.View.AbilityPlayerMonster.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
        }

        private static void AbilityPlayerMonsterDetails()
        {
            MainTabControlView.View.AbilityPlayerMonsterDetails.Visibility = (MainTabControlView.View.AbilityPlayerMonsterDetails.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
        }

        private static void HealingAbilties()
        {
            MainTabControlView.View.HealingAbilties.Visibility = (MainTabControlView.View.HealingAbilties.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
        }

        private static void HealingPlayers()
        {
            MainTabControlView.View.HealingPlayers.Visibility = (MainTabControlView.View.HealingPlayers.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
        }

        private static void HealingDetails()
        {
            MainTabControlView.View.HealingDetails.Visibility = (MainTabControlView.View.HealingDetails.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
        }

        private static void DamageDetailMonster()
        {
            MainTabControlView.View.DamageDetailMonster.Visibility = (MainTabControlView.View.DamageDetailMonster.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
        }

        private static void DamageDetailAbilties()
        {
            MainTabControlView.View.DamageDetailAbilties.Visibility = (MainTabControlView.View.DamageDetailAbilties.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
        }

        private static void MonsterDetailsDrops()
        {
            MainTabControlView.View.MonsterDetailsDrops.Visibility = (MainTabControlView.View.MonsterDetailsDrops.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
        }

        private static void MonsterDetailAbilities()
        {
            MainTabControlView.View.MonsterDetailAbilities.Visibility = (MainTabControlView.View.MonsterDetailAbilities.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}