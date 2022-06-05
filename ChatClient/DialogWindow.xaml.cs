using System.Windows;
using System.Windows.Controls;

namespace ChatClient
{
    public partial class DialogWindow : Window
    {
        public DialogWindow()
        {
            InitializeComponent();
        }

        private void OnOkclicked(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

        }

        public string UserName { get => userName.Text; }
        
        public string RoomName { get => roomName.Text;}
    }
}
