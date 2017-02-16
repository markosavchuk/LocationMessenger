using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocationMessenger.ViewModels;
using Xamarin.Forms;

namespace LocationMessenger.Pages
{
    public partial class HistoryPage : ContentPage
    {
        private UserDateViewModel viewModel;

        public HistoryPage()
        {
            InitializeComponent();

            viewModel = new UserDateViewModel();
            BindingContext = viewModel;
        }
    }
}
