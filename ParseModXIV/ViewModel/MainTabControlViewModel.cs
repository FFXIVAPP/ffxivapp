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

        #endregion

        #region " GUI FUNCTIONS "

        private void AbilityPlayerDetail()
        {
            MainTabControlView.View.AbilityPlayerDetail.Visibility = (MainTabControlView.View.AbilityPlayerDetail.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void AbilityPlayerMonster()
        {
            MainTabControlView.View.AbilityPlayerMonster.Visibility = (MainTabControlView.View.AbilityPlayerMonster.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void AbilityPlayerMonsterDetails()
        {
            MainTabControlView.View.AbilityPlayerMonsterDetails.Visibility = (MainTabControlView.View.AbilityPlayerMonsterDetails.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
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