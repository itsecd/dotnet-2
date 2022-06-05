using ChatClient.ViewModel;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;

namespace ChatClient.View
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        public ChatWindow()
        {
            InitializeComponent();

            DataContext = new AppViewModel();

            //HubConnection connection = new HubConnectionBuilder()
            //    .WithUrl("http://localhost:5000/chatroom")
            //    .Build();
        }

        private async void enterButton_Click(object sender, RoutedEventArgs e)
        {
            LoginScreen.Visibility = Visibility.Hidden;
            ChatScreen.Visibility = Visibility.Visible;
        }
    }
}
