using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LocationMessenger.Models;
using System.Linq;

namespace LocationMessenger
{
	public class Data : IData
	{
		private IAzureDataService _azureService;

		private bool _initialized;

		private IList<MessageAzure> _messageAzure;

		public List<Person> Contacts { get; set; }
		public List<Chat> Chats { get; set; }
		public Person Me { get; set; }

		public event EventHandler ChatsChanged;
		public event EventHandler DataReady;

		public Data(IAzureDataService azureService)
		{
			_azureService = azureService;
		}

		public async Task Initialize()
		{
			//TODO retrieve from facebook my id and info about friends
			Me = FakeData.FakeData.Me;
			Contacts = new List<Person>(FakeData.FakeData.Contacts);

			await _azureService.Initialize(Me.Id);

			var chatsAzure = await _azureService.GetChats(useOffline: true);
			_messageAzure = await _azureService.GetMessages(useOffline: true);

			UpdateChats(chatsAzure, _messageAzure);

			if (DataReady != null)
                DataReady(this, EventArgs.Empty);

			_initialized = true;

			chatsAzure = await _azureService.GetChats();
			_messageAzure = await _azureService.GetMessages();

			UpdateChats(chatsAzure, _messageAzure);

			RaiseChangedChats();

			await CreateChatsWithFriends();
		}

		private void UpdateChats(Tuple<IList<ChatAzure>,IList<ChatUsersAzure>> chatsAzure, IList<MessageAzure> messageAzure)
		{
			Chats = new List<Chat>();
			foreach (var chat in chatsAzure.Item1)
			{
				var members = new List<Person>();
				foreach (var userId in chatsAzure.Item2.Where(u=>u.ChatId.Equals(chat.Id)).Select(u=>u.UserId))
				{
					if (Contacts.FirstOrDefault(c=>c.Id.Equals(userId))!=null)
					{
						members.Add(Contacts.FirstOrDefault(c=>c.Id.Equals(userId)));
					}
				}

				var messages = new List<Message>();
				foreach (var msg in messageAzure.Where(m=>m.ChatId.Equals(chat.Id)))
				{
					messages.Add(new Message()
					{
						Id = msg.Id,
						Owner = (Me.Id.Equals(msg.OwnerId)) ? Me : Contacts.Find(c => c.Id.Equals(msg.OwnerId)),
						Text = msg.Text,
						Date = msg.CreatedAt,
						Location = new Location(msg.Latitude, msg.Longitude)
					});
				}

				Chats.Add(new Chat()
				{
					Id = chat.Id,
					Members = members,
					Messages = messages.OrderBy(m=>m.Date).ToList(),
					Read = chatsAzure.Item2.Where(u => u.ChatId.Equals(chat.Id))
									 .First(u => u.UserId.Equals(Me.Id)).Read
				});
			}

			SortChats();
		}

		private async Task CreateChatsWithFriends()
		{
			foreach (var contact in Contacts)
			{
				if (!Chats.Any(c=>c.Members.Any(m=>m.Id.Equals(contact.Id))))
				{
					var chatsAzure = await _azureService.AddChat(contact.Id);

					UpdateChats(chatsAzure, _messageAzure);

					RaiseChangedChats();
				}
			}
		}

		public async Task SendMessage(Message msg, string chatId)
		{
			Chats.First(c=>c.Id.Equals(chatId)).Messages.Add(msg);

			SortChats();

			RaiseChangedChats();

			await _azureService.SendMessage(msg, chatId);
		}

		public async Task ReadChat(string idChat)
		{
			var chat = Chats.FirstOrDefault(c=>c.Id.Equals(idChat));
			if (chat!=null)
			{
				chat.Read = true;
				RaiseChangedChats();
			}
			await _azureService.ReadChat(idChat);
		}

		public void RaiseChangedChats()
		{
			if (ChatsChanged != null)
				ChatsChanged(null, EventArgs.Empty);
		}

		private void SortChats()
		{
			Chats = Chats.OrderByDescending(c=>c.Messages.Select(m=>m.Date).LastOrDefault()).ToList();
		}
	}
}
