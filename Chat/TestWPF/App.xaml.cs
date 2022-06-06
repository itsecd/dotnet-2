using ReactiveUI;
using System;
using System.Reactive;
using System.Windows;
using ChatClient.View;
using ChatClient.ViewModel;

namespace ChatClient
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
            var appViewModel = new AppViewModel();

            var window = new ChatWindow()
            {
                ViewModel = appViewModel
            };

            Current.MainWindow = window;
            window.Show();
        }
    }
}
