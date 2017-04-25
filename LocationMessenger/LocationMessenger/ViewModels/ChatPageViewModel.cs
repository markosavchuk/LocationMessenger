using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LocationMessenger.Models;
using LocationMessenger.ViewModels.ListViews;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace LocationMessenger.ViewModels
{
	public class ChatPageViewModel : BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private string _id;
        private ObservableCollection<ChatListViewModel> _messages;
        private string _buttonTitle = "Choose location";
        private Position? _choosedLocation;
        private string _typedMessage;
        private string _choosedAddress = "Address for message...";
		private string _title = "Chat Page";

		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}

        public ObservableCollection<ChatListViewModel> Messages
        {
            get { return _messages; }
            set { SetProperty(ref _messages, value); }
        }

        public string ButtonTitle
        {
            get { return _buttonTitle; }
            set { SetProperty(ref _buttonTitle, value); }
        }

        public Position? ChoosedLocation
        {
            get { return _choosedLocation; }
            set
            {
                SetProperty(ref _choosedLocation, value);
                if (_choosedLocation == null)
                {
                    ButtonTitle = "Choose location";
                }
                else
                {
                    ButtonTitle = "Send";
                }
                /*IsNotChoosedLocation = _choosedLocation == null;
                IsChoosedLocation = !IsNotChoosedLocation;*/
            }
        }

        public string ChoosedAddress
        {
            get { return _choosedAddress; }
            set { SetProperty(ref _choosedAddress, value); }
        }

        public string TypedMessage
        {
            get { return _typedMessage; }
            set { SetProperty(ref _typedMessage, value); }
        }

        /*public bool IsChoosedLocation
        {
            get { return _isChoosedLocation; }
            set
            {
                SetProperty(ref _isChoosedLocation, value);
                SetProperty(ref _isNotChoosedLocation, !value);
            }
        }

        public bool IsNotChoosedLocation
        {
            get { return _isNotChoosedLocation; }
            set
            {
                SetProperty(ref _isNotChoosedLocation, value);
                SetProperty(ref _isChoosedLocation, !value);
            }
        }*/

        public DelegateCommand ChooseLocation { get; set; }

        public ChatPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            Messages = new ObservableCollection<ChatListViewModel>();
            ChooseLocation = new DelegateCommand(GoToChooseLocation);
        }
       
        public void OnNavigatedFrom(NavigationParameters parameters)
        {
			//if (ChoosedLocation != null)
			//	_navigationService.NavigateAsync("MainTabbedPage");
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("idChat"))
            {
                var id = parameters["idChat"] as string;
                if (id != null)
                {
                    _id = id;

					var members = FakeData.FakeData.Chats.FirstOrDefault(c => c.Id.Equals(id)).Members;
					if (!members[0].Equals(FakeData.FakeData.Me))
					{
						Title = (members[0].Name ?? "") + " " + (members[0].Surname ?? "");
					}
					else
					{
						Title = (members[1].Name ?? "") + " " + (members[1].Surname ?? "");
					}

                    var messages = FakeData.FakeData.Chats.FirstOrDefault(c => c.Id.Equals(id)).Messages;
                    foreach (var msg in messages)
                    {
                        Messages.Add(new ChatListViewModel()
                        {
                            Message = msg,
                            Address = "Fetching address...",
                            Alligment = (msg.Owner.Id.Equals(FakeData.FakeData.Me.Id)) 
                                ? LayoutOptions.End 
                                : LayoutOptions.Start,
                        });                        
                    }

                    foreach (var msg in Messages)
                    {
                        try
                        {
                            msg.Address = await GetAdressFromLocation(
                                new Position(msg.Message.Location.Latitude, msg.Message.Location.Longitude));
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine("Failed to load address");   
                        }
                    }
                }
            }

            if (parameters.ContainsKey("position"))
            {
                ChoosedLocation = (Position) parameters["position"];
                ChoosedAddress = await GetAdressFromLocation(ChoosedLocation.Value);
            }
            else
            {
                ChoosedLocation = null;
            }
        }

		public void OnNavigatingTo(NavigationParameters parameters)
		{
		}


        private async void GoToChooseLocation()
        {
            if (ChoosedLocation==null)
				await _navigationService.NavigateAsync("ChooseLocationMapPage?idChat="+_id);
            else
                await Send(ChoosedLocation.Value, TypedMessage);

        }

        private async Task Send(Position position, string message)
        {
            var msg = new Message()
            {
                Id = Guid.NewGuid().ToString(),
                Location = new Location(position.Latitude, position.Longitude),
                Owner = FakeData.FakeData.Me,
                Text = message
            };
            FakeData.FakeData.Chats.First(c=>c.Id.Equals(_id)).Messages.Add(msg);
			FakeData.FakeData.RaiseChangedChats();

            var listviewmsg = new ChatListViewModel()
            {
                Message = msg,
                Address = "Fetching address...",
                Alligment = (msg.Owner.Id.Equals(FakeData.FakeData.Me.Id))
                                ? LayoutOptions.End
                                : LayoutOptions.Start,
            };

            Messages.Add(listviewmsg);
            

            try
            {
                listviewmsg.Address = await GetAdressFromLocation(
                    new Position(listviewmsg.Message.Location.Latitude, listviewmsg.Message.Location.Longitude));
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to load address");
            }
        }

        private async Task<string> GetAdressFromLocation(Position position)
        {
            Geocoder geocoder = new Geocoder();
            var address = await geocoder.GetAddressesForPositionAsync(position);
            var addrList = address as IList<string> ?? address.ToList();
            return addrList.Any() ? addrList[0].Replace("\n",", ") : "Adress is missing...";
        }
	}
}
