using System.Reactive.Linq;
using ReactiveUI;
using BotClient.ViewModels;
using Lab2Server.Models;
using System.Windows;

namespace BotClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(LoginViewModel loginViewModel)
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}