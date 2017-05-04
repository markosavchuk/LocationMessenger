using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocationMessenger.Models;
using System.ComponentModel;
using static LocationMessenger.Models.Person.GenderEnum;

namespace LocationMessenger.FakeData
{
	public class FakeData
    {
        static FakeData()
        {
			
            Me = new Person()
            {
                Id = "myid233",
                Name = "Marko",
                Surname = "Savchuk",
                Gender = Male,
                Image = new Person.ImagePerson()
                {
                    Url = @"https://avatars.slack-edge.com/2016-11-19/107419585703_0941099a8ac1e69c84a3_original.jpg"
                }
            };

			Contacts = new ObservableCollection<Person>()
            {
				new Person()
				{
					Id = "7",
                    Name = "Rachel",
                    Surname = "Guiomar",
                    Gender = Female

				},
                new Person()
				{
					Id = "8",
                    Name = "Eliora",
                    Surname = "Khurshid",
                    Gender = Female

				},
                new Person()
				{
					Id = "9",
                    Name = "Violetta",
                    Surname = "Raluca",
                    Gender = Female

				},
                new Person()
				{	
					Id = "10",
                    Name = "Yamuna",
                    Surname = "Ogechi",
                    Gender = Female       
				},
                new Person()
                {
                    Id = "1",
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
                    Id = "2",
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
                    Id = "3",
                    Name = "Boguslav",
                    Surname = "Herodion",
                    Gender = Male,
                },
                new Person()
                {
                    Id = "4",
                    Name = "Paolino",
                    Surname = "Isamu",
                    Gender = Male
                },
                new Person()
                {
                    Id = "5",
                    Surname = "Maruf",
                    Name = "Souta",
                    Gender = Male
                },
                new Person()
                {
                    Id = "6",
                    Name = "Diana",
                    Surname = "Monika",
                    Gender = Female,
                    Image = new Person.ImagePerson()
                    {
                        Url = @"https://hips.hearstapps.com/hmg-prod.s3.amazonaws.com/images/maggie-lindemann-pretty-girl-1475155655.jpg?resize=200:*&crop=1xw:0.666564039408867xh;center,top"
                    }
                },
            
            };
        }

        public static Person Me;

		public static ObservableCollection<Person> Contacts;

	}
}
