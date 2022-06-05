using ChatClient.Model;
using ChatServer.Serializers;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChatClient.ViewModel
{
    public class AppViewModel : INotifyPropertyChanged
    {
		private RoomList selectedRoom;
		private string user;
		public List <RoomList> roomList { get; set; }

		public string User
        {
			get { return user; }
            set
            {
				user = value;
				OnPropertyChanged("User");
			}
		}
		public RoomList SelectedRoom
        {
			get { return selectedRoom; }
            set
            {
				selectedRoom = value;
				OnPropertyChanged("SelectedUser");
            }
        }

		public AppViewModel()
        {
			roomList = RoomListSerializer.DeserializeRoomList(user);
        }
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
	}
}
