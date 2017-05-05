using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocationMessenger.Views;
using Microsoft.Practices.Unity;
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
            await NavigationService.NavigateAsync("MainNavigationPage/MainTabbedPage");
        }

        protected override void RegisterTypes()
        {
			Container.RegisterType(typeof(IAzureDataService), typeof(AzureDataService), null, new ContainerControlledLifetimeManager());
			Container.RegisterType(typeof(IData), typeof(Data), null, new ContainerControlledLifetimeManager());

            Container.RegisterTypeForNavigation<MainTabbedPage>();
            Container.RegisterTypeForNavigation<HistoryPage>();
            Container.RegisterTypeForNavigation<MapPage>();
            Container.RegisterTypeForNavigation<ChatPage>();
            Container.RegisterTypeForNavigation<ChooseLocationMapPage>();
			Container.RegisterTypeForNavigation<MainNavigationPage>();
        }
    }
}
