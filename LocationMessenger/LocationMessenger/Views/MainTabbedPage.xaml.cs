using Xamarin.Forms;

namespace LocationMessenger.Views
{
    public partial class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage()
        {
			NavigationPage.SetHasNavigationBar(this, false);

			InitializeComponent();
        }
    }
}
