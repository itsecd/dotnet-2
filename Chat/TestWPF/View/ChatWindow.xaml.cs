using ReactiveUI;
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
    }
}
