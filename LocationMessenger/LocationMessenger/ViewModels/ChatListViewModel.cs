using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocationMessenger.Models;

namespace LocationMessenger.ViewModels
{
    public class ChatListViewModel
    {
        public string Id { get; set; }
        public string LastMessage { get; set; }
        public string ChatName { get; set; }
    }
}
