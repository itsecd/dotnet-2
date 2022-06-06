using System.ComponentModel;
using Grpc.Net.Client;
using TelegramBotClient.Properties;
using TelegramBotClient.Commands;
using TelegramBot;

namespace TelegramBotClient.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private long _userId;
        public long UserId
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
            OkCommand = new Command(_ =>
            {
                var channel = GrpcChannel.ForAddress(Settings.Default.Address);
                var client = new TelegramEventService.TelegramEventServiceClient(channel);
                var userReminders = client.GetReminders(new UserRequest { UserId = _userId });
                var mainWindow = new MainWindow();
                mainWindow.Show();
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
