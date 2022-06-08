using Chat.ViewModel;
using ReactiveUI;
using System.Reactive.Linq;

namespace Chat
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    /// 
    public class LoginWindowBase : ReactiveWindow<LoginViewModel>   
    {
    }
    public partial class LoginWindow : LoginWindowBase
    {
        public LoginWindow()
        {
            InitializeComponent();

            _ = this.WhenActivated(cd =>
            {
                if (ViewModel is null)
                    return;

                ViewModel.LogWindow = this;
                cd.Add(ViewModel.OpenChatWindow.RegisterHandler(interaction =>
                {
                    var chatWindow = new MainWindow();
                    var chatWindowViewModel = new MainWindowViewModel(interaction.Input, chatWindow);
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
