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
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text == "admin" && PasswordBox.Password == "123")
            {
                Open(ChatScreen);
            }
            else
            {
                LoginMessageBlock.Text = "Wrong login or Password!";
                LoginMessageBlock.Visibility = Visibility.Visible;
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Open(RegisterScreen);
        }

        private void Open(Border screen)
        {
            LoginScreen.Visibility = Visibility.Hidden;
            ChatScreen.Visibility = Visibility.Hidden;

            screen.Visibility = Visibility.Visible;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Open(LoginScreen);
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Text != null)
            {
               
            }
        }
    }
}
