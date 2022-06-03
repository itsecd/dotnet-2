using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GomokuClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Button[,] _playground = new Button[15, 15];

        public MainWindow()
        {
            InitializeComponent();
            for(int i=0;i<_playground.GetLength(0);++i)
                for(int j = 0; j < _playground.GetLength(1); ++j)
                {
                    _playground[i,j] = new Button();
                    _playground[i, j].Name = $"Button_{i}_{j}";
                    Grid.SetColumn(_playground[i, j], i);
                    Grid.SetRow(_playground[i, j], j);
                    PlaygroundGrid.Children.Add(_playground[i, j]);
                    
                }
            PlaygroundGrid.AddHandler(Button.ClickEvent, new RoutedEventHandler(PlaygroudClick));



            

        }

        private void PlaygroudClick(object sender, RoutedEventArgs e)
        {
            Button button = ((Button)e.OriginalSource);
            button.Content = "X";
        }
    }
}
