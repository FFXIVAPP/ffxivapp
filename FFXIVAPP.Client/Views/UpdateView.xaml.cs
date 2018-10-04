using System;
using System.Timers;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using FFXIVAPP.Client.ViewModels;

namespace FFXIVAPP.Client.Views
{
    public class UpdateView : UserControlBase
    {
        private Timer _spinner;
        private RotateTransform _rotate;

        public UpdateView()
        {
            InitializeComponent();
            var spinner = this.FindControl<Image>("PluginUpdateSpinner");
            _rotate = (RotateTransform)spinner.RenderTransform;
            _spinner = new Timer(25);
            _spinner.Elapsed += SpinnerRotatingTimer;

            UpdateViewModel.Instance.PropertyChanged += (o, e) => 
            {
                if (e.PropertyName == nameof(UpdateViewModel.UpdatingAvailablePlugins))
                {
                    if (UpdateViewModel.Instance.UpdatingAvailablePlugins)
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