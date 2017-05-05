using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LocationMessenger.Models;

namespace LocationMessenger
{
	public interface IData
	{
		List<Person> Contacts { get; set; }
		List<Chat> Chats { get; set; }
		Person Me { get; set; }

		event EventHandler ChatsChanged;
		event EventHandler DataReady;
		Task Initialize();
		Task SendMessage(Message msg, string chatId);
		Task ReadChat(string idChat);
		void RaiseChangedChats();
	}
}
