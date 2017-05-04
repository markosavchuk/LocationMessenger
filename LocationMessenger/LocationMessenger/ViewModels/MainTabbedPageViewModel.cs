using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Navigation;

namespace LocationMessenger.ViewModels
{
	class MainTabbedPageViewModel : BindableBase
	{
		private readonly IData _data;

		string _title = "Location Messenger";

		public string Title
		{
			get{ return _title; }
			set{ SetProperty(ref _title, value ); }
		}

		public MainTabbedPageViewModel(IData data)
		{
			_data = data;
		}

		public async Task InitializeData()
		{
			System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			await _data.Initialize();
			sw.Stop();
			System.Diagnostics.Debug.WriteLine(sw.Elapsed.ToString());
		}
	}
}
