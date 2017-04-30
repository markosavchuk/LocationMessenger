using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LocationMessenger.Views
{
	public partial class HistoryPage : ContentPage
	{
		public HistoryPage()
		{
			InitializeComponent();
		}

		void ChatSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			lstView.SelectedItem = null;
		}
	}
}
