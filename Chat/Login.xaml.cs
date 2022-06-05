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
using System.Windows.Shapes;

namespace Chat
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private string MessageTextName { get; set; } = "Тест";
        public Login()
        {
            InitializeComponent();
        }

        private void Button_login(object sender, RoutedEventArgs e)
        {
            if (MessageTextName.Length > 0)
            {
                var window = new MainWindow(MessageTextName);
                window.Show();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MessageTextName = ((TextBox)sender).Text;
        }
    }
}
