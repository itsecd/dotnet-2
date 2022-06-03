using ChatServiceClient.ViewModel;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;

namespace ChatServiceClient
{
    public class AuthWindowBase : ReactiveWindow<AuthWindowViewModel>
    {
    }

    public partial class AuthWindow : AuthWindowBase
    {
        public AuthWindow()
        {
            InitializeComponent();

            _ = this.WhenActivated(cd =>
            {
                if (ViewModel is null)
                    return;

                ViewModel.AuthWindow = this;
                cd.Add(ViewModel.OpenChatWindow.RegisterHandler(interaction =>
                {
                    var chatWindow = new ChatWindow();
                    var chatWindowViewModel = new ChatWindowViewModel(interaction.Input, chatWindow);
                    chatWindow.ViewModel = chatWindowViewModel;
                    Observable.Start(() =>
                    {
                        _ = chatWindow.ShowDialog();
                    }, RxApp.MainThreadScheduler);
                }));
            });


        }
    }
}
