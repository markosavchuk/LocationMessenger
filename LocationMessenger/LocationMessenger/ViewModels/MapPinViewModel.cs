using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace LocationMessenger.ViewModels
{
    public class MapPinViewModel
    {
        public Pin Pin { get; set; }
        public string IdMessage { get; set; }
        public string AuthorName { get; set; }
        public string VisibleMessage { get; set; }
        public string UrlImage { get; set; }
    }
}
