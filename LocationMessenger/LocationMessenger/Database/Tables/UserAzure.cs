using System;
namespace LocationMessenger
{
	public class UserAzure
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

		[Newtonsoft.Json.JsonProperty("name")]
		public string Name { get; set; }

		[Newtonsoft.Json.JsonProperty("surname")]
		public string Surname { get; set; }

		[Newtonsoft.Json.JsonProperty("urlImage")]
		public string ImageUrl { get; set; }

		[Newtonsoft.Json.JsonProperty("gender")]
		public double Gender { get; set; }
	}
}
