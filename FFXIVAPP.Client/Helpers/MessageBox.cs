using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace FFXIVAPP.Client.Helpers
{
    public enum MessageDialogStyle
    {
        Affirmative,
        AffirmativeAndNegative        
    }

    [PropertyChanged.DoNotNotify]
    public class MessageBox: Window
    {
        private TextBlock _text;
        private Button _btnOk;
        private Button _btnCancel;

        private string _message;
        public string Message
        {
            get => _message;
            set 
            {
                _message = value;
                _text.Text = _message;
            }
        }

        public MessageBox(bool showCancel = false)
        {
            this.Width = 200;
            this.Height = 150;
            this.DataContext = this;
            this.Padding = Thickness.Parse("5");

            var grid = new Grid
            {
                RowDefinitions = RowDefinitions.Parse("*,Auto"),
                ColumnDefinitions = ColumnDefinitions.Parse("*,*")
            };

            var t = new Subject<string>();

            _text = new TextBlock
            {
                [Grid.RowProperty] = 0,
                [Grid.ColumnProperty] = 0,
                [Grid.ColumnSpanProperty] = 2,
                TextWrapping = TextWrapping.Wrap,
                Text = Message
            };

            _btnOk = new Button
            {
                Margin = Thickness.Parse("5"),
                Content = "OK"
            };

            var stackOk = new StackPanel
            {
                [Grid.RowProperty] = 1,
                [Grid.ColumnProperty] = 0
            };

            stackOk.Children.Add(_btnOk);

            _btnCancel  = new Button
            {
                Margin = Thickness.Parse("5"),
                Content = "Cancel"
            };

            var stackCancel = new StackPanel
            {
                [Grid.RowProperty] = 1,
                [Grid.ColumnProperty] = 1
            };

            stackCancel.Children.Add(_btnCancel);

            _btnOk.Click += delegate {
                this.Close(true);
            };

            _btnCancel.Click += delegate {
                this.Close(false);
            };

            grid.Children.Add(_text);

            if (showCancel)
            {
                grid.Children.Add(stackCancel);
            }
            else
            {
                stackOk[Grid.ColumnSpanProperty] = 2;
            }

            grid.Children.Add(stackOk);
            this.Content = grid;
        }

        public static async Task<bool> ShowAsync(string title, string message, MessageDialogStyle msgStyle, Action okAction = null , Action cancelAction = null)
        {
            return await ShowAsync(null, title, message, msgStyle, okAction, cancelAction);
        }

        public static async Task<bool> ShowAsync(WindowBase owner, string title, string message, MessageDialogStyle msgStyle, Action okAction = null , Action cancelAction = null)
        {
            var msg = new MessageBox(msgStyle == MessageDialogStyle.AffirmativeAndNegative)
            {
                Title = title,
                Message = message
            };

            var result = await msg.ShowDialog<bool>();
            if (result)
            {
                okAction?.Invoke();
            }
            else
            {
                cancelAction?.Invoke();
            }

            return result;
        } 
    } 
} 
