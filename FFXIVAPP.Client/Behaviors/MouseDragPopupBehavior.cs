// FFXIVAPP.Client
// MouseDragPopupBehavior.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Behaviors
{
    [DoNotObfuscate]
    internal class MouseDragPopupBehavior : Behavior<Popup>
    {
        private bool _mouseDown;
        private Point _oldMousePosition;

        protected override void OnAttached()
        {
            AssociatedObject.MouseLeftButtonDown += (s, e) =>
            {
                _mouseDown = true;
                _oldMousePosition = AssociatedObject.PointToScreen(e.GetPosition(AssociatedObject));
                AssociatedObject.Child.CaptureMouse();
            };
            AssociatedObject.MouseMove += (s, e) =>
            {
                if (!_mouseDown)
                {
                    return;
                }
                var newMousePosition = AssociatedObject.PointToScreen(e.GetPosition(AssociatedObject));
                var offset = newMousePosition - _oldMousePosition;
                _oldMousePosition = newMousePosition;
                AssociatedObject.HorizontalOffset += offset.X;
                AssociatedObject.VerticalOffset += offset.Y;
            };
            AssociatedObject.MouseLeftButtonUp += (s, e) =>
            {
                _mouseDown = false;
                AssociatedObject.Child.ReleaseMouseCapture();
            };
        }
    }
}
