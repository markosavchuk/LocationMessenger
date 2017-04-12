using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocationMessenger.Models;
using Prism.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace LocationMessenger.ViewModels.ListViews
{
    public class ChatListViewModel : BindableBase
    {
        private Message _message;
        private string _address;
        private LayoutOptions _alligment;

        public Message Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }

        public LayoutOptions Alligment
        {
            get { return _alligment; }
            set { SetProperty(ref _alligment, value); }
        }
    }
}
