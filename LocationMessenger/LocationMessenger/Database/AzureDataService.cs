using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using LocationMessenger.Models;
using System.Linq;
using Xamarin.Forms.Maps;

namespace LocationMessenger
{
	public class AzureDataService : IAzureDataService
	{
		private const string UrlServer = "http://locationmessangerapp.azurewebsites.net";
		private const string LocalDBPath = "SyncStore31";

		private string _myId;
		private string _myIdQuery;

		private MobileServiceClient _mobileServiceClient { get; set; }
		private ILocationService _locationService;

		private IMobileServiceSyncTable<ChatAzure> _chatTable { get; set; }
		private IMobileServiceSyncTable<MessageAzure> _messageTable { get; set; }
		private IMobileServiceSyncTable<UserAzure> _userTable { get; set; }
		private IMobileServiceSyncTable<ChatUsersAzure> _chatUsersTableMyConnections { get; set; }
		private IMobileServiceSyncTable<ChatUsersAzure> _chatUsersTableFriendsConnections { get; set; }

		private IList<ChatAzure> _chatList { get; set;}
		private IList<ChatUsersAzure> _chatUsersListMyConn { get; set; }
		private IList<ChatUsersAzure> _chatUsersListFriendsConn { get; set; }
		private IList<MessageAzure> _messageList { get; set; }

		public AzureDataService(ILocationService location)
		{
			_locationService = location;

			location.ChangedLocation+= async (sender, e) => 
			{
				await CheckMessageInArea(true);
			};
		}

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
			_chatUsersTableMyConnections = _mobileServiceClient.GetSyncTable<ChatUsersAzure>();
			_chatUsersTableFriendsConnections = _mobileServiceClient.GetSyncTable<ChatUsersAzure>();

			_myId = myId;
			_myIdQuery = (_myId.Length > 10) ? _myId.Substring(0,10) : _myId;
		}

		private async Task SyncChatUsersMyConections(bool useOffline = false)
		{

			if (!useOffline)
			{
				IMobileServiceTableQuery<ChatUsersAzure> query = _chatUsersTableMyConnections
					.CreateQuery()
					.Where(c => c.UserId ==_myId);

				try
				{
					await _chatUsersTableMyConnections.PullAsync("sycnMyConn" + _myIdQuery, query);
				}
				catch (Exception e)
				{

				}
			}

			_chatUsersListMyConn = await _chatUsersTableMyConnections
				.Where(c => c.UserId==_myId)
				.ToListAsync();

			_chatUsersListMyConn = _chatUsersListMyConn
				.Where(c => c.UserId == _myId)
				.ToList();
		}

		private async Task SycnChatUsersFriendsConnections(bool useOffline = false)
		{
			if (_chatList == null)
				return;
			
			var ids = new List<string>();
			foreach (var chat in _chatList)
			{
				ids.Add(chat.Id);
			}

			if (!useOffline)
			{

				IMobileServiceTableQuery<ChatUsersAzure> query = _chatUsersTableMyConnections
						.CreateQuery()
						.Where(c => ids.Contains(c.ChatId));
				
				try
				{
					await _chatUsersTableFriendsConnections.PullAsync("syncFriends" + _myIdQuery, query);
				}
				catch (Exception e)
				{

				}
			}

			_chatUsersListFriendsConn = await _chatUsersTableFriendsConnections
				.Where(c => ids.Contains(c.ChatId))
				.ToListAsync();

			_chatUsersListFriendsConn = _chatUsersListFriendsConn
							.Where(c => ids.Contains(c.ChatId))
							.ToList();
		}

		private async Task SyncChats(bool useOffline = false)
		{
			await SyncChatUsersMyConections(useOffline);
			
			var ids = new List<string>();
			foreach (var cu in _chatUsersListMyConn)
			{
				ids.Add(cu.ChatId);
			}

			if (!useOffline)
			{
				IMobileServiceTableQuery<ChatAzure> query = _chatTable
						.CreateQuery()
						.Where(c => ids.Contains(c.Id));
				try
				{
					await _chatTable.PullAsync("syncChat" + _myIdQuery, query);
				}
				catch (Exception ex)
				{

				}
			}

			_chatList = await _chatTable
				.Where(c => ids.Contains(c.Id))
				.ToListAsync();

			_chatList = _chatList
							.Where(c => ids.Contains(c.Id))
							.ToList();
		}

