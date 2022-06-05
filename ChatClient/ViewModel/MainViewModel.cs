using ChatClient.Models;
using ChatClient.Properties;
using ChatServer;
using Grpc.Net.Client;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.ViewModel
{
    public class MainViewModel : ReactiveObject
    {
        private string _userName;
        private string _roomName;

        public string TextMessage { get; set; }

        public ObservableCollection<MyUserInfo> Users { get; set; } = new ObservableCollection<MyUserInfo>();
        public ObservableCollection<MyHistoryOfMessagesModel> Messages { get; set; } = new ObservableCollection<MyHistoryOfMessagesModel>();

        private ChatRoom.ChatRoomClient _client;

        private Grpc.Core.AsyncDuplexStreamingCall<Message, Message> _streamingCall;

        public ReactiveCommand<Unit, Unit> CreateCommand { get; }
        public ReactiveCommand<Unit, Task> JoinCommand { get; }
        public ReactiveCommand<Unit, Task> SendCommand { get; }

        public ReactiveCommand<Unit, Task> DisconnectCommand { get; }



        public MainViewModel()
    {
            var channel = GrpcChannel.ForAddress(Settings.Default.Address);
            _client = new ChatRoom.ChatRoomClient(channel);
            CreateCommand = ReactiveCommand.Create(CreateImpl);
            JoinCommand = ReactiveCommand.Create(JoinImpl);
            SendCommand = ReactiveCommand.Create(SendImpl);
            DisconnectCommand = ReactiveCommand.Create(DisconnectImpl);
        }

        private async void CreateImpl()
        {
            DialogWindow dialogWindow = new DialogWindow();
            var dialogResult = dialogWindow.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                _userName = dialogWindow.UserName;
                _roomName = dialogWindow.RoomName;
                _streamingCall = _client.Create();
                await _streamingCall.RequestStream.WriteAsync(new Message { User = _userName, Text = _roomName, Command = "create" });
                await _streamingCall.ResponseStream.MoveNext(new System.Threading.CancellationToken());

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

                _streamingCall = _client.Join();
                await _streamingCall.RequestStream.WriteAsync(new Message { User = _userName, Text = _roomName, Command = "" });
                await _streamingCall.ResponseStream.MoveNext(new System.Threading.CancellationToken());
                var LastMessage = new MyHistoryOfMessagesModel
                {
                    User = _streamingCall.ResponseStream.Current.User,
                    data = DateTime.Now,
                    Message = _streamingCall.ResponseStream.Current.Text
                };
                Messages.Add(new MyHistoryOfMessagesModel
                {
                    User = _streamingCall.ResponseStream.Current.User,
                    data = DateTime.Now,
                    Message = _streamingCall.ResponseStream.Current.Text
                });
                RoomInfo roomInfo = new RoomInfo { RoomName = _roomName };
                UsersInfoResponse usersResponse = await _client.GetUsersAsync(roomInfo);

                foreach (var user in usersResponse.Users)
                {
                    Users.Add(new MyUserInfo { Name = user.UserName, Status = user.IsOnline });

                }
                HistoryOfMessages messages = new();
                messages = await _client.GetHistoryOfMessagesAsync(roomInfo);
                var mindata = DateTime.Parse(messages.DateOfMessage[0]);
                var countOfMessages = messages.Messages.Count;
                for (var i = 0; i < countOfMessages; ++i)
                {
                    var index = 0;
                    for (var j = 0; j < messages.Messages.Count; j++)
                    {
                        if (DateTime.Parse(messages.DateOfMessage[j]) < DateTime.Parse(messages.DateOfMessage[index]))
                        {
                            index = j;
                        }

                    }
                    Messages.Add(new MyHistoryOfMessagesModel
                    {
                        User = messages.Messages[index].User,
                        data = DateTime.Parse(messages.DateOfMessage[index]),
                        Message = messages.Messages[index].Text
                    });
                    messages.DateOfMessage.RemoveAt(index);
                    messages.Messages.RemoveAt(index);

                }
                Messages.Add(LastMessage);
                var readTask = Task.Run(async () =>
                {
                    while (await _streamingCall.ResponseStream.MoveNext(new System.Threading.CancellationToken()))
                    {
                        Application.Current.Dispatcher.Invoke(() => Messages.Add(new MyHistoryOfMessagesModel
                        {
                            User = _streamingCall.ResponseStream.Current.User,
                            data = DateTime.Now,
                            Message = _streamingCall.ResponseStream.Current.Text
                        }));

                        var messageText = _streamingCall.ResponseStream.Current.Text;
                        var messageCommand = _streamingCall.ResponseStream.Current.Command;
                        if (messageText.Contains("connected") && messageCommand == "")
                        {
                            var connectedUser = messageText.Replace(" connected", "");
                            var isUserExist = false;
                            foreach (var user in Users)
                            {
                                if (user.Name == connectedUser)
                                {
                                    isUserExist = true;
                                    user.Status = true;
                                    user.RaisePropertyChanged("FormatName");
                                }
                            }
                            if (!isUserExist)
                            {
                                Application.Current.Dispatcher.Invoke(() => Users.Add(new MyUserInfo
                                {
                                    Name = connectedUser,
                                    Status = true
                                }));
                                this.RaisePropertyChanged(nameof(Users));
                            }
                        }
                        if (messageText.Contains("disconnected") && messageCommand == "disconnect")
                        {
                            var connectedUser = messageText.Replace(" disconnected", "");
                            foreach (var user in Users)
                            {
                                if (user.Name == connectedUser)
                                {
                                    user.Status = false;
                                    user.RaisePropertyChanged("FormatName");
                                }
                            }
                        }
                    }
                });
                await readTask;
            }
            else
            {
                MessageBox.Show("Окно закрыто пользователем!");
            }
        }

        private async Task SendImpl()
        {
            await _streamingCall.RequestStream.WriteAsync(new Message { User = _userName, Text = TextMessage, Command = "message" });

            Messages.Add(new MyHistoryOfMessagesModel
            {
                User = _userName,
                data = DateTime.Now,
                Message = TextMessage
            });
            TextMessage = string.Empty;
            this.RaisePropertyChanged(nameof(TextMessage));
        }

        private async Task DisconnectImpl()
        {
            await _streamingCall.RequestStream.WriteAsync(new Message { User = _userName, Command = "disconnect" });
            Users.Clear();
            Messages.Clear();
            this.RaisePropertyChanged(nameof(Users));
            this.RaisePropertyChanged(nameof(Messages));

        }
    }
}
