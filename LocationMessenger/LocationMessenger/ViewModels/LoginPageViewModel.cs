using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Prism.Navigation;
using Plugin.Settings;
using System.Threading.Tasks;

namespace LocationMessenger.ViewModels
{
	public class LoginPageViewModel : BindableBase
	{		
		private const string appId = "117270602175132";

		private INavigationService _navigationService;

		private bool _auth = false;

		public string LoginUrl
		{
			get 
			{  
				return @"https://www.facebook.com/v2.9/dialog/oauth?client_id="+
					appId +
					"&display=popup" +
					"&response_type=token" +
					"&scope=user_friends,public_profile" +
					"&auth_type=https"+
					"&redirect_uri=https://www.facebook.com/connect/login_success.html";
			}
		}

		public LoginPageViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
		}

		public async Task Login(string url)
		{
			var token = ExstractToken(url);

			if (!String.IsNullOrEmpty(token) && !_auth)
			{
				var expiredTime = ExtractExpiredDate(url);

				CrossSettings.Current.AddOrUpdateValue(Data.FacebookTokenSettings, token);
				if (expiredTime.CompareTo(new DateTime()) != 0)
				{
					CrossSettings.Current.AddOrUpdateValue(Data.FacebookExpiredTokenSettings, expiredTime);
					await _navigationService.NavigateAsync("MainTabbedPage");
				}
			}
		}

		private string ExstractToken(string url)
		{
			var str1 = "access_token=";
			var str2 = "&expires_in=";

			var start = url.IndexOf(str1, StringComparison.Ordinal) + str1.Length;
			var end = url.IndexOf(str2, StringComparison.Ordinal);
			if (start != -1 && end != -1)
				return url.Substring(start, end - start);
			else
				return "";
		}

		private DateTime ExtractExpiredDate(string url)
		{
			var str = "&expires_in=";
			var start = url.IndexOf(str, StringComparison.Ordinal) + str.Length;
			if (start != -1)
			{
				var strTime = url.Substring(start, url.Length - start);
				int second;
				var succ = Int32.TryParse(strTime, out second);
				if (succ)
				{
					var span = TimeSpan.FromSeconds(second);
					return DateTime.Now.Add(span);
				}
				else
					return new DateTime();
			}
			else
				return new DateTime();
		}
	}
}
