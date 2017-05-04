using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LocationMessenger.Models;

namespace LocationMessenger
{
	public interface IAzureDataService
	{
		Task Initialize(string myId);
		Task<IList<MessageAzure>> GetMessages(bool syncChats = false, bool useOffline = false);
		Task AddMessage(Message msgModel, string chatId);
		Task<Tuple<IList<ChatAzure>, IList<ChatUsersAzure>>> GetChats(bool useOffline = false);
		Task<Tuple<IList<ChatAzure>, IList<ChatUsersAzure>>> AddChat(string idFriend);
	}
}
