using ChatClient.ViewModel;
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

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void userName_TextChanged(object sender, TextChangedEventArgs e)
        {
            //(DataContext as MainViewModel).Rooms

            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            ChatServer.ChatRoom.ChatRoomClient cl = new ChatServer.ChatRoom.ChatRoomClient(channel);

        }
    }
}
