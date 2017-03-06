using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocationMessenger.ViewModels;
using Prism.Commands;
using Xamarin.Forms.Maps;

namespace LocationMessenger.Views.CustomControls
{
    public class MapMessages : Map
    {
        public ObservableCollection<MapPinViewModel> CustomPins { get; set; }
        public DelegateCommand<string> MessageClicked { get; set; }

        public MapMessages()
        {
           
        }
    }
}
