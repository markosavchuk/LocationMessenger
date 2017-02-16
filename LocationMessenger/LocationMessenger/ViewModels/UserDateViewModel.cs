using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LocationMessenger.Models;
using static LocationMessenger.Models.Person.GenderEnum;
using LocationMessenger.FakeData;

namespace LocationMessenger.ViewModels
{
    public class UserDateViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Person> Contacts { get; set; }
        public ObservableCollection<ChatListViewModel> Chats { get; set; }
        public Person Me { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public UserDateViewModel()
        {
            Me = FakeData.FakeData.Me;
            Contacts = new ObservableCollection<Person>(FakeData.FakeData.Contacts);
            Chats = new ObservableCollection<ChatListViewModel>();
            /*for (int i = 0; i < 10; i++)
            {*/
                foreach (var chat in FakeData.FakeData.Chats)
                {
                    var member = chat.Members.FirstOrDefault(m => m != Me);
                    Chats.Add(new ChatListViewModel()
                    {
                        Id = chat.Id,
                        ChatName = (member.Name ?? "") + " " + (member.Surname ?? ""),
                        LastMessage = chat.Messages.Last() != null ? chat.Messages.Last().Text : "Chat is empty..."
                    });
                }
            //}
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
