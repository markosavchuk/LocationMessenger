using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LocationMessenger.Models;
using LocationMessenger.ViewModels.ListViews;
using Prism.Navigation;

namespace LocationMessenger.ViewModels
{
	public class HistoryPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
		private readonly IData _data;

        private Person _me;

        private string _title = "History";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private ObservableCollection<HistoryListViewModel> _chats;
        public ObservableCollection<HistoryListViewModel> Chats
        {
            get { return _chats; }
            set { SetProperty(ref _chats, value); }
        }

        public HistoryListViewModel ChatSelected
        {
            set
            {
                if (value != null)
                {
                    NavigateToChat(value.Id);
                }
            }
        }

		public HistoryPageViewModel(INavigationService navigationService, IData data)
        {
            _navigationService = navigationService;
			_data = data;

			Chats = new ObservableCollection<HistoryListViewModel>();

			_me = _data.Me;

			_data.DataReady += (sender, e) => FillChats();
			_data.ChatsChanged += (sender, e) => FillChats();
        }

        private void NavigateToChat(string id)
        {
			_navigationService.NavigateAsync("ChatPage?idChat=" + id, useModalNavigation:false);
        }

		private void FillChats()
		{
			Chats.Clear();
			
			foreach (var chat in _data.Chats)
			{
				var member = chat.Members.FirstOrDefault(m => m != _me);
				if (member == null)
					continue;
				
				Chats.Add(new HistoryListViewModel()
				{
					Id = chat.Id,
					ChatName = (member.Name ?? "") + " " + (member.Surname ?? ""),
					LastMessage = chat.Messages.LastOrDefault() != null ? chat.Messages.Last().Text : "Start chat..."
				});
			}
		}
	}
}
