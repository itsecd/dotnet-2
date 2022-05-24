using Client.Services;
using Client.ViewModels;
using ReactiveUI;
using System.Reactive.Linq;


namespace Client
{
    public class AuthWindowBase : ReactiveWindow<AuthViewModel> 
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
                cd.Add(ViewModel.OpenMainWindow.RegisterHandler(interaction =>
                {
                    var userEventListService = new UserEventListService();
                    var mainViewModel = new MainViewModel(userEventListService, interaction.Input);
                    var mainView = new MainWindow
                    {
                        Owner = this,
                        ViewModel = mainViewModel
                    };

                    // No async version of ShowDialog...
                    return Observable.Start(() =>
                    {
                        _ = mainView.ShowDialog();
                    }, RxApp.MainThreadScheduler);
                }));
            });
        }
    }

   
}
