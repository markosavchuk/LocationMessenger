using System;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace LocationMessenger
{
	public interface ILocationService
	{
		event EventHandler ChangedLocation;
		Position DefaultLocation { get; set; }
		Position Location { get; set; }
		Task<Position> GetLocation();
		Task StartListenign();
		bool MessageInArea(Position msgPos);
	}
}
