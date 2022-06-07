using Microsoft.AspNetCore.SignalR.Client;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Chat.ViewModel
{
    public partial class MainWindowViewModel: INotifyPropertyChanged
    {
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
            //Close = ReactiveCommand.CreateFromTask(JoinImpl);
        }
        private async void StartConnection()
        {
            _Connection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5000/chatroom")
                    .Build();

            _Connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                //addMessage($"{user}: {message}");
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

        /*private void addMessage(string message)
        {
            var newMessage = new TextBox();
            newMessage.MaxLength = 250;
            newMessage.Text = message;
            MainWindowBase.scroll1.Children.Add(newMessage);
        }*/

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

        /*private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _MessageText = ((TextBox)sender).Text;
        }*/

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}


/*    private string _User { get; set; }
            private string _MessageText { get; set; } = "Тест";
            private HubConnection _Connection { get; set; }

            public MainWindow()
            {
                InitializeComponent();
            }

            private async void StartConnection()
            {
                _Connection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5000/chatroom")
                    .Build();

                _Connection.On<string, string>("ReceiveMessage", (user, message) =>
                {
                    addMessage($"{user}: {message}");
                });

                _Connection.On<string, string, string>("ReceiveMessageFromGroup", (groupName, user, message) =>
                {
                    addMessage($"({groupName}) {user}: {message}");
                });

                _Connection.On<string, string>("ReceiveDirectMessage", (user, message) =>
                {
                    addMessage($"|From user| {user}: {message}");
                });

                _Connection.On<string, string>("ReceiveServiceMessage", (groupName, message) =>
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

                await _Connection.StartAsync();

                await _Connection.InvokeAsync("Enter", _User);
            }

            private void addMessage(string message)
            {
                var newMessage = new TextBox();
                newMessage.MaxLength = 250;
                newMessage.Text = message;
                scroll1.Children.Add(newMessage);
            }

            private async void Button_Message_Send(object sender, RoutedEventArgs e)
            {
                var message = _MessageText;

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

            private async void Button_Message_Escape(object sender, RoutedEventArgs e)
            {
                await _Connection.StopAsync();
            }

            private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
            {
                _MessageText = ((TextBox)sender).Text;
            }*/