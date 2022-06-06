using ReactiveUI;
using ChatClient.ViewModel;

namespace ChatClient.View
{
    public class ChatWindowBase : ReactiveWindow<AppViewModel> { }
    public partial class ChatWindow : ChatWindowBase
    {
        public ChatWindow()
        {
            InitializeComponent();
        }
    }
}
