// FFXIVAPP
// ParseVM.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System.Windows.Input;
using FFXIVAPP.Classes;

namespace FFXIVAPP.ViewModels
{
    public class ParseVM
    {
        public ICommand ResetStatsCommand { get; private set; }

        public ParseVM()
        {
            ResetStatsCommand = new DelegateCommand(ResetStats);
        }

        #region GUI Functions

        /// <summary>
        /// </summary>
        private static void ResetStats()
        {
            MainVM.ClearStats();
        }

        #endregion
    }
}