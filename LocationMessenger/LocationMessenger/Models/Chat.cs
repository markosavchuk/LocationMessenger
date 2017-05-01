using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationMessenger.Models
{
    public class Chat
    {
        public string Id { get; set; }
        public List<Person> Members { get; set; }
        public List<Message> Messages { get; set; }
		public bool Read { get; set; }
    }
}
