using TestWPF.ViewModel;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;
using ReactiveUI;

namespace TestWPF.View
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>

    public class ChatWindowBase : ReactiveWindow<AppViewModel> { }
    public partial class ChatWindow : ChatWindowBase
    {
        public ChatWindow()
        {
            InitializeComponent();

            

            _ = this.WhenActivated(cd =>
            {
                if (ViewModel is null)
                    return;

            });

        }

        private async void enterButton_Click(object sender, RoutedEventArgs e)
        {
            LoginScreen.Visibility = Visibility.Hidden;
            ChatScreen.Visibility = Visibility.Visible;
        }
    }
}
