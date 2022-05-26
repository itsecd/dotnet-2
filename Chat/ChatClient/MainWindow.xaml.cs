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
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text == "admin" && PasswordBox.Password == "123")
            {
                //Open(ContactsScreen);
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

            screen.Visibility = Visibility.Visible;
        }
    }
}
