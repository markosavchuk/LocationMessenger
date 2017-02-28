using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LocationMessenger.Models;
using LocationMessenger.ViewModels.ListViews;

namespace LocationMessenger.ViewModels
{
    public class HistoryPageViewModel : BindableBase
    {
        private string _title = "History";
        private ObservableCollection<ChatListViewModel> _chats;
        private Person _me;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<ChatListViewModel> Chats
        {
            get { return _chats; }
            set { SetProperty(ref _chats, value); }
        }

        public HistoryPageViewModel()
        {
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
    }
}
