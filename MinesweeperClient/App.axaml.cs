using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MinesweeperClient.ViewModels;
using MinesweeperClient.Views;
using MinesweeperClient.Models;

namespace MinesweeperClient
{
    public partial class App : Application
    {
        private readonly Connection _wire = new Connection();
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow{Wire = _wire};
                desktop.MainWindow.DataContext = new MainWindowViewModel(desktop.MainWindow, _wire);
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}