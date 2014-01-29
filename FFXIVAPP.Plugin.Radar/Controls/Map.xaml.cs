// FFXIVAPP.Plugin.Radar
// Map.xaml.cs
// 
// © 2013 Ryan Wilson

using System.Windows.Media;

namespace FFXIVAPP.Plugin.Radar.Controls
{
    /// <summary>
    ///     Interaction logic for Radar.xaml
    /// </summary>
    public partial class Map
    {
        #region Map Declarations

        public bool IsRendered { get; set; }

        #endregion

        public Map View;

        public Map()
        {
            View = this;
            InitializeComponent();
            if (IsRendered)
            {
                return;
            }
            IsRendered = true;
        }

        public void Refresh()
        {
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }
    }
}
