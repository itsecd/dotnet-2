using System;
using Avalonia;
using Avalonia.Layout;
using Avalonia.Styling;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Avalonia.Input;
using Avalonia.Threading;
using System.IO;

using MinesweeperClient.Models;

namespace MinesweeperClient
{
    public partial class MainWindow : Window
    {
        const int FIELD_WIDTH = 30;
        const int FIELD_HEIGHT = 16;
        const int MINE_COUNT = 99;
        const int TILE_SIZE = 35;
        private MinesweeperField _field = new(FIELD_HEIGHT, FIELD_WIDTH);
        private Grid _grid;
        private Button[,] _buttonGrid = new Button[FIELD_HEIGHT, FIELD_WIDTH];
        private GameStates _gameState;
        Bitmap _flag;
        Bitmap _bomb;
        Bitmap _bombMarked;
        Label _flagsLeft;
        int _flagsCounter;
        ListBox _playerList;
        public MainWindow()
        {
            InitializeComponent();
            _flag = new Bitmap(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Flag.png"));
            _bomb = new Bitmap(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Bomb.png"));
            _bombMarked = new Bitmap(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Assets", "BombMarked.png"));
            _grid = this.FindControl<Grid>("FieldGrid");
            _flagsLeft = this.FindControl<Label>("FlagsLeft");
            _gameState = GameStates.Lose;
            _playerList = this.FindControl<ListBox>("PlayerListBox");
            InitGrid();
            InitInfo();
            Icon = new WindowIcon(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Assets", "AppIcon.png"));
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
                        Name = $"Rectangle_{x}_{y}",
                        Height = TILE_SIZE,
                        Width = TILE_SIZE,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(x * TILE_SIZE, y * TILE_SIZE, 0, 0),
                        Background = new SolidColorBrush { Color = Color.Parse("#c0c0c0") },
                        BorderThickness = new Thickness(1, 1, 1, 1),
                        BorderBrush = new SolidColorBrush { Color = Color.Parse("#808080") },
                        FontSize = 20,
                        FontWeight = FontWeight.Bold,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    };
                    _buttonGrid[y, x].AddHandler(PointerPressedEvent, OnTileClicked, RoutingStrategies.Tunnel);
                    _grid.Children.Add(_buttonGrid[y, x]);
                }
            }
        }
        private void InitInfo()
        {
            _flagsCounter = 99;
            _flagsLeft.FontSize = 20;
            _flagsLeft.Content = "Flags left: 99";
        }
        private void DrawInfo()
        {
            _flagsLeft.Content = $"Flags left: {_flagsCounter}";
        }
        private void ResetInfo()
        {
            _flagsCounter = 99;
            _flagsLeft.Content = "Flags left: 99";
        }
        private void OnResetClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Game started!");
            _field.Reset();
            ResetGrid();
            ResetInfo();
            _gameState = GameStates.Ready;
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
                    switch (_field.RevealState(x, y))
                    {
                        case RevealStates.Opened:
                            if (_field[x, y] == -1)
                                _buttonGrid[y, x].Content = new Image { Source = _bomb };
                            else if (_field[x, y] == 0)
                                _buttonGrid[y, x].Content = string.Empty;
                            else
                                _buttonGrid[y, x].Content = _field[x, y].ToString();
                            _buttonGrid[y, x].IsEnabled = false;
                            break;
                        case RevealStates.Flagged:
                            _buttonGrid[y, x].Content = new Image { Source = _flag };
                            break;
                        case RevealStates.BombMarked:
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
        private void OnTileClicked(object sender, PointerPressedEventArgs e)
        {
            // не обрабатываем нажатия, если игра окончена
            if (_gameState == GameStates.Win || _gameState == GameStates.Lose)
                return;
            // получение координат нажатой клетки
            Button button = sender as Button;
            string[] buttonPos = button.Name.Split("_");
            int x = int.Parse(buttonPos[1]);
            int y = int.Parse(buttonPos[2]);
            // генерация поля при нажатии на пустую клетку
            if (_gameState == GameStates.Ready)
                _field.Generate(MINE_COUNT, x, y);
            // обработка кнопок мышки
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                Console.WriteLine($"[{x};{y}] left_click");
                if (_field.RevealState(x, y) != RevealStates.Flagged)
                {
                    if (_field[x, y] == -1)
                        _field.RevealMines();
                    else
                        _field.Reveal(x, y);
                }
            }
            else if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
            {
                Console.WriteLine($"[{x};{y}] right_click");
                _field.SwitchFlag(x, y);
                if (_field.RevealState(x, y) == RevealStates.Flagged)
                    _flagsCounter--;
                else
                    _flagsCounter++;
            }
            // отрисовка поля и проверка состояния игры
            DrawGrid();
            DrawInfo();
            _gameState = _field.GameState();
            if (_field.GameState() == GameStates.Win)
                Console.WriteLine("You won!");
            if (_field.GameState() == GameStates.Lose)
                Console.WriteLine("You lose!");
        }
    }
}