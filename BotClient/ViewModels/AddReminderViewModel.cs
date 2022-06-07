using BotClient.Commands;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Windows;

namespace BotClient.ViewModels
{
    public class AddReminderViewModel : INotifyPropertyChanged
    {
        private User _user;
        private long _userId;
        private DateTime _dateTime;
        private string _name;
        private string _description;
        private int _repeatPeriod;

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

        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged(nameof(_name));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (value == _description) return;
                _description = value;
                OnPropertyChanged(nameof(_description));
            }
        }

        public DateTime DateTime
        {
            get => _dateTime;
            set
            {
                if (value == _dateTime) return;
                _dateTime = value;
                OnPropertyChanged(nameof(_dateTime));
            }
        }

        public int RepeatPeriod
        {
            get => _repeatPeriod;
            set
            {
                if (value == _repeatPeriod) return;
                _repeatPeriod = value;
                OnPropertyChanged(nameof(_dateTime));
            }
        }

        public AddReminderViewModel()
        {
            Ok = new Command(async commandParameter =>
            {
                if (commandParameter is not Window window) return;
                using var httpClient = new HttpClient();
                var telegramBotServer = new TelegramBotServer(Properties.Settings1.Default.OpenApiServer, httpClient);
                await telegramBotServer.RemindersAsync(((int)UserId), new Reminder { DateTime = _dateTime, Name = _name, Description = _description, RepeatPeriod = _repeatPeriod});
                window.Close();
            }, _ => true);
            Cancel = new Command(commandParameter =>
            {
                if (commandParameter is not Window window) return;
                window.Close();
            }, _ => true);
        }

        public Command Ok { get; private set; }
        
        public Command Cancel { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
