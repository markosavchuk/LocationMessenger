﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using LocationMessenger.ViewModels;
using Plugin.Geolocator;
using Xamarin.Forms;

namespace LocationMessenger.Views
{
    public partial class MainTabbedPage : TabbedPage
    {
		bool _initialized = false;

        public MainTabbedPage()
        {
			NavigationPage.SetHasNavigationBar(this, false);

			InitializeComponent();
        }

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			if (!_initialized)
			{
				var _viewmodel = BindingContext as MainTabbedPageViewModel;
				await _viewmodel.InitializeData();
				_initialized = true;
			}
		}
	}
}
