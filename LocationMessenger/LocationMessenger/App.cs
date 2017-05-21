using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocationMessenger.Views;
using Microsoft.Practices.Unity;
using Plugin.Geolocator;
using Plugin.Settings;
using Prism.Modularity;
using Prism.Unity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LocationMessenger
{
    public class App : PrismApplication
    {
        protected async override void OnInitialized()
        {
			if (CrossSettings.Current.Contains(Data.FacebookTokenSettings)
				&& CrossSettings.Current.Contains(Data.FacebookExpiredTokenSettings))
			{
				if (CrossSettings.Current.GetValueOrDefault<DateTime>(Data.FacebookExpiredTokenSettings)
					.CompareTo(DateTime.Now) <= -1)
				{
					await NavigationService.NavigateAsync("MainNavigationPage/LoginPage");
				}
				else
				{
					await NavigationService.NavigateAsync("MainNavigationPage/MainTabbedPage");
				}
			}
			else
			{
				await NavigationService.NavigateAsync("MainNavigationPage/LoginPage");
			}
		}

        protected override void RegisterTypes()
        {
			Container.RegisterType(typeof(IAzureDataService), typeof(AzureDataService), null, new ContainerControlledLifetimeManager());
			Container.RegisterType(typeof(IData), typeof(Data), null, new ContainerControlledLifetimeManager());
			Container.RegisterType(typeof(ILocationService), typeof(LocationService), new ContainerControlledLifetimeManager());

            Container.RegisterTypeForNavigation<MainTabbedPage>();
            Container.RegisterTypeForNavigation<HistoryPage>();
            Container.RegisterTypeForNavigation<MapPage>();
            Container.RegisterTypeForNavigation<ChatPage>();
            Container.RegisterTypeForNavigation<ChooseLocationMapPage>();
			Container.RegisterTypeForNavigation<MainNavigationPage>();
			Container.RegisterTypeForNavigation<LoginPage>();
        }
    }
}
