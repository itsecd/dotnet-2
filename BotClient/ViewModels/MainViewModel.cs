using System.ComponentModel;
using BotClient.Commands;
using BotClient.Views;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace BotClient.ViewModels
{
    public class MainViewModel: INotifyPropertyChanged
    {
        private User _user;
        public User User
        {
            get => _user;
            set
            {
                if (value == _user) return;
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        public Reminder SelectedReminder { get; set; }

        public ObservableCollection<Reminder> Reminders { get; }

        public MainViewModel(User user)
        {
            _user = user;
            ObservableCollection<Reminder> reminders = new ObservableCollection<Reminder>();
            Reminders = reminders;
            foreach (var item in _user.ReminderList)
            {
                reminders.Add(item);
            }

            AddCall = new Command(_ =>
            {
                AddReminderWindow addReminderWindow = new AddReminderWindow(new AddReminderViewModel(Reminders) { UserId = _user.UserId });
                addReminderWindow.Show();
            }, _ => true);

            RemoveCall = new Command(async _ =>
            {
                if (SelectedReminder == null) return;
                using var httpClient = new HttpClient();
                var telegramBotServer = new TelegramBotServer(Properties.Settings1.Default.OpenApiServer, httpClient);
                await telegramBotServer.Reminders3Async((int)_user.UserId, SelectedReminder.Id);
                foreach (var reminder in Reminders)
                {
                    if (reminder.Id == SelectedReminder.Id)
                    {
                        Reminders.Remove(reminder);
                        break;
                    }
                }
            }, _ => true);
        }

        public Command AddCall { get; private set; }

        public Command RemoveCall { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}