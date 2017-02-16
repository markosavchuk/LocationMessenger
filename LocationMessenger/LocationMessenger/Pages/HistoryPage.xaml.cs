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

            /*ObservableCollection<Tuple<string, string>> veggies;

            veggies = new ObservableCollection<Tuple<string, string>>();
            veggies.Add(new Tuple<string, string>("Item1","Type1"));
            veggies.Add(new Tuple<string, string>("Item2", "Type2"));
            veggies.Add(new Tuple<string, string>("Item3", "Type3"));
            lstView.ItemsSource = veggies;*/
        }
    }
}
