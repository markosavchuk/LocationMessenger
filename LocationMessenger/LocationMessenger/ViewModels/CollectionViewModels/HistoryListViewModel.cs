using Prism.Mvvm;

namespace LocationMessenger.ViewModels.ListViews
{
    public class HistoryListViewModel : BindableBase
    {
		private string _id;
		private string _lastMessage;
		private string _chatName;

		public string Id
		{
			get { return _id; }
			set { SetProperty(ref _id, value); }
		}

		public string LastMessage
		{
			get { return _lastMessage; }
			set { SetProperty(ref _lastMessage, value); }
		}

		public string ChatName
		{
			get { return _chatName; }
			set { SetProperty(ref _chatName, value); }
		}
	}
}
