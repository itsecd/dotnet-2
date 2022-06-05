using ChatClient.ViewModel;
using System.Windows;

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
