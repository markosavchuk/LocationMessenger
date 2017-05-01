using System;
namespace LocationMessenger
{
	public class ChatUsersAzure
	{
		[Newtonsoft.Json.JsonProperty("id")]
		public string Id { get; set; }

		[Microsoft.WindowsAzure.MobileServices.CreatedAt]
		public DateTime CreatedAt { get; set; }

		[Microsoft.WindowsAzure.MobileServices.UpdatedAt]
		public DateTime UpdatedAt { get; set; }

		[Microsoft.WindowsAzure.MobileServices.Version]
		public string Version { get; set; }

		[Microsoft.WindowsAzure.MobileServices.Deleted]
		public bool Deleted { get; set; }

		[Newtonsoft.Json.JsonProperty("idUser")]
		public string UserId { get; set; }

		[Newtonsoft.Json.JsonProperty("idChat")]
		public string ChatId { get; set; }

		[Newtonsoft.Json.JsonProperty("read")]
		public bool Read { get; set; }
	}
}
