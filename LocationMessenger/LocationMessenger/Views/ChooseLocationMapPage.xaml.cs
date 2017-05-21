using System;
using LocationMessenger.ViewModels;
using LocationMessenger.Views.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace LocationMessenger.Views
{
    public partial class ChooseLocationMapPage : ContentPage
    {
        private readonly ChooseLocationMapPageViewModel _viewModel;

        public ChooseLocationMapPage()
        {
			NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

			_viewModel = BindingContext as ChooseLocationMapPageViewModel;

            ChooseMap.MoveToRegion(MapSpan.FromCenterAndRadius(
				_viewModel.LocationService.Location!=null ? 
					_viewModel.LocationService.Location : 
					_viewModel.LocationService.DefaultLocation, 
				Distance.FromMiles(1.0)));
        }

        private void ChooseLocationMap(object sender, MapTapEventArgs e)
        {
            _viewModel?.Tapped(sender, e);
        }

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_viewModel.LocationService.ChangedLocation += Track;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			_viewModel.LocationService.ChangedLocation -= Track;
		}

		void Track(object sender, EventArgs e)
		{
			ChooseMap.MoveToRegion(MapSpan.FromCenterAndRadius(
				_viewModel.LocationService.Location, Distance.FromMiles(1.0)));
		}
    }
}
