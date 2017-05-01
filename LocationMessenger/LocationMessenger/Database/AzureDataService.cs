using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using LocationMessenger.Models;

namespace LocationMessenger
{
	public class AzureDataService : IAzureDataService
	{
		private const string UrlServer = "http://locationmessangerapp.azurewebsites.net";
		private const string LocalDBPath = "SyncStore6";

		private string _myId;

		private MobileServiceClient _mobileServiceClient { get; set; }

		private IMobileServiceSyncTable<ChatAzure> _chatTable { get; set; }
		private IMobileServiceSyncTable<MessageAzure> _messageTable { get; set; }
		private IMobileServiceSyncTable<UserAzure> _userTable { get; set; }
		private IMobileServiceSyncTable<ChatUsersAzure> _chatUsersTable { get; set; }

		private IList<ChatAzure> _chatList { get; set;}
		private IList<ChatUsersAzure> _chatUsersList { get; set;}

		public async Task Initialize(string myId)
		{
			_mobileServiceClient = new MobileServiceClient(UrlServer);

			var store = new MobileServiceSQLiteStore(LocalDBPath);

			store.DefineTable<ChatAzure>();
			store.DefineTable<MessageAzure>();
			store.DefineTable<UserAzure>();
			store.DefineTable<ChatUsersAzure>();

			await _mobileServiceClient.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

			_chatTable = _mobileServiceClient.GetSyncTable<ChatAzure>();
			_messageTable = _mobileServiceClient.GetSyncTable<MessageAzure>();
			_userTable = _mobileServiceClient.GetSyncTable<UserAzure>();
			_chatUsersTable = _mobileServiceClient.GetSyncTable<ChatUsersAzure>();

			_myId = myId;
		}

		private async Task SyncChatUsers()
		{
			IMobileServiceTableQuery<ChatUsersAzure> query = _chatUsersTable.Where(c => c.UserId==_myId);
			await _chatUsersTable.PullAsync("syncchatusers", query);
			await _mobileServiceClient.SyncContext.PushAsync();
			_chatUsersList = await _chatUsersTable.ToListAsync();
		}

		private async Task SyncChats()
		{
			await SyncChatUsers();

			var ids = new List<string>();
			foreach (var cu in _chatUsersList)
			{
				ids.Add(cu.ChatId);
			}

			IMobileServiceTableQuery<ChatAzure> query = _chatTable.Where(c => ids.Contains(c.Id));
			await _chatTable.PullAsync("syncchat", query);
			await _mobileServiceClient.SyncContext.PushAsync();
		}

		private async Task SyncMessages(bool syncChats = false)
		{
			if (syncChats || _chatList==null)
			{
				await GetChats();
			}

			var ids = new List<string>();
			foreach (var id in _chatList)
			{
				ids.Add(id.Id);
			}

			IMobileServiceTableQuery<MessageAzure> query = _messageTable.Where(m => ids.Contains(m.ChatId));
			await _messageTable.PullAsync("syncmessages", query);
			await _mobileServiceClient.SyncContext.PushAsync();
		}

		public async Task<IList<MessageAzure>> GetMessages(bool syncChats = false)
		{
			await SyncMessages(syncChats);
			return await _messageTable.ToListAsync();
		}

		public async Task AddMessage(Message msgModel, string chatId)
		{
			var msgAzure = new MessageAzure()
			{
				Longitude = msgModel.Location.Longitude,
				Latitude = msgModel.Location.Latitude,
				Text = msgModel.Text,
				OwnerId = msgModel.Owner.Id,
				ChatId = chatId
			};

			await _messageTable.InsertAsync(msgAzure);

			await SyncMessages();
		}

		public async Task<IList<ChatAzure>> GetChats()
		{
			await SyncChats();
			_chatList = await _chatTable.ToListAsync();
			return _chatList;
		}

		public async Task AddChat(string idFriend)
		{
			var chatId = Guid.NewGuid().ToString();
			var chat = new ChatAzure()
			{
				Id = chatId
			};
			await _chatTable.InsertAsync(chat);

			var userChats1 = new ChatUsersAzure()
			{
				UserId = _myId,
				ChatId = chatId
			};
			await _chatUsersTable.InsertAsync(userChats1);

			var userChats2 = new ChatUsersAzure()
			{
				UserId = idFriend,
				ChatId = chatId
			};
			await _chatUsersTable.InsertAsync(userChats2);

			await SyncChats();
		}
	}
}
