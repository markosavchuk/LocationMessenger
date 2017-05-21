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

				 MapMsg.MoveToRegion(MapSpan.FromCenterAndRadius(
					_viewmodel.LocationService.Location!=null ?
						_viewmodel.LocationService.Location : 
						_viewmodel.LocationService.DefaultLocation, 
					Distance.FromMiles(1.0)));
            }
        }

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_viewmodel.LocationService.ChangedLocation += Track;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			_viewmodel.LocationService.ChangedLocation -= Track;
		}

		private void Track(object sender, EventArgs atgs)
		{
			MapMsg.MoveToRegion(MapSpan.FromCenterAndRadius(
						_viewmodel.LocationService.Location, Distance.FromMiles(1.0)));
		}
    }
}
