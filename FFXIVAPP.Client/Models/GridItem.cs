using System;
using System.ComponentModel;
using ReactiveUI;

namespace FFXIVAPP.Client.Models
{
   [Obsolete("Remove this class")] 
    public class GridItem: INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Current { get; set; }
        public string Latest { get; set; }
        public string Files { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
