using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using LocationMessenger.Views.CustomControls;
using Prism.Navigation;
using Xamarin.Forms.Maps;

namespace LocationMessenger.ViewModels
{
    public class ChooseLocationMapPageViewModel : BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private string _id;

        public EventHandler<MapTapEventArgs> Tapped { get; set; }

        public ChooseLocationMapPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            Tapped += (sender, args) => ChooseLocation(args.Position);
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
                    _id = id;
                }
            }
        }

		public void OnNavigatingTo(NavigationParameters parameters)
		{
		}

        private void ChooseLocation(Position position)
        {
			
            var param = new NavigationParameters {{"position", position}};
			//_navigationService.GoBackAsync(param, true);
			_navigationService.NavigateAsync($"ChatPage?idChat={_id}", param, useModalNavigation: false);
        }
	}
}
