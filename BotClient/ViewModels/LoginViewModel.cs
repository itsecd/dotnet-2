using BotClient.Commands;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
            MainWindow mainWindow = new MainWindow(this);
            OkCommand = new Command(async _ =>
            {
                using var httpClient = new HttpClient();
                var telegramBotServer = new TelegramBotServer("/api/User/{userid}", httpClient);  // адрес взят из swagger.json, клиент не собирается с этой строкой кода
                await telegramBotServer.UserAsync(UserId);
                mainWindow.Show();
                // todo закрытие LoginWindow из которого вызван OkCommand, на данный момент не знаю как это сделать
            }, _=> true);
        }

        public Command OkCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
