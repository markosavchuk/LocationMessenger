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
		private readonly IData _data;

		private string _idChat;
		private string _chooseLocationButtonTitle = "Choose location";
		private ObservableCollection<ChatListViewModel> _messages;
        private Position _choosedLocation;
        private string _typedMessage;
        private string _choosedAddress = "Address for message...";
		private string _title = "Chat Page";
		private bool _isChoosedLocation = false;
		private bool _isNotChoosedLocation = true;
		private string _sendButtonTitle = "Send";

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

		public string ChooseLocationButtonTitle
        {
            get { return _chooseLocationButtonTitle; }
            set { SetProperty(ref _chooseLocationButtonTitle, value); }
        }

		public bool IsChoosedLocation
		{
			get { return _isChoosedLocation; }
			set { 
					SetProperty(ref _isChoosedLocation, value);
					IsNotChoosedLocation = !_isChoosedLocation;
				}
		}

		public bool IsNotChoosedLocation
		{
			get { return _isNotChoosedLocation; }
			set { SetProperty(ref _isNotChoosedLocation, value); }
		}

		public string SendButtonTitle 
		{
			get { return _sendButtonTitle;}
			set { SetProperty(ref _sendButtonTitle, value); }
		}

		public Position ChoosedLocation
        {
            get { return _choosedLocation; }
            set { SetProperty(ref _choosedLocation, value); }
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

		public ChatListViewModel SelectedMessage
		{
			set 
			{
				if (value!=null)
				{
					if (value.IsSelected)
					{
						value.IsSelected = false;
						return;
					}

					foreach (var message in Messages)
					{
						if (message.IsSelected)
						{
							message.IsSelected = false;
						}
					}
					value.IsSelected = true;

					if (Messages.LastOrDefault().Equals(value))
					{
						if (RefreshScrollDown!=null)
							RefreshScrollDown(this, EventArgs.Empty);
					}
				}
			}
		}

		public DelegateCommand ChooseLocationClick { get; set; }
		public DelegateCommand SendClick { get; set; }
		public event EventHandler RefreshScrollDown;

		public ChatPageViewModel(INavigationService navigationService, IData data)
        {
            _navigationService = navigationService;
			_data = data;

            Messages = new ObservableCollection<ChatListViewModel>();
            ChooseLocationClick = new DelegateCommand(ChooseLocation);
			SendClick = new DelegateCommand(async ()=>await Send(ChoosedLocation, TypedMessage));
        }
       
        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("idChat"))
            {
                var id = parameters["idChat"] as string;
				if (id != null && !IsChoosedLocation)
                {
                    _idChat = id;

					SetTitle();
                }
            }

			await FillMessages();

            if (parameters.ContainsKey("position"))
            {
                ChoosedLocation = (Position) parameters["position"];
				IsChoosedLocation = true;
                ChoosedAddress = await GetAdressFromLocation(ChoosedLocation);
            }
            else
            {
				IsChoosedLocation = false;
            }

			if (RefreshScrollDown != null)
                RefreshScrollDown(this, EventArgs.Empty);

			await _data.ReadChat(_idChat);
        }

		public void OnNavigatingTo(NavigationParameters parameters)
		{

		}

		private async Task FillMessages()
		{
			Messages = new ObservableCollection<ChatListViewModel>();

			var messages = _data.Chats.FirstOrDefault(c => c.Id.Equals(_idChat)).Messages;
            foreach (var msg in messages)
            {
                Messages.Add(new ChatListViewModel()
				{
					Message = msg,
                    Address = "Fetching address...",
					Alligment = (msg.Owner.Id.Equals(_data.Me.Id))
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
                catch
                {
                    Debug.WriteLine("Failed to load address");   
                }
            }
		}

		private void SetTitle()
		{
			var members = _data.Chats.FirstOrDefault(c => c.Id.Equals(_idChat)).Members;
			if (!members[0].Equals(_data.Me))
			{
				Title = (members[0].Name ?? "") + " " + (members[0].Surname ?? "");
			}
			else
			{
				Title = (members[1].Name ?? "") + " " + (members[1].Surname ?? "");
			}
		}

		private async void ChooseLocation()
        {
			await _navigationService.NavigateAsync("ChooseLocationMapPage?idChat="+_idChat);
        }

        private async Task Send(Position position, string message)
        {
            var msg = new Message()
            {
                Id = Guid.NewGuid().ToString(),
                Location = new Location(position.Latitude, position.Longitude),
				Owner = _data.Me,
                Text = message,
				Date = DateTime.Now
            };


			var listviewmsg = new ChatListViewModel()
			{
				Message = msg,
				Address = "Fetching address...",
				Alligment = (msg.Owner.Id.Equals(_data.Me.Id))
								? LayoutOptions.End
								: LayoutOptions.Start,
				IsSelected = true
            };

            Messages.Add(listviewmsg);

			if (RefreshScrollDown != null)
                RefreshScrollDown(this, EventArgs.Empty);

            try
            {
                listviewmsg.Address = await GetAdressFromLocation(
                    new Position(listviewmsg.Message.Location.Latitude, listviewmsg.Message.Location.Longitude));
            }
            catch
            {
                Debug.WriteLine("Failed to load address");
            }

			TypedMessage = "";
			ChoosedLocation = new Position();
			IsChoosedLocation = false;

			await _data.SendMessage(msg, _idChat);
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
