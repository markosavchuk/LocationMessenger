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

namespace LocationMessenger.ViewModels
{
    public class UserDateViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public UserDateViewModel()
        {
            FakeData fakeData = new FakeData();
            Contacts = new ObservableCollection<Person>(fakeData.Contacts);
        }

        public ObservableCollection<Person> Contacts { get; set; }
        public string Test { get { return "Test"; } }
        

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // fake data tmp
    public class FakeData
    {
        public List<Person> Contacts => new List<Person>()
        {
            new Person()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Uthyr",
                Surname = "Bennie",
                Gender = Male
            },
            new Person()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Seneca",
                Surname = "Pollux",
                Gender = Male
            },
            new Person()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Boguslav",
                Surname = "Herodion",
                Gender = Male
            },
            new Person()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Paolino",
                Surname = "Isamu",
                Gender = Male
            },
            new Person()
            {
                Id = Guid.NewGuid().ToString(),
                Surname = "Maruf",
                Name = "Souta",
                Gender = Male
            },
            new Person()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Diana",
                Surname = "Monika",
                Gender = Female
            },
            new Person()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Rachel",
                Surname = "Guiomar",
                Gender = Female
            },
            new Person()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Eliora",
                Surname = "Khurshid",
                Gender = Female
            },
            new Person()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Violetta",
                Surname = "Raluca",
                Gender = Female
            },
            new Person()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Yamuna",
                Surname = "Ogechi",
                Gender = Female
            }
        };
    }
}
