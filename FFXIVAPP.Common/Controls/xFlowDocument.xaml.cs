// FFXIVAPP.Common
// xFlowDocument.xaml.cs
// 
// © 2013 Ryan Wilson

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
