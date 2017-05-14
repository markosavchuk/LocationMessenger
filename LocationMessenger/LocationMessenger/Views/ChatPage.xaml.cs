using System;
using System.ComponentModel;
using System.Linq;
using LocationMessenger.Models;
using LocationMessenger.ViewModels;
using LocationMessenger.ViewModels.ListViews;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Maps;
using System.Collections.Generic;

namespace LocationMessenger.Views
{
    public partial class ChatPage : ContentPage
    {
        private ChatPageViewModel _viewModel;

        public ChatPage()
        {
            InitializeComponent();

            _viewModel = BindingContext as ChatPageViewModel;

			_viewModel.RefreshScrollDown += (sender, e) => 
			{
			 	var msg = _viewModel.Messages.LastOrDefault();
				if (msg != null)
				{
					LstView.ScrollTo(msg, ScrollToPosition.End, false);
				}
			};
        }

		private List<int> _signedMap = new List<int>();

		private void OnAppearingViewCell(object sender, EventArgs e)
        {
			var cell = sender as ViewCell;
			var msg = cell?.BindingContext as ChatListViewModel;
			if (msg != null)
			{
				var stackLayout = cell.View as StackLayout;
				if (stackLayout?.Children.Count >= 3)
				{
					if (!_signedMap.Contains(msg.GetHashCode()))
					{
						var view = stackLayout.Children[2] as ContentView;
						if (view!=null)
						{
							view.PropertyChanged+= (senderCell, eCell) => 
							{
								if (eCell.PropertyName.Equals("IsVisible") && msg.IsSelected)
								{
									var map = new Map();
									var position = new Position(msg.Message.Location.Latitude, msg.Message.Location.Longitude);

									map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(0.1)));
									map.Pins.Add(new Pin()
									{
										Label = String.Empty,
										Position = position
									});
									map.HeightRequest = 200;
									map.HasZoomEnabled = true;
									map.MapType = MapType.Street;

									view.Content = map;
								}
								else if (eCell.PropertyName.Equals("IsVisible") && !msg.IsSelected)
								{
									try
									{
										view.Content = null;
									}
									catch
									{
										// possible exception due to bug in Xamarin.Forms library
										msg.IsSelected = false;
									}
								}
							};
						}

						_signedMap.Add(msg.GetHashCode());
					}

					var contentView = stackLayout.Children[2] as ContentView;
					if (contentView != null && msg.IsSelected)
					{
						try
						{
							var msgMap = contentView.Content as Map;
							if (msgMap!=null)
							{
								msgMap = new Map();
								var position = new Position(msg.Message.Location.Latitude, msg.Message.Location.Longitude);

								msgMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(0.1)));
								msgMap.Pins.Add(new Pin()
								{
									Label = String.Empty,
									Position = position
								});
								msgMap.HeightRequest = 200;
								msgMap.HasZoomEnabled = true;
								msgMap.MapType = MapType.Street;

								contentView.Content = msgMap;
							}
						}
						catch (NullReferenceException ex)
						{
							// possible exception due to bug in Xamarin.Forms library
						}
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
