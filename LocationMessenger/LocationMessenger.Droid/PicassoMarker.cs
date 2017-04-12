using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Graphics.Drawable;
using Android.Views;
using Android.Widget;
using Square.Picasso;

namespace LocationMessenger.Droid
{
    class PicassoMarker : Java.Lang.Object, ITarget
    {
        private readonly Marker _marker;

        public PicassoMarker(Marker marker)
        {
            _marker = marker;
        }

        public void OnBitmapLoaded(Bitmap bmp, Picasso.LoadedFrom from)
        {
            _marker.SetIcon(BitmapDescriptorFactory.FromBitmap(CreateRoundedBitmap(bmp)));
        }

        public static Bitmap CreateRoundedBitmap(Bitmap bitmap, int padding = 10)
        {
            Bitmap output = Bitmap.CreateBitmap(bitmap.Width, bitmap.Height, Bitmap.Config.Argb8888);
            var canvas = new Canvas(output);

            var paint = new Paint();
            var rect = new Rect(0, 0, bitmap.Width - padding, bitmap.Height - padding);
            var rectF = new RectF(rect);
            paint.AntiAlias = true;
            canvas.DrawARGB(0, 0, 0, 0);
            canvas.DrawOval(rectF, paint);
            paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));

            canvas.DrawBitmap(bitmap, rect, rect, paint);

            return output;
        }

        public void OnPrepareLoad(Drawable p0)
        {
        }

        public void OnBitmapFailed(Drawable p0)
        {
            Console.WriteLine("loading failed");
        }
    }
}