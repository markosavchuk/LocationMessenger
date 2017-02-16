using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocationMessenger.Models;
using static LocationMessenger.Models.Person.GenderEnum;

namespace LocationMessenger.FakeData
{
    public static class FakeData
    {
        // actually not so fake :)
        public static Person Me => new Person()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Marko",
            Surname = "Savchuk",
            Gender = Male
        };

        public static List<Person> Contacts => new List<Person>()
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

        public static List<Chat> Chats => new List<Chat>()
        {
            new Chat()
            {
                Id = Guid.NewGuid().ToString(),
                Members = new List<Person>() {Contacts[0], Contacts[1]},
                Messages = new List<Message>()
                {
                    new Message()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Date = DateTime.Parse("2016-02-08 14:40:52"),
                        Owner = Contacts[0],
                        Text = "Hi there"
                    },
                    new Message()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Date = DateTime.Parse("2016-02-09 12:40:52"),
                        Owner = Contacts[1],
                        Text = String.Format("Glad to here you {0}",Contacts[0].Name)
                    },
                    new Message()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Date = DateTime.Parse("2016-02-09 18:42:52"),
                        Owner = Contacts[1],
                        Text = String.Format("Hope you will catch this message :)")
                    }
                }
            },
            new Chat()
            {
                Id = Guid.NewGuid().ToString(),
                Members = new List<Person>() {Contacts[2], Contacts[3]},
                Messages = new List<Message>()
                {
                    new Message()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Date = DateTime.Parse("2016-02-08 14:40:52"),
                        Owner = Contacts[0],
                        Text = "Heyy, finally you got here!!"
                    },
                    new Message()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Date = DateTime.Parse("2016-02-09 12:40:52"),
                        Owner = Contacts[0],
                        Text = String.Format("Call me here plese")
                    },
                    new Message()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Date = DateTime.Parse("2016-02-09 18:42:52"),
                        Owner = Contacts[1],
                        Text = String.Format("See you tomorrow here :)")
                    }
                }
            }
        };
    }
}
