using ChatClient.Properties;
using ChatServer;
using Grpc.Net.Client;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.ViewModel
{
    public class MainViewModel : ReactiveObject
    {
        private string _userName;
        private string _roomName;
        public ObservableCollection<UserInfo> Users { get; set; } = new ObservableCollection<UserInfo>();
        public ObservableCollection<(DateTime,Message)> Messages = new ObservableCollection<(DateTime, Message)>();
        
        private ChatServer.ChatRoom.ChatRoomClient _client;

        
        public ReactiveCommand<Unit, Unit> CreateCommand { get; }
        public ReactiveCommand<Unit, Task> JoinCommand { get; }
        
        public MainViewModel()
        {
            var channel = GrpcChannel.ForAddress(Settings.Default.Address);
            _client = new ChatServer.ChatRoom.ChatRoomClient(channel);
            CreateCommand = ReactiveCommand.Create(CreateImpl);
            JoinCommand = ReactiveCommand.Create(JoinImpl);
        }

        private void CreateImpl()
        {
            DialogWindow dialogWindow = new DialogWindow();
            if ((bool)dialogWindow.ShowDialog())
            {
                _userName = dialogWindow.UserName;
                _roomName = dialogWindow.RoomName;
                var streamingCall = _client.Create();
                streamingCall.RequestStream.WriteAsync(new Message { User = _userName, Text = _roomName, Command = "create" });




            }
            else
            {
                MessageBox.Show("Окно закрыто пользователем!");
            }
        }

        private async Task JoinImpl()
        {
            DialogWindow dialogWindow = new DialogWindow();
            var dialogResult = dialogWindow.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                _userName = dialogWindow.UserName;
                _roomName = dialogWindow.RoomName;

                using (var streamingCall = _client.Join())
                {
                    await streamingCall.RequestStream.WriteAsync(new Message { User = _userName, Text = _roomName, Command = "" });
                    await streamingCall.ResponseStream.MoveNext(new System.Threading.CancellationToken());
                    Messages.Add((DateTime.Now, streamingCall.ResponseStream.Current));

                    RoomInfo roomInfo = new RoomInfo { RoomName = _roomName };
                    UsersInfoResponse usersResponse = await _client.GetUsersAsync(roomInfo);
                    
                    foreach (var user in usersResponse.Users)
                    {
                        Users.Add(user);

                    }

                    var readTask = Task.Run(async () =>
                    {
                        while (await streamingCall.ResponseStream.MoveNext(new System.Threading.CancellationToken()))
                        {
                            Messages.Add((DateTime.Now, streamingCall.ResponseStream.Current));
                        }
                    });
                    await readTask;
                }

                //выгрузить список пользователей
                //выгрузить историю сообщений


            }
            else
            {
                MessageBox.Show("Окно закрыто пользователем!");
            }
        }
    }
}
