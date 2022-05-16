using System;
using System.Collections.Generic;
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
        const int TILE_SIZE = 20;
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
        public MainWindow()
        {
            InitializeComponent();
            // assets
            _flag = new Bitmap("Assets/Flag.png");
            _bomb = new Bitmap("Assets/Bomb.png");
            _bombMarked = new Bitmap("Assets/BombMarked.png");
            // field
            _grid = this.FindControl<Panel>("game_grid");
            InitGrid();
            // info
            _playerList = this.FindControl<ItemsControl>("player_list");
            _flagsLabel = this.FindControl<Label>("flags_label");
            _flagsCounter = 99;
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
                        FontSize = 20,
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

        private void OnTileClicked(object? sender, PointerPressedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}