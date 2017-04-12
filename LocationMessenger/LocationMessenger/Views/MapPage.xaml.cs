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
		MapPageViewModel _viewmodel;

        public MapPage()
        {
            InitializeComponent();

            _viewmodel = BindingContext as MapPageViewModel;
            if (_viewmodel != null)
            {
                MapMsg.CustomPins = _viewmodel.Pins;
				MapMsg.MessageClicked += (sender, e) => _viewmodel.NavigateToClickedChat(e);

				_viewmodel.PinsChanged += (sender, e) => MapMsg.UpdatePins();
            }

            MapMsg.MoveToRegion(MapSpan.FromCenterAndRadius(
              new Position(49.834813, 23.997578), Distance.FromMiles(1.0)));
        }
    }
}
