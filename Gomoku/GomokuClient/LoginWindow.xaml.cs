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

namespace GomokuClient
{
    public partial class LoginWindow : Window
    {
        public string Login { get; set; } = "Player";

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void PlayClick(object sender, RoutedEventArgs e)
        {
            Login = LoginTextBox.Text;
            BusyIndicator.IsBusy = true;
        }
    }
}
