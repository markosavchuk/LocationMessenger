using Prism.Commands;
using Prism.Mvvm;
using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Navigation;
using Xamarin.Forms.Maps;

namespace LocationMessenger.ViewModels
{
    public class MapPageViewModel : BindableBase
    {
        private INavigationService _navigationService;

        private string _title = "Map";
        private ObservableCollection<MapPinViewModel> _pins;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<MapPinViewModel> Pins
        {
            get { return _pins; }
            set { SetProperty(ref _pins, value); }
        }

        public DelegateCommand<string> MessageClicked{ get; set; }

        public MapPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            _pins = new ObservableCollection<MapPinViewModel>();
            var chats = FakeData.FakeData.Chats;
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
                            Label = String.Empty // should be setted
                        }
                    }); 
                }
            }

            MessageClicked = new DelegateCommand<string>(NavigateToChat);
        }

        private void NavigateToChat(string idMessage)
        {
            var chats = FakeData.FakeData.Chats;
            var b = chats.Any(c => c.Messages.Any(m => m.Id.Equals(idMessage)));
            if (FakeData.FakeData.Chats.Any(c => c.Messages.Any(m => m.Id.Equals(idMessage))))
            {
                var idChat = FakeData.FakeData.Chats
                    .First(c => c.Messages.Exists(m => m.Id.Equals(idMessage))).Id;
                _navigationService.NavigateAsync("ChatPage?idChat=" + idChat);
            }            
        }
    }
}
