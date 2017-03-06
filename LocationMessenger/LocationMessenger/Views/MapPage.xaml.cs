using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocationMessenger.ViewModels;
using LocationMessenger.Views.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace LocationMessenger.Views
{
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();

            var viewmodel = BindingContext as MapPageViewModel;
            if (viewmodel != null)
            {
                MapMsg.CustomPins = viewmodel.Pins;
                MapMsg.MessageClicked = viewmodel.MessageClicked;
            }

            MapMsg.MoveToRegion(MapSpan.FromCenterAndRadius(
              new Position(49.834813, 23.997578), Distance.FromMiles(1.0)));
        }
    }
}
