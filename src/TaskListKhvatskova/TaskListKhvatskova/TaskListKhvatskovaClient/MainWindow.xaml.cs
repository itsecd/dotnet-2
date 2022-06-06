using ReactiveUI;
using System.Reactive.Linq;
using TaskListKhvatskovaClient.ViewModels;

namespace TaskListKhvatskovaClient
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

                cd.Add(ViewModel.CreateTag.RegisterHandler(interaction =>
                {
                    var tagViewModel = new AddTagViewModel();
                    var tagWindow = new AddTag
                    {
                        Owner = this,
                        ViewModel = tagViewModel
                    };

                    return Observable.Start(() =>
                    {
                        _ = tagWindow.ShowDialog();
                    }, RxApp.MainThreadScheduler);
                }));

                cd.Add(ViewModel.CreateExecutor.RegisterHandler(interaction =>
                {
                    var executorViewModel = new AddExecutorViewModel();
                    var executorWindow = new AddExecutor
                    {
                        Owner = this,
                        ViewModel = executorViewModel
                    };

                    return Observable.Start(() =>
                    {
                        _ = executorWindow.ShowDialog();
                    }, RxApp.MainThreadScheduler);
                }));

                cd.Add(ViewModel.CreateTask.RegisterHandler(interaction =>
                {
                    var taskViewModel = new AddTaskViewModel();
                    var taskWindow = new AddTask
                    {
                        Owner = this,
                        ViewModel = taskViewModel
                    };

                    return Observable.Start(() =>
                    {
                        _ = taskWindow.ShowDialog();
                    }, RxApp.MainThreadScheduler);
                }));
            });
        }
    }
}