		private async Task SyncMessages(bool syncChats = false, bool useOffline = false)
		{
			if (syncChats || _chatList==null)
			{
				await SyncChats(useOffline);
			}

			var ids = new List<string>();
			foreach (var id in _chatList)
			{
				ids.Add(id.Id);
			}


			if (!useOffline)
			{
				IMobileServiceTableQuery<MessageAzure> query = _messageTable
						.CreateQuery()
						.Where(m => ids.Contains(m.ChatId));

				try
				{
					await _messageTable.PullAsync("syncMsg" + _myIdQuery, query);
				}
				catch (Exception e)
				{

				}
			}

			_messageList = await _messageTable
				.Where(m => ids.Contains(m.ChatId))
				.ToListAsync();

			await CheckMessageInArea();

			_messageList = _messageList
							.Where(m => ids.Contains(m.ChatId))
							.ToList();
		}

		private async Task CheckMessageInArea(bool notification = false)
		{
			foreach (var msg in _messageList)
			{
				if (!msg.Visible)
				{
					if (_locationService.MessageInArea(new Position(msg.Latitude, msg.Longitude)))
					{
						msg.Visible = true;
						await _messageTable.UpdateAsync(msg);

						if (notification)
						{
							//notify changes!
						}
					}
				}
			}
		}

		public async Task<IList<MessageAzure>> GetMessages(bool syncChats = false, bool useOffline = false)
		{
			await SyncMessages(syncChats, useOffline);
			return _messageList.Where(m=>m.Visible)
				               .ToList();
		}

		public async Task SendMessage(Message msgModel, string chatId)
		{
			var msgAzure = new MessageAzure()
			{
				Id = msgModel.Id,
				Longitude = msgModel.Location.Longitude,
				Latitude = msgModel.Location.Latitude,
				Text = msgModel.Text,
				OwnerId = msgModel.Owner.Id,
				ChatId = chatId,
				Visible = true
			};

			await _messageTable.InsertAsync(msgAzure);

			foreach (var con in _chatUsersListFriendsConn)
			{
				if (con.ChatId.Equals(chatId) && !con.UserId.Equals(_myId))
				{
					con.Read = false;
					await _chatUsersTableFriendsConnections.UpdateAsync(con);
				}
			}

			await SyncMessages();
		}

		public async Task<Tuple<IList<ChatAzure>,IList<ChatUsersAzure>>> GetChats(bool useOffline = false)
		{
			await SyncChats(useOffline);

			await SycnChatUsersFriendsConnections(useOffline);

			return new Tuple<IList<ChatAzure>, IList<ChatUsersAzure>>
				(_chatList, _chatUsersListFriendsConn);
		}

		public async Task<Tuple<IList<ChatAzure>,IList<ChatUsersAzure>>> AddChat(string idFriend)
		{
			var chatId = Guid.NewGuid().ToString();

			var userChats1 = new ChatUsersAzure()
			{
				UserId = _myId,
				ChatId = chatId,
				Read = true
			};
			await _chatUsersTableMyConnections.InsertAsync(userChats1);

			var userChats2 = new ChatUsersAzure()
			{
				UserId = idFriend,
				ChatId = chatId,
				Read = false
			};
			await _chatUsersTableMyConnections.InsertAsync(userChats2);

			var chat = new ChatAzure()
			{
				Id = chatId
			};
			await _chatTable.InsertAsync(chat);

			return await GetChats();
		}

		public async Task ReadChat(string idChat)
		{
			var userChat =_chatUsersListMyConn.FirstOrDefault(c=>c.ChatId.Equals(idChat));
			if (userChat!=null)
			{
				userChat.Read = true;
				await _chatUsersTableMyConnections.UpdateAsync(userChat);
			}
		}
	}
}
