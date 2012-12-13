// FFXIVAPP.Common
// xFlowDocument.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System.ComponentModel;
using System.Runtime.CompilerServices;

#endregion

namespace FFXIVAPP.Common.Controls
{
    /// <summary>
    ///     Interaction logic for xFlowDocument.xaml
    /// </summary>
    public partial class xFlowDocument : INotifyPropertyChanged
    {
        #region Property Bindings

        private string _zoomLevel;

        public string ZoomLevel
        {
            get { return _zoomLevel; }
            set
            {
                _zoomLevel = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public xFlowDocument()
        {
            InitializeComponent();
            ZoomLevel = "100";
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
