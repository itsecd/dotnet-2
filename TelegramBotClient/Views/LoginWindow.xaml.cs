using System.Windows;
using TelegramBotClient.ViewModels;

namespace TelegramBotClient.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }
    }
}
