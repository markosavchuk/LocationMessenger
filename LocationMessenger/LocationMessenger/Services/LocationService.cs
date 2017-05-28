using System;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;

namespace LocationMessenger
{
	public class LocationService : ILocationService
	{
		private Plugin.Geolocator.Abstractions.IGeolocator _locator;

		private const double Radius = 0.010; // 10 meters

		public event EventHandler ChangedLocation;

		public Position DefaultLocation { get; set; } = new Position(49.8350515, 24.0073365);
		public Position Location { get; set; }

		public LocationService()
		{
			_locator = CrossGeolocator.Current;
			_locator.DesiredAccuracy = 10;


			_locator.PositionChanged += (sender, e) => 
			{
				Location = new Position(e.Position.Latitude, e.Position.Longitude);

				if (ChangedLocation!=null)
					ChangedLocation(this, EventArgs.Empty);
			};
		}

		public async Task<Position> GetLocation()
		{
			if (_locator.IsGeolocationEnabled && _locator.IsGeolocationAvailable)
			{
				var position = await _locator.GetPositionAsync(3000);
				if (_locator != null)
				{
					Location = new Position(position.Latitude, position.Longitude);

					if (ChangedLocation != null)
						ChangedLocation(this, EventArgs.Empty);

					return Location;
				}
				else
				{
					if (Location == null)
						return DefaultLocation;
					else
						return Location;
				}
			}
			else
			{
				if (Location == null)
					return DefaultLocation;
				else
					return Location;
			}
		}

		public async Task StartListenign()
		{
			await _locator.StartListeningAsync(5000, 1);
		}

		public bool MessageInArea(Position msgPos)
		{
			if (Location == null)
				return false;
			if (Distance(msgPos.Latitude, msgPos.Longitude, Location.Latitude, Location.Longitude, 'K') <= Radius)
				return true;
			else
				return false;
		}

		private double Distance(double lat1, double lon1, double lat2, double lon2, char unit)
		{
			double theta = lon1 - lon2;
			double dist = Math.Sin(Deg2rad(lat1)) * Math.Sin(Deg2rad(lat2)) + 
		                  Math.Cos(Deg2rad(lat1)) * Math.Cos(Deg2rad(lat2)) * Math.Cos(Deg2rad(theta));
			dist = Math.Acos(dist);
			dist = Rad2deg(dist);
			dist = dist * 60 * 1.1515;

			if (unit == 'K')
			{
				dist = dist * 1.609344;
			}
			else if (unit == 'N')
			{
				dist = dist * 0.8684;
			}

			return (dist);
		}

		private double Deg2rad(double deg)
		{
			return (deg * Math.PI / 180.0);
		}

		private double Rad2deg(double rad)
		{
			return (rad / Math.PI * 180.0);
		}
	}
}
