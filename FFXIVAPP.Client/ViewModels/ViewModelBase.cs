using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using ReactiveUI;

namespace FFXIVAPP.Client.ViewModels {
    public class ViewModelBase : ReactiveObject {
        protected void OnPropertyChanged(string propertyName) {
            this.RaisePropertyChanged(propertyName);
        } 

        protected void RaisePropertyChanged([CallerMemberName] string caller = "") {
                IReactiveObjectExtensions.RaisePropertyChanged(this, caller);
        }
    }
}
