using ReactiveUI;
using System.Windows;
using TestWPF.ViewModel;

namespace TestWPF.View
{
    public class ChatWindowBase : ReactiveWindow<AppViewModel> { }
    public partial class ChatWindow : ChatWindowBase
    {
        public ChatWindow()
        {
            InitializeComponent();
        }
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Clear();
        }
    }
}
