using System.Windows;
using TaskClient.ViewModel;

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
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(async () => await ((MainTaskViewModel)DataContext).InitializeAsync());
        }
    }
}
