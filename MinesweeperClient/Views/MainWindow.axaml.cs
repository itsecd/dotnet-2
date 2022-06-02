using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using MinesweeperClient.Models;

namespace MinesweeperClient.Views
{
    public partial class MainWindow : Window
    {
        const int FIELD_WIDTH = 30;
        const int FIELD_HEIGHT = 16;
        const int MINE_COUNT = 99;
        const int TILE_SIZE = 35;
        // field
        private MinesweeperField _field = new(FIELD_HEIGHT, FIELD_WIDTH);
        private Panel _grid;
        private Button[,] _buttonGrid = new Button[FIELD_HEIGHT, FIELD_WIDTH];
        // assets
        Bitmap _flag;
        Bitmap _bomb;
        Bitmap _bombMarked;
        // info
        List<PlayerInfo> _players = new();
        ItemsControl _playerList;
        Label _flagsLabel;
        int _flagsCounter;
        // game
        GameStatus _gameStatus;
        public Connection? _wire;
        Button _readyButton;
        public MainWindow()
        {
            InitializeComponent();
            // assets
            _flag = new Bitmap("Assets/Flag.png");
            _bomb = new Bitmap("Assets/Bomb.png");
            _bombMarked = new Bitmap("Assets/BombMarked.png");
            // field
            _grid = this.FindControl<Panel>("game_grid");
            _readyButton = this.FindControl<Button>("ready_button");
            InitGrid();
            // info
            _playerList = this.FindControl<ItemsControl>("player_list");
            _playerList.Items = _players;
            // _players.Add(new PlayerInfo{Name="Player1", PlayCount=69, WinCount=69, WinStreak=420});
            // _players.Clear();
            // _players.Add(new PlayerInfo{Name="Player1", PlayCount=69, WinCount=69, WinStreak=420});
            _flagsLabel = this.FindControl<Label>("flags_label");
            _flagsCounter = 99;
            // connection
            // Task.Run(() => PlayersUpdate());
            Task.Run(() => FlagsUpdate());
            Task.Factory.StartNew(() => PlayersUpdate());
        }
        async void PlayersUpdate()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (_wire != null && _wire.IsConnected)
                {
                    await _wire.UpdatePlayers();
                    _players.Clear();
                    foreach (PlayerInfo info in _wire.Players)
                    {
                        Console.WriteLine($"got {info.Name}'s stats, {info.PlayCount}/{info.WinCount}/{info.WinStreak}");
                        _players.Add(info);
                    }
                    // _playerList.Items = _players;
                }
            }
        }
        void FlagsUpdate()
        {
            int flags_cache = _flagsCounter;
            while (true)
            {
                Thread.Sleep(100);
                if (_flagsCounter != flags_cache)
                {
                    flags_cache = _flagsCounter;
                    _flagsLabel.Content = $"Flags left: {flags_cache}";
                }
            }
        }
        private void InitGrid()
        {
            _grid.Height = FIELD_HEIGHT * TILE_SIZE;
            _grid.Width = FIELD_WIDTH * TILE_SIZE;
            for (int y = 0; y < FIELD_HEIGHT; y++)
            {
                for (int x = 0; x < FIELD_WIDTH; x++)
                {
                    _buttonGrid[y, x] = new Button
                    {
                        Name = $"Tile_{x}_{y}",
                        Height = TILE_SIZE,
                        Width = TILE_SIZE,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(x * TILE_SIZE, y * TILE_SIZE, 0, 0),
                        Background = new SolidColorBrush { Color = Color.Parse("#c0c0c0") },
                        BorderThickness = new Thickness(1),
                        BorderBrush = new SolidColorBrush { Color = Color.Parse("#808080") },
                        // FontSize = 20,
                        // FontWeight = FontWeight.Bold,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        IsEnabled = false
                    };
                    _buttonGrid[y, x].AddHandler(PointerPressedEvent, OnTileClicked, RoutingStrategies.Tunnel);
                    _grid.Children.Add(_buttonGrid[y, x]);
                }
            }
        }
        private void ResetGrid()
        {
            for (int y = 0; y < FIELD_HEIGHT; y++)
            {
                for (int x = 0; x < FIELD_WIDTH; x++)
                {
                    _buttonGrid[y, x].Content = string.Empty;
                    _buttonGrid[y, x].IsEnabled = true;
                }
            }
        }
        private void DrawGrid()
        {
            for (int y = 0; y < FIELD_HEIGHT; y++)
            {
                for (int x = 0; x < FIELD_WIDTH; x++)
                {
                    switch (_field.TileState(x, y))
                    {
                        case TileStates.Opened:
                            if (_field[x, y] == -1)
                                _buttonGrid[y, x].Content = new Image { Source = _bomb };
                            else if (_field[x, y] == 0)
                                _buttonGrid[y, x].Content = string.Empty;
                            else
                            {
                                SolidColorBrush? numColor = _field[x, y] switch
                                {
                                    1 => new SolidColorBrush(Color.Parse("#0000ff")),
                                    2 => new SolidColorBrush(Color.Parse("#008000")),
                                    3 => new SolidColorBrush(Color.Parse("#ff0000")),
                                    4 => new SolidColorBrush(Color.Parse("#000080")),
                                    5 => new SolidColorBrush(Color.Parse("#800000")),
                                    6 => new SolidColorBrush(Color.Parse("#008080")),
                                    7 => new SolidColorBrush(Color.Parse("#000000")),
                                    8 => new SolidColorBrush(Color.Parse("#808080")),
                                    _ => null
                                };
                                _buttonGrid[y, x].Content = new Label
                                {
                                    Foreground = numColor,
                                    FontSize = TILE_SIZE - 20,
                                    Content = _field[x, y].ToString(),
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Center,
                                    VerticalContentAlignment = VerticalAlignment.Center,
                                    HorizontalContentAlignment = HorizontalAlignment.Center
                                };
                            }
                            _buttonGrid[y, x].IsEnabled = false;
                            break;
                        case TileStates.Flagged:
                            _buttonGrid[y, x].Content = new Image { Source = _flag };
                            break;
                        case TileStates.Marked:
                            _buttonGrid[y, x].Content = new Image { Source = _bombMarked };
                            _buttonGrid[y, x].IsEnabled = false;
                            break;
                        default:
                            _buttonGrid[y, x].Content = string.Empty;
                            _buttonGrid[y, x].Foreground = _buttonGrid[y, x].Background;
                            break;
                    }
                }
            }
        }
        private void OnTileClicked(object? sender, PointerPressedEventArgs e)
        {
            // не обрабатываем нажатия, если игра не идет
            if (_gameStatus != GameStatus.InProgress)
                return;
            // получение координат нажатой клетки
            Button? button = sender as Button;
            if (button == null || button.Name == null || button.IsEnabled == false)
                return;
            int x = int.Parse(button.Name.Split("_")[1]);
            int y = int.Parse(button.Name.Split("_")[2]);
            // генерация поля при нажатии на пустую клетку
            if (_field.GameState() == GameStatus.Ready)
                _field.Generate(x, y, MINE_COUNT);
            // обработка кнопок мышки
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                Console.WriteLine($"[{x};{y}] L");
                if (_field.TileState(x, y) != TileStates.Flagged)
                {
                    if (_field[x, y] == -1)
                        _field.RevealMines();
                    else
                        _field.Reveal(x, y);
                }
            }
            else if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
            {
                Console.WriteLine($"[{x};{y}] R");
                _field.SwitchFlag(x, y);
                if (_field.TileState(x, y) == TileStates.Flagged)
                    _flagsCounter--;
                else
                    _flagsCounter++;
            }
            // отрисовка поля и проверка состояния игры
            DrawGrid();
            _gameStatus = _field.GameState();
            if (_field.GameState() == GameStatus.Win)
                Console.WriteLine("You won!");
            if (_field.GameState() == GameStatus.Lose)
                Console.WriteLine("You lose!");
        }
        private async void OnReadyClicked(object sender, RoutedEventArgs e)
        {
            if (_wire == null || !_wire.IsConnected)
                return;
            if (await _wire.Ready())
                _gameStatus = GameStatus.InProgress;
            ResetGrid();
        }
    }
}