using Prism.Commands;
using Prism.Mvvm;
using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Navigation;
using Xamarin.Forms.Maps;
using Prism.Modularity;
using LocationMessenger.Models;

namespace LocationMessenger.ViewModels
{
    public class MapPageViewModel : BindableBase
    {
        private INavigationService _navigationService;
		private IData _data;

        private string _title = "Map";
        private ObservableCollection<MapPinViewModel> _pins;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<MapPinViewModel> Pins
        {
            get 
			{ 
				return _pins; 
			}
            set { SetProperty(ref _pins, value); }
        }

		//public event EventHandler<string> MessageClicked;

		public event EventHandler PinsChanged;

		public MapPageViewModel(INavigationService navigationService, IData data)
        {
            _navigationService = navigationService;
			_data = data;

			Pins = new ObservableCollection<MapPinViewModel>();

			_data.DataReady += (sender, e) => FillPins();
			_data.ChatsChanged += (sender, e) => FillPins();
        }

		public async void NavigateToClickedChat(string idMessage)
        {
            if (_data.Chats.Any(c => c.Messages.Any(m => m.Id.Equals(idMessage))))
            {
				var idChat = _data.Chats
                    .First(c => c.Messages.Exists(m => m.Id.Equals(idMessage))).Id;

				await _navigationService.NavigateAsync("ChatPage?idChat=" + idChat, useModalNavigation:false);


				//
					/*System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
					sw.Start();

					var service = new AzureDataService();
					await service.Initialize(_data.Me.Id);
					var t = await service.GetChats();
					//var chat = await service.GetChats();

					sw.Stop();*/
				//
            }
        }

		private void FillPins()
		{
			if (_pins==null)
				_pins = new ObservableCollection<MapPinViewModel>();

			_pins.Clear();
			
			foreach (var chat in _data.Chats)
			{
				foreach (var message in chat.Messages)
				{
					Pins.Add(new MapPinViewModel()
					{
						AuthorName = (message.Owner.Name ?? "") + " " + (message.Owner.Surname ?? ""),
						IdMessage = message.Id,
						VisibleMessage = message.Text,
						UrlImage = message.Owner.Image.Url,
						Pin = new Pin()
						{
							Position = new Position(message.Location.Latitude, message.Location.Longitude),
							Label = String.Empty // should be set
						}
					});
				}
			}

			if (PinsChanged!=null)
				PinsChanged(null, EventArgs.Empty);
		}
    }
}
