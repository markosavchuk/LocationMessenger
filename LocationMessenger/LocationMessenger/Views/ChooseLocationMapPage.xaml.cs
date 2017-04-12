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
            InitializeComponent();

            ChooseMap.MoveToRegion(MapSpan.FromCenterAndRadius(
              new Position(49.834813, 23.997578), Distance.FromMiles(1.0)));

            _viewModel = BindingContext as ChooseLocationMapPageViewModel;
        }

        private void ChooseLocationMap(object sender, MapTapEventArgs e)
        {
            _viewModel?.Tapped(sender, e);
        }
    }
}
