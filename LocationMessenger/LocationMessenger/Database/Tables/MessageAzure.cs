using System;
namespace LocationMessenger
{
	public class MessageAzure
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

		[Newtonsoft.Json.JsonProperty("latitude")]
		public double Latitude { get; set; }

		[Newtonsoft.Json.JsonProperty("longitude")]
		public double Longitude { get; set; }

		[Newtonsoft.Json.JsonProperty("ownerId")]
		public string OwnerId { get; set; }

		[Newtonsoft.Json.JsonProperty("chatId")]
		public string ChatId { get; set; }

		[Newtonsoft.Json.JsonProperty("text")]
		public string Text { get; set; }

		public bool Visible { get; set; }
	}
}
