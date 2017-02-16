using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationMessenger.Models
{
    public class Person
    {
        public enum GenderEnum
        {
            Male,
            Female
        }

        public class ImagePerson
        {
            public string Id { get; set; }
            public string Url { get; set; }
            public byte[] ImageByte { get; set; }
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public GenderEnum Gender { get; set; }
        public ImagePerson Image { get; set; }
    }
}
