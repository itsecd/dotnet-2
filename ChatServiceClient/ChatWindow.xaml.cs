using ChatServiceClient.ViewModel;
using ReactiveUI;
using System.Reactive.Linq;

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

            _ = this.WhenActivated(cd =>
            {
                if (ViewModel is null)
                    return;

                cd.Add(ViewModel.CloseChatWindow.RegisterHandler(interaction =>
                {
                    var authWindow = new AuthWindow();
                    var authViewModel = new AuthWindowViewModel(authWindow);
                    authWindow.ViewModel = authViewModel;
                    Observable.Start(() =>
                    {
                        _ = authWindow.ShowDialog();
                    }, RxApp.MainThreadScheduler);
                }));
            });
        }
    }
}
