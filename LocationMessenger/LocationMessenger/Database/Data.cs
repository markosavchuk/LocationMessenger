using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LocationMessenger.Models;
using System.Linq;
using System.Net.Http;
using Plugin.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;

namespace LocationMessenger
{
	public class Data : IData
	{
		private IAzureDataService _azureService;
		private ILocationService _locationService;

		private bool _initialized;

		private IList<MessageAzure> _messageAzure;
		private Tuple<IList<ChatAzure>, IList<ChatUsersAzure>> _chatsAzure;

		public static string FacebookTokenSettings = "FacebookTokenSettings";
		public static string FacebookExpiredTokenSettings = "FacebookExpiredTokenSettings";

		public List<Person> Contacts { get; set; }
		public List<Chat> Chats { get; set; }
		public Person Me { get; set; }

		public event EventHandler ChatsChanged;
		public event EventHandler DataReady;

		public Data(IAzureDataService azureService, ILocationService location)
		{
			_azureService = azureService;
			_locationService = location;
		}

		public async Task Initialize()
		{
			if (_initialized)
				return;
			
			if (CrossSettings.Current.Contains(FacebookTokenSettings))
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					await GetFacebookInfo(CrossSettings.Current.GetValueOrDefault<string>(FacebookTokenSettings));
				}
			}

			if (Me==null || String.IsNullOrEmpty(Me.Id))
				return;

			await _locationService.StartListenign();
			await _locationService.GetLocation();

			await _azureService.Initialize(Me.Id);

			_chatsAzure = await _azureService.GetChats(useOffline: true);
			_messageAzure = await _azureService.GetMessages(useOffline: true);

			UpdateChats(_chatsAzure, _messageAzure);

			if (DataReady != null)
                DataReady(this, EventArgs.Empty);

			_initialized = true;

			_chatsAzure = await _azureService.GetChats();
			_messageAzure = await _azureService.GetMessages();

			UpdateChats(_chatsAzure, _messageAzure);

			RaiseChangedChats();

			_azureService.NewMessageAvailable += (sender, e) => 
			{
				_messageAzure = _azureService.GetMessages(instant: true).Result;
				UpdateChats(_chatsAzure, _messageAzure);
				RaiseChangedChats();
			};

			await CreateChatsWithFriends();

			await UpdateMessages();
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

		private async Task GetFacebookInfo(string token)
		{
			var requestStr = @"https://graph.facebook.com/me/?" +
				"fields=first_name,last_name,picture.type(normal)" +
				"&access_token=" + token;

			var httpClient = new HttpClient();
			var userJson = await httpClient.GetStringAsync(requestStr);
			var obj = JObject.Parse(userJson);

			Me = new Person();

			var id = obj["id"];
			if (id!=null)
			{
				Me.Id = id.ToString();	
			}

			var firstName = obj["first_name"];
			if (firstName!=null)
			{
				Me.Name = firstName.ToString();
			}

			var lastName = obj["last_name"];
			if (lastName!=null)
			{
				Me.Surname = lastName.ToString();
			}

			var picture = obj["picture"];
			if (picture!=null)
			{
				var data = picture["data"];
				if (data!=null)
				{
					var url = data["url"];
					if (url!=null)
					{
						Me.UrlImage = url.ToString();
					}
				}
			}

			var requestFriends = @"https://graph.facebook.com/me/friends?" +
				"fields=id,first_name,last_name,picture.type(normal)" +
				"&access_token=" + token;

			userJson = await httpClient.GetStringAsync(requestFriends);
			obj = (JObject)JsonConvert.DeserializeObject(userJson);

			Contacts = new List<Person>();

			if (obj["data"] is JArray)
	        {
	            foreach (var item in obj["data"])
	            {
					var facebookFriend = new Person
					{
						Name = item["first_name"]?.ToString(),
						Surname = item["last_name"]?.ToString(),
						Id = item["id"]?.ToString()
					};

					var picture_json = item["picture"];

					if (picture_json != null)
					{
						var data = picture_json["data"];
						if (data!=null)
						{
							var url = data["url"];
							if (url!=null)
							{
								facebookFriend.UrlImage = url.ToString();
							}
						}
					}
					if (facebookFriend.Id!=null)
					{
						Contacts.Add(facebookFriend);
					}
	            }
	        } 
		}

		private async Task UpdateMessages()
		{
			System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
			sw.Start();

			var countPrev = _messageAzure.Count();
			_messageAzure = await _azureService.GetMessages();

			if (_messageAzure.Count() > countPrev)
			{
				UpdateChats(_chatsAzure, _messageAzure);
				RaiseChangedChats();
			}

			sw.Stop();
			System.Diagnostics.Debug.WriteLine(sw.Elapsed);

			await UpdateMessages();
		}
	}
}
