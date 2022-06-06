using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Chat.ViewModel
{
    public partial class MainWindowViewModel
    {
        private HubConnection Connection { get; set; }
        private string MessageText { get; set; } = "Тест";
        private string User { get; set; }
        public ObservableCollection<string> Messages { get; set; } = new();
        private MainWindow MainWindow { get; }
        public MainWindowViewModel(string user, MainWindow mw)
        {
            User = user;
            MainWindow = mw;
        }
        private async void StartConnection()
        {
            Connection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5000/chatroom")
                    .Build();

            Connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                //addMessage($"{user}: {message}");
                Messages.Add($"{user}: {message}");
            });

            Connection.On<string, string, string>("ReceiveMessageFromGroup", (groupName, user, message) =>
            {
                Messages.Add($"({groupName}) {user}: {message}");
            });

            Connection.On<string, string>("ReceiveDirectMessage", (user, message) =>
            {
                Messages.Add($"|From user| {user}: {message}");
            });

            Connection.On<string, string>("ReceiveServiceMessage", (groupName, message) =>
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

            await Connection.StartAsync();

            await Connection.InvokeAsync("Enter", User);
        }

        /*private void addMessage(string message)
        {
            var newMessage = new TextBox();
            newMessage.MaxLength = 250;
            newMessage.Text = message;
            MainWindowBase.scroll1.Children.Add(newMessage);
        }*/

        private async void Button_Message_Send(object sender, RoutedEventArgs e)
        {
            var message = MessageText;

            if (message.StartsWith("+"))
                await Connection.InvokeAsync("JoinGroup", message.Split('+', ' ')[1], User);
            else if (message.StartsWith("-"))
                await Connection.InvokeAsync("LeaveGroup", message.Split('-', ' ')[1], User);
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
                await Connection.InvokeAsync("SendMessage", User, message);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MessageText = ((TextBox)sender).Text;
        }
    }
}


/*    private string User { get; set; }
            private string MessageText { get; set; } = "Тест";
            private HubConnection Connection { get; set; }

            public MainWindow()
            {
                InitializeComponent();
            }

            private async void StartConnection()
            {
                Connection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5000/chatroom")
                    .Build();

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

                await Connection.StartAsync();

                await Connection.InvokeAsync("Enter", User);
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
                var message = MessageText;

                if (message.StartsWith("+"))
                    await Connection.InvokeAsync("JoinGroup", message.Split('+', ' ')[1], User);
                else if (message.StartsWith("-"))
                    await Connection.InvokeAsync("LeaveGroup", message.Split('-', ' ')[1], User);
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
                    await Connection.InvokeAsync("SendMessage", User, message);
            }

            private async void Button_Message_Escape(object sender, RoutedEventArgs e)
            {
                await Connection.StopAsync();
            }

            private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
            {
                MessageText = ((TextBox)sender).Text;
            }*/