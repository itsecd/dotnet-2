using System.Windows;
using System.Windows.Controls;

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
        }

        private void NewGameClick(object sender, RoutedEventArgs e)
        {
            BusyIndicator.IsBusy = true;
        }

        private void PlaygroudClick(object sender, RoutedEventArgs e)
        {
            Button cell = ((Button)e.OriginalSource);
            if (client.IsFirstTurn)
                cell.Content = "X";
            else
                cell.Content = "O";
            var pointString = cell.Name.Replace("Button_", "").Split("_");
            int x = int.Parse(pointString[0]);
            int y = int.Parse(pointString[1]);
            LockPlayground();
            NewGameButton.Visibility = Visibility.Visible;
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

    }
}
