using BotClient.Commands;
using BotClient.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BotClient.ViewModels
{
    public class AddReminderViewModel : INotifyPropertyChanged
    {
        private int _userId;
        private DateTime _dateTime;
        private string _name;
        private string _description;
        private int _repeatPeriod;
        public int UserId // нужно как то получить из LoginViewModel
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
            Ok = new Command(async _ =>
            {
                using var httpClient = new HttpClient();
                var telegramBotServer = new TelegramBotServer("/api/User/{id}/reminders", httpClient); // адрес взят из swagger.json, клиент не собирается с этой строкой кода
                await telegramBotServer.RemindersAsync(UserId, new Reminder { DateTime = _dateTime, Name = _name, Description = _description, RepeatPeriod = _repeatPeriod});
                // todo закрытие текущего окна AddReminderWindow и вызов метода show к ранее созданному классу MainWindow, на данный момент не знаю как выполнить данные задачи
            }, _ => true);
            Cancel = new Command(_ =>
            {
                // todo закрытие текущего окна AddReminderWindow и вызов метода show к ранее созданному классу MainWindow, на данный момент не знаю как выполнить данные задачи
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
