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
        static FakeData()
        {
            Me = new Person()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Marko",
                Surname = "Savchuk",
                Gender = Male,
                Image = new Person.ImagePerson()
                {
                    Url = @"https://avatars.slack-edge.com/2016-11-19/107419585703_0941099a8ac1e69c84a3_original.jpg"
                }
            };

            Contacts = new List<Person>()
            {
                new Person()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Uthyr",
                    Surname = "Bennie",
                    Gender = Male,
                    Image = new Person.ImagePerson()
                    {
                        Url = @"https://media.npr.org/assets/img/2016/02/24/sam-sanders_sq-2592b60f365f3ef0a165172ef115b6199d61f985-s100-c85.jpg"
                    }
                },
                new Person()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Seneca",
                    Surname = "Pollux",
                    Gender = Male,
                    Image = new Person.ImagePerson()
                    {
                        Url = @"https://s-media-cache-ak0.pinimg.com/736x/c8/55/34/c8553420c95024c73fd21502430a14a2.jpg"
                    }
                },
                new Person()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Boguslav",
                    Surname = "Herodion",
                    Gender = Male,
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
                    Gender = Female,
                    Image = new Person.ImagePerson()
                    {
                        Url = @"https://hips.hearstapps.com/hmg-prod.s3.amazonaws.com/images/maggie-lindemann-pretty-girl-1475155655.jpg?resize=200:*&crop=1xw:0.666564039408867xh;center,top"
                    }
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

            Chats = new List<Chat>()
            {
                new Chat()
                {
                    Id = Guid.NewGuid().ToString(),
                    Members = new List<Person>() {Me, Contacts[1]},
                    Messages = new List<Message>()
                    {
                        new Message()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Date = DateTime.Parse("2016-02-08 14:40:52"),
                            Owner = Me,
                            Text = "Hi there",
                            Location = new Location(49.834813, 23.997578)
                        },
                        new Message()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Date = DateTime.Parse("2016-02-09 12:40:52"),
                            Owner = Contacts[1],
                            Text = String.Format("Glad to here you {0}",Contacts[0].Name),
                            Location = new Location(49.841511, 24.029507)
                        },
                        new Message()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Date = DateTime.Parse("2016-02-09 18:42:52"),
                            Owner = Contacts[1],
                            Text = String.Format("Hope you will catch this message :)"),
                            Location = new Location(49.839380, 24.019636)
                        }
                    }
                },
                new Chat()
                {
                    Id = Guid.NewGuid().ToString(),
                    Members = new List<Person>() {Contacts[5], Me},
                    Messages = new List<Message>()
                    {
                        new Message()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Date = DateTime.Parse("2016-02-08 14:40:52"),
                            Owner = Contacts[5],
                            Text = "Heyy, finally you got here!!",
                            Location = new Location(49.823878, 24.024872)
                        },
                        new Message()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Date = DateTime.Parse("2016-02-09 12:40:52"),
                            Owner = Contacts[5],
                            Text = String.Format("Call me here plese"),
                            Location = new Location(49.832903, 24.055900)
                        },
                        new Message()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Date = DateTime.Parse("2016-02-09 18:42:52"),
                            Owner = Me,
                            Text = String.Format("See you tomorrow here :)"),
                            Location = new Location(49.837027, 24.001912)
                        }
                    }
                }
            };
        }

        // actually not so fake :)
        public static Person Me;

        public static List<Person> Contacts;

        public static List<Chat> Chats;
    }
}
