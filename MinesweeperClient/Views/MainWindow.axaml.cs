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
using Avalonia.Threading;
using MinesweeperClient.Models;
using MinesweeperClient.ViewModels;

namespace MinesweeperClient.Views
{
    public partial class MainWindow : Window
    {
        private const int FieldWidth = 30;
        private const int FieldHeight = 16;
        private const int MineCount = 99;
        private const int TileSize = 35;
        // field
        private readonly MinesweeperField _field = new(FieldHeight, FieldWidth);
        private readonly Panel _grid;
        private readonly Button[,] _buttonGrid = new Button[FieldHeight, FieldWidth];
        // assets
        private readonly Bitmap _flag;
        private readonly Bitmap _bomb;
        private readonly Bitmap _bombMarked;
        private readonly Label _flagsLabel;
        private int _flagsCounter;
        // game
        private GameStatus _gameStatus;
        public Connection? Wire;
        public MainWindow()
        {
            InitializeComponent();
            // assets
            _flag = new Bitmap("Assets/Flag.png");
            _bomb = new Bitmap("Assets/Bomb.png");
            _bombMarked = new Bitmap("Assets/BombMarked.png");
            // field
            _grid = this.FindControl<Panel>("GameGrid");
            InitGrid();
            _flagsLabel = this.FindControl<Label>("FlagsLabel");
            _flagsCounter = 99;
            // connection
            Task.Run(PlayersUpdate);
        }
        private async void PlayersUpdate()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (Wire is not { IsConnected: true }) continue;

                if (_gameStatus == GameStatus.InProgress)
                {
                    GameMessage msg = await Wire.Peek();
                    if (msg.State == "win")
                    {
                        Console.WriteLine("You lose!");
                        ResetGrid();
                        await Wire.Lose();
                        _gameStatus = GameStatus.Ready;
                    }
                }

                await Wire.UpdatePlayers();
                Dispatcher.UIThread.Post(() =>
                {
                    if (DataContext is not MainWindowViewModel viewModel) return;
                    viewModel.Players.Clear();
                    foreach (var info in Wire.Players)
                    {
                        Console.WriteLine($"got {info.Name}'s stats, {info.PlayCount}/{info.WinCount}/{info.WinStreak}");
                        viewModel.Players.Add(info);
                    }
                }, DispatcherPriority.Background);
            }
        }
        private void InitGrid()
        {
            _grid.Height = FieldHeight * TileSize;
            _grid.Width = FieldWidth * TileSize;
            for (int y = 0; y < FieldHeight; y++)
            {
                for (int x = 0; x < FieldWidth; x++)
                {
                    _buttonGrid[y, x] = new Button
                    {
                        Name = $"Tile_{x}_{y}",
                        Height = TileSize,
                        Width = TileSize,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(x * TileSize, y * TileSize, 0, 0),
                        Background = new SolidColorBrush { Color = Color.Parse("#c0c0c0") },
                        BorderThickness = new Thickness(1),
                        BorderBrush = new SolidColorBrush { Color = Color.Parse("#808080") },
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
            for (int y = 0; y < FieldHeight; y++)
            {
                for (int x = 0; x < FieldWidth; x++)
                {
                    _buttonGrid[y, x].Content = string.Empty;
                    _buttonGrid[y, x].IsEnabled = true;
                }
            }
        }
        private void DrawGrid()
        {
            for (int y = 0; y < FieldHeight; y++)
            {
                for (int x = 0; x < FieldWidth; x++)
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
                                    FontSize = 18,
                                    FontWeight = FontWeight.Bold,
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
        private async void OnTileClicked(object? sender, PointerPressedEventArgs e)
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
                _field.Generate(x, y, MineCount);
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
                _flagsLabel.Content = $"Flags left: {_flagsCounter}";
            }
            // отрисовка поля и проверка состояния игры
            DrawGrid();
            // _gameStatus = _field.GameState();
            if (_field.GameState() == GameStatus.Win)
            {
                Console.WriteLine("You won!");
                ResetGrid();
                await Wire.Win();
                _gameStatus = GameStatus.Ready;
            }
            if (_field.GameState() == GameStatus.Lose)
            {
                Console.WriteLine("You lose!");
                ResetGrid();
                await Wire.Lose();
                _gameStatus = GameStatus.Ready;
            }
        }
        private async void OnReadyClicked(object sender, RoutedEventArgs e)
        {
            if (Wire == null || !Wire.IsConnected)
                return;
            if (await Wire.Ready())
            {
                _gameStatus = GameStatus.InProgress;
            }
            ResetGrid();
        }
    }
}