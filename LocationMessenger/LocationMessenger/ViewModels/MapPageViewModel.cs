using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LocationMessenger.ViewModels
{
    public class MapPageViewModel : BindableBase
    {
        private string _title = "Map";

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MapPageViewModel()
        {

        }
    }
}
