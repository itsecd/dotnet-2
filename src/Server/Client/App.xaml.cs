using System;
using System.Reactive;
using System.Windows;
using Client.ViewModels;
using ReactiveUI;

namespace Client
{
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
            var authWindow = new AuthWindow();
            var authViewModel = new AuthViewModel(authWindow);
            authWindow.ViewModel = authViewModel;
            Current.MainWindow = authWindow;
            authWindow.Show();
        }
    }
}
