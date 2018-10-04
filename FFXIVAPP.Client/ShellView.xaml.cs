// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellView.xaml.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   ShellView.xaml.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client {
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Timers;
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;
    using Avalonia.Media;
    using FFXIVAPP.Client.Helpers;
    using FFXIVAPP.Client.Models;
    using FFXIVAPP.Client.SettingsProviders.Application;
    using FFXIVAPP.Common.Helpers;
    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;
    using NLog;
    using PropertyChanged;

    [DoNotNotify]
    public class ShellView : Window {
        public static ShellView View;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private bool IsRendered;

        private Timer _spinner;
        private RotateTransform _rotate;
        public ShellView()
        {
            View = this;
            this.Initialized += this.InitDone;
            InitializeComponent();
            var spinner = this.FindControl<Image>("PluginUpdateSpinner");
            _rotate = (RotateTransform)spinner.RenderTransform;
            _spinner = new Timer(25);
            _spinner.Elapsed += SpinnerRotatingTimer;

            ViewModels.UpdateViewModel.Instance.PropertyChanged += (o, e) => 
            {
                if (e.PropertyName == nameof(ViewModels.UpdateViewModel.UpdatingAvailablePlugins))
                {
                    if (ViewModels.UpdateViewModel.Instance.UpdatingAvailablePlugins)
                    {
                        _spinner.Start();
                    }
                    else
                    {
                        _spinner.Stop();
                        Avalonia.Threading.DispatcherTimer.RunOnce(() => {
                            _rotate.Angle = 0;
                        }, new TimeSpan(0));
                    }
                }
            };
        }

        public TabControl ShellViewTC { get; private set; }
        public TabControl PluginsTC { get; private set; }
        
        private void InitDone(object sender, EventArgs e) {
            if (this.IsRendered) {
                return;
            }

            this.IsRendered = true;

            this.ShellViewTC = this.FindControl<TabControl>("ShellViewTC");
            this.PluginsTC = this.FindControl<TabControl>("PluginsTC");

            if (string.IsNullOrWhiteSpace(Settings.Default.UILanguage)) {
                Settings.Default.UILanguage = Settings.Default.GameLanguage;
            }
            else {
                LocaleHelper.Update(Settings.Default.Culture);
            }

            DispatcherHelper.Invoke(
                delegate {
                    Initializer.LoadAvailablePlugins();
                    Initializer.CheckUpdates();
                    Initializer.SetGlobals();

                    Initializer.StartMemoryWorkers();
                    /* TODO: Network
                    if (Settings.Default.EnableNetworkReading && !Initializer.NetworkWorking) {
                        Initializer.StartNetworkWorker();
                    }
                    */
                });

            Initializer.GetHomePlugin();
            Initializer.UpdatePluginConstants();
        }

        private void SpinnerRotatingTimer(object sender, ElapsedEventArgs e)
        {
            Avalonia.Threading.DispatcherTimer.RunOnce(() => {
                _rotate.Angle = _rotate.Angle + 10;
            }, new TimeSpan(0));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}