using Grpc.Net.Client;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TelegramBot;
using TelegramBotClient.Commands;
using TelegramBotClient.Model;
using TelegramBotClient.Properties;
using TelegramBotClient.Views;

namespace TelegramBotClient.ViewModels
{
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        private readonly long _userId;

        public ObservableCollection<EventReminder> EventReminders { get; }
        
        public EventReminder SelectedReminder { get; set; }
        public Command AddCommand { get; private set; }
        public Command RemoveCommand { get; private set; }
        public Command EditCommand { get; private set; }

        public MainViewModel(long userId, ObservableCollection<EventReminder> eventReminders)
        {
            _userId = userId;
            EventReminders = eventReminders;

            AddCommand = new Command(commandParameter =>
            {
                var addReminderWindow = new AddReminderWindow(new ReminderViewModel(_userId, EventReminders));
                addReminderWindow.Show();
            }, _ => true);

            RemoveCommand = new Command(commandParameter =>
            {
                var channel = GrpcChannel.ForAddress(Settings.Default.Address);
                var client = new TelegramEventService.TelegramEventServiceClient(channel);

                if (SelectedReminder == null) return;

                client.RemoveReminder(new Reminder
                {
                    Id = SelectedReminder.Id,
                    UserId = _userId,
                });

                foreach(var reminder in EventReminders)
                {
                    if(reminder.Id == SelectedReminder.Id)
                    {
                        EventReminders.Remove(reminder);
                        break;
                    }
                }
            }, _ => true );
            EditCommand = new Command(commandParameter =>
            {
                if (SelectedReminder == null) return;

                var editReminderWindow = new AddReminderWindow(new EditViewModel(_userId, SelectedReminder, EventReminders));
                editReminderWindow.Show();
            }, _ => true);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
