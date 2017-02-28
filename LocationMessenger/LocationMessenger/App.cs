using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocationMessenger.Views;
using Prism.Unity;
using Xamarin.Forms;

namespace LocationMessenger
{
    public class App : PrismApplication
    {
        /*public App()
        {
            //InitializeComponent();

            MainPage = new Views.MapPage();
        }*/
        protected override void OnInitialized()
        {
            NavigationService.NavigateAsync("MainTabbedPage");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<MainTabbedPage>();
            Container.RegisterTypeForNavigation<HistoryPage>();
            Container.RegisterTypeForNavigation<MapPage>();
        }
    }
}
