using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LocationMessenger.Droid;
using LocationMessenger.Views.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ChooseLocationMap), typeof(ChooseLocationMapRenderer))]
namespace LocationMessenger.Droid
{
    class ChooseLocationMapRenderer : MapRenderer, IOnMapReadyCallback
    {
        // We use a native google map for Android
        private GoogleMap _map;

        public void OnMapReady(GoogleMap googleMap)
        {
            _map = googleMap;

            if (_map != null)
                _map.MapClick += googleMap_MapClick;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            if (_map != null)
                _map.MapClick -= googleMap_MapClick;

            base.OnElementChanged(e);

            ((MapView) Control)?.GetMapAsync(this);
        }

        private void googleMap_MapClick(object sender, GoogleMap.MapClickEventArgs e)
        {
            ((ChooseLocationMap)Element).OnTap(new Position(e.Point.Latitude, e.Point.Longitude));
        }
    }
}