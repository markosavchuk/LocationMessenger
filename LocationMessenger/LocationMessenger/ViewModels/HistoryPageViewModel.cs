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

        private Person _me;

        private string _title = "History";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private ObservableCollection<ChatListViewModel> _chats;
        public ObservableCollection<ChatListViewModel> Chats
        {
            get { return _chats; }
            set { SetProperty(ref _chats, value); }
        }

        public ChatListViewModel ChatSelected
        {
            set
            {
                if (value != null)
                {
                    NavigateToChat(value.Id);
                }
            }
        }

        public HistoryPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            _me = FakeData.FakeData.Me;

            _chats = new ObservableCollection<ChatListViewModel> ();
            foreach (var chat in FakeData.FakeData.Chats)
            {
                var member = chat.Members.FirstOrDefault(m => m != _me);
                Chats.Add(new ChatListViewModel()
                {
                    Id = chat.Id,
                    ChatName = (member.Name ?? "") + " " + (member.Surname ?? ""),
                    LastMessage = chat.Messages.Last() != null ? chat.Messages.Last().Text : "Chat is empty..."
                });
            }
        }

        private void NavigateToChat(string id)
        {
            _navigationService.NavigateAsync("ChatPage?id="+id);
        }
    }
}
