using ReactiveUI;
using System;
using System.Reactive;
using System.Windows;

namespace BotClient
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
    }
}
