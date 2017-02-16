using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationMessenger.Models
{
    public class Message
    {
        public string Id { get; set; }
        public Person Owner { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool Sent { get; set; }
        public Location Location { get; set; }
    }
}
