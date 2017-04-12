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
		string _title = "Location Messenger";

		public string Title
		{
			get{ return _title; }
			set{ SetProperty(ref _title, value ); }
		}
	}
}
