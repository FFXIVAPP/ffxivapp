// FFXIVAPP.Client
// SettingsViewModel.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FFXIVAPP.Client.Plugins.Log.Helpers;
using FFXIVAPP.Client.Plugins.Log.Views;
using FFXIVAPP.Common.ViewModelBase;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Plugins.Log.ViewModels
{
    [DoNotObfuscate]
    internal sealed class SettingsViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static SettingsViewModel _instance;

        public static SettingsViewModel Instance
        {
            get { return _instance ?? (_instance = new SettingsViewModel()); }
        }

        #endregion

        #region Declarations

        public ICommand AddTabCommand { get; private set; }

        #endregion

        public SettingsViewModel()
        {
            AddTabCommand = new DelegateCommand(AddTab);
        }

        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings

        /// <summary>
        /// </summary>
        private static void AddTab()
        {
            var xKey = SettingsView.View.TName.Text;
            string xValue;
            var xRegularExpression = SettingsView.View.TRegEx.Text;
            if (SettingsView.View.Codes.SelectedItems.Count < 1)
            {
                xValue = "";
            }
            else
            {
                xValue = SettingsView.View.Codes.SelectedItems.Cast<object>()
                                     .Aggregate("", (current, item) => current + (item.ToString()
                                                                                      .Split(',')[0] + ","))
                                     .Replace("[", "");
                xValue = xValue.Substring(0, xValue.Length - 1);
            }
            if (xKey == "" || xValue == "" || xRegularExpression == "")
            {
            }
            else
            {
                TabItemHelper.AddTabByName(xKey, xValue, xRegularExpression);
                SettingsView.View.TName.Text = "";
                SettingsView.View.Codes.UnselectAll();
                MainView.View.MainViewTC.SelectedIndex = MainView.View.MainViewTC.Items.Count - 1;
                ShellView.View.ShellViewTC.SelectedIndex = 0;
            }
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
