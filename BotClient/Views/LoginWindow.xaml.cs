using BotClient.ViewModels;
using System.Windows;

namespace BotClient.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }

        public LoginWindow(LoginViewModel loginViewModel)
        {
            InitializeComponent();
            DataContext = loginViewModel;
        }
    }
}
