using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using TelegramBot;
using TelegramBotClient.Commands;
using TelegramBotClient.Model;
using TelegramBotClient.Properties;
using Duration = Google.Protobuf.WellKnownTypes.Duration;

namespace TelegramBotClient.ViewModels
{
    public sealed class EditViewModel : INotifyPropertyChanged
    {
        private readonly long _userId;
        private readonly ObservableCollection<EventReminder> _eventReminders;

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Time { get; set; }
        public string RepeatPeriod { get; set; }
        public Command OkCommand { get; private set; }
        public Command CancelCommand { get; private set; }

        public EditViewModel(long userId, EventReminder selectedReminder, ObservableCollection<EventReminder> eventReminders)
        {
            _userId = userId;
            Id = selectedReminder.Id;
            Name = selectedReminder.Name;
            Description = selectedReminder.Description;
            Time = selectedReminder.Time.ToString();
            RepeatPeriod = selectedReminder.RepeatPeriod.ToString();
            _eventReminders = eventReminders;

            OkCommand = new Command(commandParameter =>
            {
                var window = (Window)commandParameter;
                var channel = GrpcChannel.ForAddress(Settings.Default.Address);
                var client = new TelegramEventService.TelegramEventServiceClient(channel);

                if (Time == null || RepeatPeriod == null) return;

                var time = DateTime.Parse(Time);
                var repeatPeriod = TimeSpan.Parse(RepeatPeriod);
                client.ChangeReminder(new Reminder
                {
                    Id = Id,
                    UserId = _userId,
                    Name = Name,
                    Description = Description,
                    DateTime = Timestamp.FromDateTime(time.ToUniversalTime()),
                    RepeatPeriod = Duration.FromTimeSpan(repeatPeriod)
                });
                foreach(var reminder in _eventReminders)
                {
                    if(reminder.Id == Id)
                    {
                        _eventReminders.Remove(reminder);
                        _eventReminders.Add(new EventReminder
                        {
                            Id = Id,
                            Name = Name,
                            Description = Description,
                            Time = time,
                            RepeatPeriod = repeatPeriod
                        });
                        break;
                    }
                }
                window.Close();
            }, _ => true);
            CancelCommand = new Command(commandParameter =>
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
