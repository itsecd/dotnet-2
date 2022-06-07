using System.ComponentModel;
using Grpc.Net.Client;
using TelegramBotClient.Properties;
using TelegramBotClient.Commands;
using TelegramBot;
using System.Windows;
using TelegramBotClient.Model;
using System.Collections.ObjectModel;

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
            OkCommand = new Command(commandParameter =>
            {
                var window = (Window)commandParameter;
                var channel = GrpcChannel.ForAddress(Settings.Default.Address);
                var client = new TelegramEventService.TelegramEventServiceClient(channel);
                var userReminders = client.GetReminders(new UserRequest { UserId = _userId });
                if (userReminders.Reminders[0].Id != -1)
                {
                    var reminders = new ObservableCollection<EventReminder>();
                    foreach (var reminder in userReminders.Reminders)
                    {
                        var time = reminder.DateTime.ToDateTime().ToLocalTime();
                        reminders.Add(new EventReminder
                        {
                            Id = reminder.Id,
                            Name = reminder.Name,
                            Description = reminder.Description,
                            Time = time,
                            RepeatPeriod = reminder.RepeatPeriod.ToTimeSpan()
                        });
                    }
                    var mainWindow = new MainWindow(new MainViewModel(_userId, reminders));
                    mainWindow.Show();
                    window.Close();
                }
                else
                {
                    MessageBox.Show("User not found", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
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
