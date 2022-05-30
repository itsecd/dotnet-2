using System;
using System.Reactive;
using System.Windows;
using OrderAccountingSystemClient.ViewModels;
using ReactiveUI;

namespace OrderAccountingSystemClient
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
            var mainWindowViewModel = new MainWindowViewModel();
            var mainWindow = new MainWindow
            {
                ViewModel = mainWindowViewModel
            };
            Current.MainWindow = MainWindow;
            mainWindow.Show();
        }
    }
}

