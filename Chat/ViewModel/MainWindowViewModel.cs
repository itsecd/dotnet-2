using Microsoft.AspNetCore.SignalR.Client;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Chat.Properties;

namespace Chat.ViewModel
{
    public partial class MainWindowViewModel: INotifyPropertyChanged
    {
        private static readonly string ChatURL = Settings.Default.defaultChatURL;
        private HubConnection _connection;

        private string _messageText;
        public string MessageText { 
            get => _messageText;
            set
            {
                _messageText = value;
                OnPropertyChanged();
            } 
        }
        private string _user;


        public ReactiveCommand<Unit, Unit> Send { get; }
        public ObservableCollection<string> Messages { get; set; } = new();

        public MainWindowViewModel(string user, MainWindow mw)
        {
            _user = user;
            StartConnection();
            Send = ReactiveCommand.CreateFromTask(Message_Send);
        }
        private async void StartConnection()
        {
            _connection = new HubConnectionBuilder()
                    .WithUrl(ChatURL)
                    .Build();

            _connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Messages.Add($"{user}: {message}");
            });

            _connection.On<string, string, string>("ReceiveMessageFromGroup", (groupName, user, message) =>
            {
                Messages.Add($"({groupName}) {user}: {message}");
            });

            _connection.On<string, string>("ReceiveDirectMessage", (user, message) =>
            {
                Messages.Add($"|From user| {user}: {message}");
            });

            _connection.On<string, string>("ReceiveServiceMessage", (groupName, message) =>
            {
                if (groupName == "common")
                {
                    Messages.Add($"|Service message| {message}");
                }
                else
                {
                    Messages.Add($"|Service message| {groupName}: {message}");
                }
            });

            await _connection.StartAsync();

            await _connection.InvokeAsync("Enter", _user);
        }

        private async Task Message_Send()
        {
            string message = MessageText;
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            if (message.StartsWith("+"))
                await _connection.InvokeAsync("JoinGroup", message.Split('+', ' ')[1], _user);
            else if (message.StartsWith("-"))
                await _connection.InvokeAsync("LeaveGroup", message.Split('-', ' ')[1], _user);
            else if (message.StartsWith("#"))
            {
                var groupName = message.Split('#', ' ')[1];
                var messageToSend = message.Replace("#" + groupName, "");
                await _connection.InvokeAsync("SendMessageToGroup", groupName, _user, messageToSend);
            }
            else if (message.StartsWith("@"))
            {
                var receiver = message.Split('@', ' ')[1];
                var messageToSend = message.Replace("@" + receiver, "");
                await _connection.InvokeAsync("SendMessageTo_user", _user, messageToSend, receiver);
            }
            else
                await _connection.InvokeAsync("SendMessage", _user, message);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}