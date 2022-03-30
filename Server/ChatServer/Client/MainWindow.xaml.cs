using ChatServer;
using Grpc.Net.Client;
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

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
     
    public partial class MainWindow : Window
    {
        private bool isConnected;
        
        private async void ConnectUser() {
            if (!isConnected) 
            {
               
                tbUserName.IsEnabled = false;
                bConDis.Content = "Disconnect";
                isConnected = true;
                string userName = tbUserName.Text;
                var channel = GrpcChannel.ForAddress("https://localhost:5001");
                var client = new ChatRoom.ChatRoomClient(channel);
                using (var chat = client.Join()) {
                    _ = Task.Run(async () =>
                    {
                        while (await chat.ResponseStream.MoveNext(cancellationToken: System.Threading.CancellationToken.None))
                        {
                            var response = chat.ResponseStream.Current;
                            MessageBox.Show($"{response.User}: {response.Text}");
                        }
                    });
                    await chat.RequestStream.WriteAsync(new Message()
                    { User = userName, Text = " has joined the room" });
                }
               


            }
        }

        private void DisconnectUser() {
            if (isConnected)
            {
                tbUserName.IsEnabled = true;
                bConDis.Content = "Connect";
                isConnected=false;
                

            }
        }
       
        public MainWindow()
        {
            InitializeComponent();
        }

        private void bConDis_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected) {
                DisconnectUser();
            }
            else 
            {
                ConnectUser();
            }
        }
    }
}
