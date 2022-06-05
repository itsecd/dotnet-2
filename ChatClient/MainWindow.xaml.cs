using ChatClient.ViewModel;
using Grpc.Net.Client;
using System.Windows;
using System.Windows.Controls;

namespace ChatClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        public string currentMessage { get => textMessage.Text; }
        
    }
}
