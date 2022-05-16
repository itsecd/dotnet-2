using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using Avalonia.Controls;
using ReactiveUI;
using MinesweeperClient.Views;

namespace MinesweeperClient.ViewModels
{

    public class MainWindowViewModel : ViewModelBase
    {
        Window _mainWindow;
        public ReactiveCommand<Unit, Unit> JoinCommand { get; }
        public ReactiveCommand<Unit, Unit> LeaveCommand { get; }
        public MainWindowViewModel(Window MainWindow)
        {
            _mainWindow = MainWindow;

            JoinCommand = ReactiveCommand.Create(JoinImplAsync);
            LeaveCommand = ReactiveCommand.Create(LeaveImpl);
        }
        private async void JoinImplAsync()
        {
            JoinDialog joinDialog = new();
            string[] vals = await joinDialog.ShowDialog<string[]>(_mainWindow);
            if (vals == null)
                Console.WriteLine("JoinDialog closed");
            else
                Console.WriteLine($"'{vals[0]}' '{vals[1]}'");
        }
        private void LeaveImpl()
        {

        }
    }
}
