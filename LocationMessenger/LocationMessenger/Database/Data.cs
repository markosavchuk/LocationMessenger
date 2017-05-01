using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LocationMessenger.Models;

namespace LocationMessenger
{
	public class Data : IData
	{
		private IAzureDataService _azureService;

		public List<Person> Contacts { get; set; }
		public List<Chat> Chats { get; set; }
		public static Person Me;
		public static event EventHandler ChatsChaged;

		public Data(IAzureDataService azureService)
		{
			_azureService = azureService;

			System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			Initialize();
			sw.Stop();
		}

		public async Task Initialize()
		{
			//TODO retrieve from facebook my id info about friends
			Me = FakeData.FakeData.Me;
			Contacts = new List<Person>(FakeData.FakeData.Contacts);

			await _azureService.Initialize(Me.Id);

			var chatsAzure = await _azureService.GetChats();
			var messageAzure = await _azureService.GetMessages();
		}

		public static void RaiseChangedChats()
		{
			if (ChatsChaged != null)
				ChatsChaged(null, EventArgs.Empty);
		}
	}
}
