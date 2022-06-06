using System.Windows;

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
            DialogResult = true;
        }

        public string UserName => UserNameTextBox.Text; 

        public string RoomName => RoomNameTextBox.Text;
    }
}
