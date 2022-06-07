using System;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.Layout;
using ReactiveUI;
using MinesweeperClient.Views;
using MinesweeperClient.Models;
using System.Collections.ObjectModel;

namespace MinesweeperClient.ViewModels
{

    public class MainWindowViewModel : ViewModelBase
    {
        private readonly Window _mainWindow;
        private readonly Connection _wire;
        public ReactiveCommand<Unit, Unit> JoinCommand { get; }
        public ReactiveCommand<Unit, Unit> LeaveCommand { get; }
        public ObservableCollection<PlayerInfo> Players { get; set; } = new();
        public MainWindowViewModel(Window mainWindow, Connection wire)
        {
            _mainWindow = mainWindow;
            _wire = wire;
            JoinCommand = ReactiveCommand.Create(JoinImplAsync);
            LeaveCommand = ReactiveCommand.Create(LeaveImpl);
        }
        private async void JoinImplAsync()
        {
            JoinDialog joinDialog = new();
            string[] vals = await joinDialog.ShowDialog<string[]>(_mainWindow);
            var msgBox = new Window
            {
                Content = new Label
                {
                    Content = "Не удалось подключиться к серверу!",
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                },
                Width = 350,
                Height = 50,
                CanResize = false,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Title = "MessageBox"
            };
            if (vals != null)
            {
                Console.WriteLine($"'{vals[0]}' '{vals[1]}'");
                if (await _wire.TryJoinAsync(vals[0], vals[1]))
                {
                    Console.WriteLine("Connected to server!");
                }
                else
                {
                    await msgBox.ShowDialog(_mainWindow);
                    return;
                }
            }
        }
        private async void LeaveImpl()
        {
            if (await _wire.Leave())
                Console.WriteLine("Disconnected from server!");
            else
                Console.WriteLine("Not disconnected from server!");
        }
    }
}
