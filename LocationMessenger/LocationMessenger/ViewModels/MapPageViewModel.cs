using Prism.Commands;
using Prism.Mvvm;
using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Navigation;
using Xamarin.Forms.Maps;
using Prism.Modularity;

namespace LocationMessenger.ViewModels
{
    public class MapPageViewModel : BindableBase
    {
        private INavigationService _navigationService;
		private readonly IModuleManager _moduleManager;

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

        public MapPageViewModel(IModuleManager moduleManager, INavigationService navigationService)
        {
            _navigationService = navigationService;
			_moduleManager = moduleManager;

			//MessageClicked += (sender, e) => NavigateToClickedChat(e);

			FakeData.FakeData.ChatsChaged += (sender, e) =>
			{
				FillPins();
			};

			FillPins();
        }

		public void NavigateToClickedChat(string idMessage)
        {
            if (FakeData.FakeData.Chats.Any(c => c.Messages.Any(m => m.Id.Equals(idMessage))))
            {
                var idChat = FakeData.FakeData.Chats
                    .First(c => c.Messages.Exists(m => m.Id.Equals(idMessage))).Id;

				_navigationService.NavigateAsync("ChatPage?idChat=" + idChat, useModalNavigation:false);
            }            
        }

		private void FillPins()
		{
			if (_pins==null)
				_pins = new ObservableCollection<MapPinViewModel>();

			_pins.Clear();
			
			foreach (var chat in FakeData.FakeData.Chats)
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
