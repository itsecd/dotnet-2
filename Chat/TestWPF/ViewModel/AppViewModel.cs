using ChatServer.Serializers;
using Microsoft.AspNetCore.SignalR.Client;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Windows;

namespace TestWPF.ViewModel
{
    public partial class AppViewModel : INotifyPropertyChanged
    {
        private static readonly HubConnection connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/chatroom")
                .Build();

        private RoomList selectedRoom;
        public ObservableCollection<RoomList> RoomList { get; set; }

        public ReactiveCommand<Unit, Unit> EnterName { get; }

        public string User { get; set; } = string.Empty;
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
            EnterName = ReactiveCommand.Create(EnterImp);
        }

        public void EnterImp()
        {
            MessageBox.Show(User);
            RoomList = RoomListSerializer.DeserializeRoomList(User);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
