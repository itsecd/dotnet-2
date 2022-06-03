using ChatServiceClient.ViewModel;
using ReactiveUI;

namespace ChatServiceClient
{
    public class ChatWindowBase : ReactiveWindow<ChatWindowViewModel>
    {
    }

    public partial class ChatWindow : ChatWindowBase
    {
        public ChatWindow()
        {
            InitializeComponent();
        }
    }
}
