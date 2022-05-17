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
using System.Collections.ObjectModel;

namespace TaskClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Executors.ItemsSource = _list;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e) =>
            RemoveButton.IsEnabled = Executors.SelectedItem != null;

        private void OnAddClick(object sender, RoutedEventArgs e) =>
            _list.Add(_random.Next(5));

        private void OnRemoveClick(object sender, RoutedEventArgs e) =>
            _list.RemoveAt(Executors.SelectedIndex);

        private void OnExitClick(object sender, RoutedEventArgs e) => Close();

        private readonly ObservableCollection<int> _list = new ObservableCollection<int>();
        private readonly Random _random = new Random();
    }
}
