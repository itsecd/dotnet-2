using Chat.ViewModel;
using ReactiveUI;
using System;
using System.Reactive;
using System.Windows;

namespace Chat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            RxApp.DefaultExceptionHandler = Observer.Create<Exception>(exception =>
            {
                System.Diagnostics.Debug.WriteLine($"RxApp Exception >>> {exception}");
            });
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var logWindow = new LoginWindow();
            var logViewModel = new LoginViewModel(logWindow);
            logWindow.ViewModel = logViewModel;
            Current.MainWindow = logWindow;
            logWindow.Show();
        }
    }
}
