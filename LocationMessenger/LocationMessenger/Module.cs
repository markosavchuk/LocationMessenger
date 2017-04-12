using System;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Unity;
using Xamarin.Forms.Xaml;
using LocationMessenger.Views;

//[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace LocationMessenger
{
	public class Module
	{
		private readonly IUnityContainer _unityContainer;
		public Module(IUnityContainer unityContainer)
		{
			_unityContainer = unityContainer;
		}

		public void Initialize()
		{
			_unityContainer.RegisterTypeForNavigation<MainTabbedPage>();
			_unityContainer.RegisterTypeForNavigation<HistoryPage>();
			_unityContainer.RegisterTypeForNavigation<MapPage>();
			_unityContainer.RegisterTypeForNavigation<ChatPage>();
			_unityContainer.RegisterTypeForNavigation<ChooseLocationMapPage>();
			_unityContainer.RegisterTypeForNavigation<MainNavigationPage>();
			_unityContainer.RegisterTypeForNavigation<ChatPage>();
		}
	}
}
