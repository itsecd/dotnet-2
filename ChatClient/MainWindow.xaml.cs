using ChatClient.ViewModel;
using Grpc.Net.Client;
using System.Windows;
using System.Windows.Controls;

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
            //ChatServer.ChatRoom.ChatRoomClient cl = new ChatServer.ChatRoom.ChatRoomClient(channel);

        }
    }
}
