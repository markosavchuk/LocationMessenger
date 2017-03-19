using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;

namespace LocationMessenger.ViewModels
{
    public class ChatPageViewModel : BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private string _id;
        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public ChatPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("idChat"))
            {
                var id = parameters["idChat"] as string;
                if (id != null)
                {
                    Id = id;
                }
            }
        }
    }
}
