using System.Windows;
using Laba2Client.ViewModels;

namespace Laba2Client
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(async () => await ((MainViewModel)DataContext).InitializeAsync());
        }
    }
}