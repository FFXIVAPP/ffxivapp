// FFXIVAPP.Common
// IgnoreMouseWheelBehavior.cs
// 
// © 2013 Ryan Wilson

using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace FFXIVAPP.Common.Behaviors
{
    public class IgnoreMouseWheelBehavior : Behavior<UIElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseWheel += AssociatedObjectPreviewMouseWheel;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseWheel -= AssociatedObjectPreviewMouseWheel;
            base.OnDetaching();
        }

        private void AssociatedObjectPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            e2.RoutedEvent = UIElement.MouseWheelEvent;

            AssociatedObject.RaiseEvent(e2);
        }
    }
}
