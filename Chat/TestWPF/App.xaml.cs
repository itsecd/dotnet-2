using ReactiveUI;
using System;
using System.Reactive;
using System.Windows;
using TestWPF.View;
using TestWPF.ViewModel;

namespace TestWPF
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
            var appViewModel = new AppViewModel();

            var Window = new ChatWindow()
            {
                ViewModel = appViewModel
            };

            Current.MainWindow = Window;
            Window.Show();
        }
    }
}
