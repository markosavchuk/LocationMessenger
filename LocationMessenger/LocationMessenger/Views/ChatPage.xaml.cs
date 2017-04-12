using System;
using System.ComponentModel;
using System.Linq;
using LocationMessenger.Models;
using LocationMessenger.ViewModels;
using LocationMessenger.ViewModels.ListViews;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Maps;

namespace LocationMessenger.Views
{
    public partial class ChatPage : ContentPage
    {
        private ChatPageViewModel _viewModel;

        public ChatPage()
        {
            InitializeComponent();

            _viewModel = BindingContext as ChatPageViewModel;
        }

        private void OnChildListViewAdded(object sender, EventArgs e)
        {
            /*var cell = sender as ViewCell;
            var msg = cell?.BindingContext as ChatListViewModel;
            if (msg != null)
            {
                var stackLayout = cell.View as StackLayout;
                if (stackLayout?.Children.Count >= 2)
                {
                    var frame1 = stackLayout.Children[0] as Frame;
                    

                    var frame2 = stackLayout.Children[1] as Frame;
                }
            }*/
        }

        private void OnAppearing(object sender, EventArgs e)
        {
            var cell = sender as ViewCell;
            var msg = cell?.BindingContext as ChatListViewModel;
            if (msg != null)
            {
                var stackLayout = cell.View as StackLayout;
                if (stackLayout?.Children.Count >= 3)
                {
                    var map = stackLayout.Children[2] as Map;

                    if (map != null)
                    {
                        stackLayout.Children.Remove(map);
                        map = new Map();
                        var position = new Position(msg.Message.Location.Latitude, msg.Message.Location.Longitude);

                        map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(0.1)));
                        map.Pins.Add(new Pin()
                        {
                            Label = String.Empty,
                            Position = position
                        });
                        map.HeightRequest = 200;
                        map.HasZoomEnabled = false;
                        map.MapType = MapType.Street;
                        
                        stackLayout.Children.Add(map);
                    }
                }
            }
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            LstView.SelectedItem = null;
        }

        private void Entry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.TypedMessage = e.NewTextValue;
        }
    }
}
