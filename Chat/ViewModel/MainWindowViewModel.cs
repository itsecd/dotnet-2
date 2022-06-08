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
        private HubConnection _Connection { get; set; }

        private string? _messageText;
        public string _MessageText { 
            get => _messageText;
            set
            {
                _messageText = value;
                OnPropertyChanged();
            } 
        }
        private string _User { get; set; }


        public ReactiveCommand<Unit, Unit> Send { get; }
        public ReactiveCommand<Unit, Unit> Close { get; }
        public ObservableCollection<string> Messages { get; set; } = new();

        private MainWindow _MainWindow { get; }

        public MainWindowViewModel(string user, MainWindow mw)
        {
            _User = user;
            _MainWindow = mw;
            StartConnection();
            Send = ReactiveCommand.CreateFromTask(Message_Send);
        }
        private async void StartConnection()
        {
            _Connection = new HubConnectionBuilder()
                    .WithUrl(ChatURL)
                    .Build();

            _Connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Messages.Add($"{user}: {message}");
            });

            _Connection.On<string, string, string>("ReceiveMessageFromGroup", (groupName, user, message) =>
            {
                Messages.Add($"({groupName}) {user}: {message}");
            });

            _Connection.On<string, string>("ReceiveDirectMessage", (user, message) =>
            {
                Messages.Add($"|From user| {user}: {message}");
            });

            _Connection.On<string, string>("ReceiveServiceMessage", (groupName, message) =>
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

            await _Connection.StartAsync();

            await _Connection.InvokeAsync("Enter", _User);
        }

        private async Task Message_Send()
        {
            string message = _MessageText;
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            if (message.StartsWith("+"))
                await _Connection.InvokeAsync("JoinGroup", message.Split('+', ' ')[1], _User);
            else if (message.StartsWith("-"))
                await _Connection.InvokeAsync("LeaveGroup", message.Split('-', ' ')[1], _User);
            else if (message.StartsWith("#"))
            {
                var groupName = message.Split('#', ' ')[1];
                var messageToSend = message.Replace("#" + groupName, "");
                await _Connection.InvokeAsync("SendMessageToGroup", groupName, _User, messageToSend);
            }
            else if (message.StartsWith("@"))
            {
                var receiver = message.Split('@', ' ')[1];
                var messageToSend = message.Replace("@" + receiver, "");
                await _Connection.InvokeAsync("SendMessageToUser", _User, messageToSend, receiver);
            }
            else
                await _Connection.InvokeAsync("SendMessage", _User, message);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}