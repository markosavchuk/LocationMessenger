using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Net;
using Java.Util;
using LocationMessenger.Droid;
using LocationMessenger.ViewModels;
using Xamarin.Forms;
using LocationMessenger.Views.CustomControls;
using Square.Picasso;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(MapMessages), typeof(MapMessageRenderer))]
namespace LocationMessenger.Droid
{
    class MapMessageRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter, IOnMapReadyCallback
    {
		GoogleMap _map;
        ObservableCollection<MapPinViewModel> _customPins;
        bool _isDrawn;
        MapMessages _formsMap;

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                _map.InfoWindowClick -= OnInfoWindowClick;
            }

            if (e.NewElement != null)
            {
                _formsMap = (MapMessages)e.NewElement;
                _customPins = _formsMap.CustomPins;
                ((MapView)Control).GetMapAsync(this);
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            _map = googleMap;
            _map.InfoWindowClick += OnInfoWindowClick;
            _map.SetInfoWindowAdapter(this);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals("VisibleRegion") && !_isDrawn)
            {
                _map.Clear();

                foreach (var pin in _customPins)
                {
                    var markerOptions = new MarkerOptions();
                    
                    markerOptions.SetPosition(new LatLng(pin.Pin.Position.Latitude, pin.Pin.Position.Longitude));
                    markerOptions.SetTitle(pin.AuthorName);
                    markerOptions.SetSnippet(pin.VisibleMessage);
                    var marker = _map.AddMarker(markerOptions);

                    var picassoMarker = new PicassoMarker(marker);
                    Picasso.With(Context)
                        .Load(pin.UrlImage)
                        .Resize(100,100)
                        .Placeholder(Resource.Drawable.icon)
                        .Into(picassoMarker);                    
                }
                _isDrawn = true;
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            if (changed)
            {
                _isDrawn = false;
            }
        }

        void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            var customPin = GetCustomPin(e.Marker);
            if (customPin == null)
            {
                throw new Exception("Custom pin not found");
            }

            _formsMap?.MessageClicked.Execute(customPin.IdMessage);
        }

        public View GetInfoContents(Marker marker)
        {
            return null;
        }

        public Android.Views.View GetInfoWindow(Marker marker)
        {
            return null;
        }

        MapPinViewModel GetCustomPin(Marker annotation)
        {
            var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
            foreach (var pin in _customPins)
            {
                if (pin.Pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }
    }
}
