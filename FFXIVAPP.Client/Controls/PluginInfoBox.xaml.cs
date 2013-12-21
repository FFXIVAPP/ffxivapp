// FFXIVAPP.Client
// PluginInfoBox.xaml.cs
// 
// © 2013 Ryan Wilson

using System;
using MahApps.Metro.Controls;

namespace FFXIVAPP.Client.Controls
{
    /// <summary>
    ///     Interaction logic for PluginInfoBox.xaml
    /// </summary>
    public partial class PluginInfoBox
    {
        public PluginInfoBox()
        {
            InitializeComponent();
        }

        private void ToggleSwitchOnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                var name = ((ToggleSwitch) sender).Tag.ToString();
                Constants.Application.EnabledPlugins[name] = ((ToggleSwitch) sender).IsChecked ?? true;
            }
            catch (Exception ex)
            {
            }
        }
    }
}
