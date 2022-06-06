using BotClient.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BotClient.ViewModels
{
    class AddReminderViewModel
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
                OkCommand = new Command(async _ =>
                {
                    using var httpClient = new HttpClient();
                    var telegramBotServer = new TelegramBotServer("", httpClient);
                    await telegramBotServer.UserAsync(UserId);
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                }, null);
            }

            public Command OkCommand { get; private set; }

            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
