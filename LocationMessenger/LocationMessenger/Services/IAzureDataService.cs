using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LocationMessenger.Models;

namespace LocationMessenger
{
	public interface IAzureDataService
	{
		Task Initialize(string myId);
		Task<IList<MessageAzure>> GetMessages(bool syncChats = false);
		Task AddMessage(Message msgModel, string chatId);
		Task<IList<ChatAzure>> GetChats();
		Task AddChat(string idFriend);
	}
}
