using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocationMessenger.ViewModels;
using Prism.Commands;
using Xamarin.Forms.Maps;

namespace LocationMessenger.Views.CustomControls
{
    public class MapMessages : Map
    {
		public event EventHandler UpdatePinsEvent;
		public event EventHandler<string> MessageClicked;

        public ObservableCollection<MapPinViewModel> CustomPins { get; set; }

        public MapMessages()
        {
           
        }

		public void UpdatePins()
		{
			if (UpdatePinsEvent != null)
				UpdatePinsEvent(null, EventArgs.Empty);
		}

		public void ClickOnMessage(string id)
		{
			if (MessageClicked != null)
				MessageClicked(null, id);
		}
    }
}
