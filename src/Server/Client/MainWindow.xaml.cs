using System.Reactive.Linq;

using ReactiveUI;

using Client.ViewModels;
using Server.Model;

namespace Client
{
    // https://github.com/reactiveui/ReactiveUI/issues/2330#issuecomment-577968613
    public class MainWindowBase : ReactiveWindow<MainViewModel>
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

                cd.Add(ViewModel.CreateUserEvent.RegisterHandler(interaction =>
                {
                    var eventViewModel = new EventViewModel();
                    var eventView = new EventWindow
                    {
                        Owner = this,
                        ViewModel = eventViewModel
                    };

                    // No async version of ShowDialog...
                    return Observable.Start(() =>
                    {
                        _ = eventView.ShowDialog();
                        interaction.SetOutput(eventView.Tag as UserEvent);
                    }, RxApp.MainThreadScheduler);
                }));
                cd.Add(ViewModel.EditUserEvent.RegisterHandler(interaction =>
                {
                    var eventViewModel = new EventViewModel(){};
                    eventViewModel.EventName = ViewModel.SelectedUserEvent!.EventName;
                    eventViewModel.DateNTime = ViewModel.SelectedUserEvent!.DateNTime.ToString("dd.MM.yyyy HH:mm");
                    eventViewModel.EventFrequency = ViewModel.SelectedUserEvent!.EventFrequency;
                    var eventView = new EventWindow
                    {
                        Owner = this,
                        ViewModel = eventViewModel
                    };

                    // No async version of ShowDialog...
                    return Observable.Start(() =>
                    {
                        _ = eventView.ShowDialog();
                        interaction.SetOutput(eventView.Tag as UserEvent);
                    }, RxApp.MainThreadScheduler);
                }));
            });
        }
    }
}
