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
        private static readonly HubConnection Connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/chatroom")
                .Build();

        public ReactiveCommand<Unit, Unit> EnterName { get; }

        public ReactiveCommand<Unit, Unit> SendMessage { get; }

        public ObservableCollection<string> Messages { get; set; } = new();

        public string Message { get; set; } = string.Empty;

        public string User { get; set; } = string.Empty;

        public AppViewModel()
        {
            EnterName = ReactiveCommand.Create(EnterImp);

            SendMessage = ReactiveCommand.Create(SendImp);

            Connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                addMessage($"{user}: {message}");
            });
            Connection.On<string, string, string>("ReceiveMessageFromGroup", (groupName, user, message) =>
            {
                addMessage($"({groupName}) {user}: {message}");
            });
            Connection.On<string, string>("ReceiveDirectMessage", (user, message) =>
            {
                addMessage($"|From user| {user}: {message}");
            });

            Connection.On<string, string>("ReceiveServiceMessage", (groupName, message) =>
            {
                if (groupName == "common")
                {
                    addMessage($"|Service message| {message}");
                }
                else
                {
                    addMessage($"|Service message| {groupName}: {message}");
                }
            });
        }


        public async void EnterImp()
        {
            MessageBox.Show(User);

            await Connection.StartAsync();
            await Connection.InvokeAsync("Enter", User);

        }

        public async void SendImp()
        {
            var message = Message;

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
        private void addMessage(string message)
        {
            Messages.Add(message);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
