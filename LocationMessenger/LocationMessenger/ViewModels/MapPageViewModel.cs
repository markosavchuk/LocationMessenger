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

        private void NavigateToChat(string id)
        {
            _navigationService.NavigateAsync("ChatPage?id=" + id);
        }
    }
}
