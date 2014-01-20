// FFXIVAPP.Client
// ShellView.xaml.cs
// 
// © 2013 Ryan Wilson

using System.ComponentModel;
using System.Windows;
using FFXIVAPP.Client.Models.Parse;
using FFXIVAPP.Client.Models.Parse.Events;

namespace FFXIVAPP.Client.Views.Parse
{
    /// <summary>
    ///     Interaction logic for DefaultView.xaml
    /// </summary>
    public partial class ShellView
    {
        #region Declarations

        private bool IsRendered { get; set; }

        #endregion

        public static ShellView View;

        public ShellView()
        {
            InitializeComponent();
            View = this;
        }

        private void ShellView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (IsRendered)
            {
                return;
            }
            IsRendered = true;
            Constants.Parse.PluginSettings.PropertyChanged += PluginSettingsOnPropertyChanged;
        }

        private void PluginSettingsOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case "ParseYou":
                    ParseControl.Instance.StatMonitor.ToggleFilter(EventParser.You);
                    ParseControl.Instance.StatMonitor.ToggleFilter(EventParser.Pet);
                    break;
                case "ParseParty":
                    ParseControl.Instance.StatMonitor.ToggleFilter(EventParser.Party);
                    ParseControl.Instance.StatMonitor.ToggleFilter(EventParser.PetParty);
                    break;
                case "ParseAlliance":
                    ParseControl.Instance.StatMonitor.ToggleFilter(EventParser.Alliance);
                    ParseControl.Instance.StatMonitor.ToggleFilter(EventParser.PetAlliance);
                    break;
                case "ParseOther":
                    ParseControl.Instance.StatMonitor.ToggleFilter(EventParser.Other);
                    ParseControl.Instance.StatMonitor.ToggleFilter(EventParser.PetOther);
                    break;
            }
        }
    }
}
