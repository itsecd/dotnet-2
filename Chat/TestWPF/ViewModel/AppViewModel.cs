using Microsoft.AspNetCore.SignalR.Client;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Windows;
using TestWPF.Properties;
using TestWPF.View;

namespace TestWPF.ViewModel
{
    public partial class AppViewModel : INotifyPropertyChanged
    {
        static readonly string baseUrl = Settings.Default.baseUrl;
        private static readonly HubConnection Connection = new HubConnectionBuilder()
                .WithUrl(baseUrl)
                .Build();

        public ReactiveCommand<Unit, Unit> EnterName { get; }

        public ReactiveCommand<Unit, Unit> SendMessage { get; }

        public ObservableCollection<string> Messages { get; set; } = new();

        private string? _user;
        public string User
        {
            get => _user!;

            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        private string? _message;
        public string Message
        {
            get => _message!;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }



        public AppViewModel()
        {
            EnterName = ReactiveCommand.Create(EnterImp);

            SendMessage = ReactiveCommand.Create(SendImp);

            Connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                AddMessage($"{user}: {message}");
            });
            Connection.On<string, string, string>("ReceiveMessageFromGroup", (groupName, user, message) =>
            {
                AddMessage($"({groupName}) {user}: {message}");
            });
            Connection.On<string, string>("ReceiveDirectMessage", (user, message) =>
            {
                AddMessage($"|From user| {user}: {message}");
            });

            Connection.On<string, string>("ReceiveServiceMessage", (groupName, message) =>
            {
                if (groupName == "common")
                {
                    AddMessage($"|Service message| {message}");
                }
                else
                {
                    AddMessage($"|Service message| {groupName}: {message}");
                }
            });
        }


        private async void EnterImp()
        {
            MessageBox.Show($"Hello {User}!");

            await Connection.StartAsync();
            await Connection.InvokeAsync("Enter", User);

            ChatWindow window = (ChatWindow)Application.Current.MainWindow;
            window.LoginScreen.Visibility = Visibility.Hidden;
            window.ChatScreen.Visibility = Visibility.Visible;

        }

        private async void SendImp()
        {
            string message = Message;
            if (message == "" || message == null)
            {
                return;
            }

            if (message.StartsWith("+"))
            {
                await Connection.InvokeAsync("JoinGroup", User, message.Split('+', ' ')[1]);
            }
            else if (message.StartsWith("-"))
            {
                await Connection.InvokeAsync("LeaveGroup", User, message.Split('-', ' ')[1]);
            }
            else if (message.StartsWith("#"))
            {
                var groupName = message.Split('#', ' ')[1];
                var messageToSend = message.Replace("#" + groupName, "");
                await Connection.InvokeAsync("SendMessageToGroup", groupName, User, messageToSend);
            }
            else if (message.StartsWith("@"))
            {
                var receiver = message.Split('@', ' ')[1];
                var messageToSend = message.Replace("@" + receiver, "");
                await Connection.InvokeAsync("SendMessageToUser", User, messageToSend, receiver);
            }
            else
            {
                await Connection.InvokeAsync("SendMessage", User, message);
            }
        }
        private void AddMessage(string message)
        {
            Messages.Add(message);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
