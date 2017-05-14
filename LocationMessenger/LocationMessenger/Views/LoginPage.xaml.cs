using LocationMessenger.ViewModels;
using Xamarin.Forms;

namespace LocationMessenger.Views
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage()
		{
			NavigationPage.SetHasNavigationBar(this, false);

			InitializeComponent();
		}

		async void OnNavigating(object sender, Xamarin.Forms.WebNavigatingEventArgs e)
		{
			var viewmodel = BindingContext as LoginPageViewModel;
			await viewmodel.Login(e.Url);
		}
	}
}

