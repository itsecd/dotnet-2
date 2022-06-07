using BotClient.Commands;
using System.ComponentModel;
using System.Net.Http;
using System.Windows;

namespace BotClient.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private int _userId;
        public int UserId
        {
            get => _userId;
            set
            {
                if (value == _userId) return;
                _userId = value;
                OnPropertyChanged(nameof(UserId));
            }
        }

        public LoginViewModel()
        {
            OkCommand = new Command(async commandParameter =>
            {
                if (commandParameter is not Window window) return;
                using var httpClient = new HttpClient();
                var telegramBotServer = new TelegramBotServer(Properties.Settings1.Default.OpenApiServer, httpClient);
                var user = await telegramBotServer.UserAsync(UserId);
                if (user != null)
                {
                    var mainWindow = new MainWindow(new MainViewModel { User = user });
                    mainWindow.Show();
                    window.Close();
                }
                else
                {
                    MessageBox.Show("Указанный пользователь не найден", "Ошибка", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }, _ => true);
        }

        public Command OkCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
