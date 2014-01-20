// FFXIVAPP.Client
// MainView.xaml.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Windows;
using System.Windows.Controls;

namespace FFXIVAPP.Client.Views.Parse
{
    /// <summary>
    ///     Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView
    {
        public static MainView View;

        public MainView()
        {
            InitializeComponent();
            View = this;
        }
    }
}
