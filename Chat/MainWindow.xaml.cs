using Chat.ViewModel;
using ReactiveUI;

namespace Chat
{
    public class MainWindowBase : ReactiveWindow<MainWindowViewModel>
    {
    }
    public partial class MainWindow : MainWindowBase
    {
        public MainWindow()
        {
            InitializeComponent();

            _ = this.WhenActivated(cd =>
            {
                if (ViewModel is null)
                    return;
            });
        }
    }
}