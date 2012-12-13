// FFXIVAPP
// ParseVM.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System.Windows.Input;
using FFXIVAPP.Classes;

#endregion

namespace FFXIVAPP.ViewModels
{
    public class ParseVM
    {
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

        public ICommand ResetStatsCommand { get; private set; }
    }
}
