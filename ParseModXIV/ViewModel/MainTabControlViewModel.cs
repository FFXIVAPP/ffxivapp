// Project: ParseModXIV
// File: MainTabControlViewModel.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.ComponentModel;
using System.Windows.Input;
using AppModXIV.Commands;
using ParseModXIV.View;

namespace ParseModXIV.ViewModel
{
    internal class MainTabControlViewModel : INotifyPropertyChanged
    {
        private DelegateCommand _command;
        private Boolean _isHostileExpanded;
        private Boolean _isHealingExpanded;
        private Boolean _isDamageExpanded;
        private Boolean _isMobExpanded;

        #region " COMMAND FUNCTIONS "

        public ICommand ToggleHostileExpandedCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(ToggleHostileExpanded);

                return _command;
            }
        }

        public ICommand ToggleHealingExpandedCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(ToggleHealingExpanded);

                return _command;
            }
        }

        public ICommand ToggleDamageExpandedCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(ToggleDamageExpanded);

                return _command;
            }
        }

        public ICommand ToggleMobExpandedCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(ToggleMobExpanded);

                return _command;
            }
        }

        public ICommand ToggleDropsExpandedCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(ToggleDropsExpanded);

                return _command;
            }
        }

        #endregion

        #region " GUI FUNCTIONS "

        private void ToggleHealingExpanded()
        {
            if (_isHealingExpanded)
            {
                MainTabControlView.View.gui_ToggleHealingList.Content = "Increase Size";
                MainTabControlView.View.gui_DetailHealing.Height = 150;
                _isHealingExpanded = false;
            }
            else
            {
                MainTabControlView.View.gui_ToggleHealingList.Content = "Decrease Size";
                MainTabControlView.View.gui_DetailHealing.Height = Double.NaN;
                _isHealingExpanded = true;
            }
        }

        private void ToggleHostileExpanded()
        {
            if (_isHostileExpanded)
            {
                MainTabControlView.View.gui_ToggleHostileList.Content = "Increase Size";
                MainTabControlView.View.gui_DetailHostile.Height = 150;
                _isHostileExpanded = false;
            }
            else
            {
                MainTabControlView.View.gui_ToggleHostileList.Content = "Decrease Size";
                MainTabControlView.View.gui_DetailHostile.Height = Double.NaN;
                _isHostileExpanded = true;
            }
        }

        private void ToggleDamageExpanded()
        {
            if (_isDamageExpanded)
            {
                MainTabControlView.View.gui_ToggleDamageList.Content = "Increase Size";
                MainTabControlView.View.gui_DetailDamage.Height = 150;
                _isDamageExpanded = false;
            }
            else
            {
                MainTabControlView.View.gui_ToggleDamageList.Content = "Decrease Size";
                MainTabControlView.View.gui_DetailDamage.Height = Double.NaN;
                _isDamageExpanded = true;
            }
        }

        private void ToggleMobExpanded()
        {
            if (_isMobExpanded)
            {
                MainTabControlView.View.gui_ToggleMobList.Content = "Increase Size";
                MainTabControlView.View.gui_DetailMob.Height = 150;
                _isMobExpanded = false;
            }
            else
            {
                MainTabControlView.View.gui_ToggleMobList.Content = "Decrease Size";
                MainTabControlView.View.gui_DetailMob.Height = Double.NaN;
                _isMobExpanded = true;
            }
        }

        private void ToggleDropsExpanded()
        {
            if (_isMobExpanded)
            {
                MainTabControlView.View.gui_ToggleDropList.Content = "Increase Size";
                MainTabControlView.View.gui_DetailDrops.Height = 150;
                _isMobExpanded = false;
            }
            else
            {
                MainTabControlView.View.gui_ToggleDropList.Content = "Decrease Size";
                MainTabControlView.View.gui_DetailDrops.Height = Double.NaN;
                _isMobExpanded = true;
            }
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