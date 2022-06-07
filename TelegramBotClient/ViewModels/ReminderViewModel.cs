using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using TelegramBot;
using TelegramBotClient.Commands;
using TelegramBotClient.Model;
using TelegramBotClient.Properties;

using Duration = Google.Protobuf.WellKnownTypes.Duration;

namespace TelegramBotClient.ViewModels
{
    public sealed class ReminderViewModel : INotifyPropertyChanged
    {
        private readonly long _userId;
        
        public ObservableCollection<EventReminder> EventReminders;
        public string Name { get; set; }
        public string Description { get; set; }
        public string Time { get; set; }
        public string RepeatPeriod { get; set; }
        public Command OkCommand { get; private set; }
        public Command CanselCommand { get; private set; }

        public ReminderViewModel(long userId, ObservableCollection<EventReminder> eventReminders)
        {
            _userId = userId;
            EventReminders = eventReminders;

            OkCommand = new Command(commandParameter =>
            {
                var window = (Window)commandParameter;
                var channel = GrpcChannel.ForAddress(Settings.Default.Address);
                var client = new TelegramEventService.TelegramEventServiceClient(channel);
                var time = DateTime.Parse(Time);
                var repeatPeriod = TimeSpan.Parse(RepeatPeriod);
                client.AddReminder(new Reminder
                {
                    Id = 0,
                    UserId = _userId,
                    Name = Name,
                    Description = Description,
                    DateTime = Timestamp.FromDateTime(time.ToUniversalTime()),
                    RepeatPeriod = Duration.FromTimeSpan(repeatPeriod)
                });
                EventReminders.Add(new EventReminder
                {
                    Id = EventReminders.Max(x => x.Id) + 1,
                    Name = Name,
                    Description = Description,
                    Time = time,
                    RepeatPeriod = repeatPeriod
                });
                window.Close();
            }, _ => true);
            CanselCommand = new Command(commandParameter =>
            {
                var window = (Window)commandParameter;
                window.Close();
            }, _ => true);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
