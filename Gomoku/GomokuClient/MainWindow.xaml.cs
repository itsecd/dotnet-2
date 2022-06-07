using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Gomoku;

namespace GomokuClient
{
    public partial class MainWindow : Window
    {
        Button[,] _playground = new Button[15, 15];

        private Client client;

        public MainWindow(Client client)
        {
            InitializeComponent();
            for (int i = 0; i < _playground.GetLength(0); ++i)
                for (int j = 0; j < _playground.GetLength(1); ++j)
                {
                    _playground[i, j] = new Button();
                    _playground[i, j].Name = $"Button_{i}_{j}";
                    Grid.SetColumn(_playground[i, j], i);
                    Grid.SetRow(_playground[i, j], j);
                    PlaygroundGrid.Children.Add(_playground[i, j]);
                }
            PlaygroundGrid.AddHandler(Button.ClickEvent, new RoutedEventHandler(PlaygroudClick));
            this.client = client;
            Player1.Content = client.Login;
            Player2.Content = client.OpponentLogin;
            LockOrUnlock();
            this.client.MakeTurnSubject.Subscribe(SetOpponentTurn);
            this.client.EndGameSubject.Subscribe(EndGame);
        }

        private void NewGameClick(object sender, RoutedEventArgs e)
        {
            BusyIndicator.IsBusy = true;
        }

        private void PlaygroudClick(object sender, RoutedEventArgs e)
        {
            Button cell = ((Button)e.OriginalSource);
            if (cell.Content is not null)
                return;
            if (client.IsFirstPlayer)
                cell.Content = "X";
            else
                cell.Content = "O";
            var pointString = cell.Name.Replace("Button_", "").Split("_");
            int x = int.Parse(pointString[0]);
            int y = int.Parse(pointString[1]);
            LockPlayground();
            Task sumTask = new Task(async () =>
            {
                await client.MakeTurnRequest(new Gomoku.Point() { X = x, Y = y });
            });
            sumTask.Start();
        }

        private void SetOpponentTurn(MakeTurnReply makeTurnReply)
        {
            Dispatcher.InvokeAsync(() =>
            {
                LockOrUnlock();
            });

            if (makeTurnReply.YourTurn)
            {
                var point = makeTurnReply.Point;
                if (!client.IsFirstPlayer)
                {
                    Dispatcher.InvokeAsync(() => _playground[point.X, point.Y].Content = "X");
                }
                else
                    Dispatcher.InvokeAsync(() => _playground[point.X, point.Y].Content = "O");
            }
        }

        private void LockPlayground()
        {
            for (int i = 0; i < _playground.GetLength(0); ++i)
                for (int j = 0; j < _playground.GetLength(1); ++j)
                {
                    _playground[i, j].IsEnabled = false;
                }
        }

        private void UnlockPlayground()
        {
            for (int i = 0; i < _playground.GetLength(0); ++i)
                for (int j = 0; j < _playground.GetLength(1); ++j)
                {
                    _playground[i, j].IsEnabled = true;
                }
        }

        private void LockOrUnlock()
        {
            Dispatcher.InvokeAsync(() =>
            {
                if (client.ActivePlayer)
                {
                    ActivePlayerLabel.Content = $"Active player is {client.Login}";
                    UnlockPlayground();
                }
                else
                {
                    ActivePlayerLabel.Content = $"Active player is {client.OpponentLogin}";
                    LockPlayground();
                }
            });
        }

        private void EndGame(EndGameReply endGameReply)
        {
            Dispatcher.InvokeAsync(() =>
            {
                LockPlayground();
                ActivePlayerLabel.Content = $"{endGameReply.Status}";
                var points = endGameReply.Points;
                foreach (var point in points)
                    _playground[point.X, point.Y].Background = Brushes.Red;
                NewGameButton.Visibility = Visibility.Visible;
            });
        }
    }
}
