using System;
namespace LocationMessenger
{
	public class ChatAzure
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
	}
}
