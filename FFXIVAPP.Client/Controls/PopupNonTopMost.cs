// FFXIVAPP.Client
// PopupNonTopMost.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Windows.Controls.Primitives;
using FFXIVAPP.Client.Properties;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Controls
{
    [DoNotObfuscate]
    public class PopupNonTopmost : Popup
    {
        protected override void OnOpened(EventArgs e)
        {
            ShellView.View.Topmost = false;
            base.OnOpened(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            ShellView.View.Topmost = Settings.Default.TopMost;
            base.OnClosed(e);
        }
    }
}
