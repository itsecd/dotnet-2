using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.AspNetCore.SignalR.Client;

namespace Chat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string UserName { get; set; }
        private string MessageText { get; set; } = "Тест";
        private HubConnection Connection { get; set; }

        public MainWindow(string name)
        {
            InitializeComponent();
            UserName = name;

            StartConnection();
        }

        private async void StartConnection()
        {
            Connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/chatroom")
                .Build();

            Connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                //MessageBox.Show($"[bold yellow]{user}[/]: [blue]{message}[/]");
                addMessage($"{user}: {message}");
            });

            Connection.On<string, string, string>("ReceiveMessageFromGroup", (groupName, user, message) =>
            {
                //MessageBox.Show($"([bold red]{groupName}[/]) [bold yellow]{user}[/]: [blue]{message}[/]");
                addMessage($"({groupName}) {user}: {message}");
            });

            Connection.On<string, string>("ReceiveDirectMessage", (user, message) =>
            {
                //MessageBox.Show($"[bold red]{user}[/]: [blue]{message}[/]");
                addMessage($"|From user| {user}: {message}");
            });

            Connection.On<string, string>("ReceiveServiceMessage", (groupName, message) =>
            {
                if (groupName == "common")
                {
                    //MessageBox.Show($"[bold green]Service message[/]: [blue]{message}[/]");
                    addMessage($"|Service message| {message}");
                }
                else
                {
                    //MessageBox.Show($"([bold red]{groupName}[/]): [blue]{message}[/]");
                    addMessage($"|Service message| {groupName}: {message}");
                }
            });

            await Connection.StartAsync();

            await Connection.InvokeAsync("Enter", UserName);
        }

        private void addMessage(string message)
        {
            var newMessage = new TextBox();
            //< TextBox Grid.Row = "2" Grid.Column = "0" MaxLength = "250" TextChanged = "TextBox_TextChanged" ></ TextBox >
            newMessage.MaxLength = 250;
            newMessage.Text = message;
            scroll1.Children.Add(newMessage);
        }

        private async void Button_Message_Send(object sender, RoutedEventArgs e)
        {
            var message = MessageText;

            if (message.StartsWith("+"))
                await Connection.InvokeAsync("JoinGroup", message.Split('+', ' ')[1], UserName);
            else if (message.StartsWith("-"))
                await Connection.InvokeAsync("LeaveGroup", message.Split('-', ' ')[1], UserName);
            else if (message.StartsWith("#"))
            {
                var groupName = message.Split('#', ' ')[1];
                var messageToSend = message.Replace("#" + groupName, "");
                await Connection.InvokeAsync("SendMessageToGroup", groupName, UserName, messageToSend);
            }
            else if (message.StartsWith("@"))
            {
                var receiver = message.Split('@', ' ')[1];
                var messageToSend = message.Replace("@" + receiver, "");
                await Connection.InvokeAsync("SendMessageToUser", UserName, messageToSend, receiver);
            }
            else
                await Connection.InvokeAsync("SendMessage", UserName, message);
        }

        private async void Button_Message_Escape(object sender, RoutedEventArgs e)
        {
            await Connection.StopAsync();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MessageText = ((TextBox)sender).Text;
        }
    }
}
