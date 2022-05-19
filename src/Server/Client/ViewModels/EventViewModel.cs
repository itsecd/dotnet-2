using System;
using System.Reactive;
using System.Windows;
using ReactiveUI;

using Server.Model;

namespace Client.ViewModels
{
    public sealed class EventViewModel : ReactiveObject
    {
        public string EventName { get; set; } = string.Empty;

        public string DateNTime { get; set; } = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

        public int EventFrequency { get; set; } = 1;

        public ReactiveCommand<Unit, Unit> Ok { get; }

        public ReactiveCommand<Unit, Unit> Cancel { get; }

        public Interaction<UserEvent?, Unit> Close { get; } = new(RxApp.MainThreadScheduler);

        public EventViewModel()
        {
            Ok = ReactiveCommand.CreateFromObservable(OkImpl);
            Cancel = ReactiveCommand.CreateFromObservable(CancelImpl);
        }

        private bool IsValidName()
        {
            if (string.IsNullOrEmpty(EventName))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(EventName))
            {
                return false;
            }
            if(EventName.Contains("  "))
            {
                return false;
            }
            return true;
        }

        private bool IsValidDateNTime()
        {
            return DateTime.TryParse(DateNTime, out _);
        }

        private bool IsValidFreq()
        {
            return (EventFrequency > 0 && EventFrequency < 8);
        }

        private IObservable<Unit> OkImpl()
        {
            if (!IsValidName())
            {
                MessageBox.Show("Event name is invalid");
                return Close.Handle(null);
            }
            if (!IsValidDateNTime())
            {
                MessageBox.Show("Event date and time is invalid");
                return Close.Handle(null);
            }
            if (!IsValidFreq())
            {
                MessageBox.Show("Event frequency is invalid");
                return Close.Handle(null);
            }
            var userEvent = new UserEvent
            {
                EventName = EventName,
                DateNTime = DateTime.Parse(DateNTime),
                EventFrequency = EventFrequency
            };
            return Close.Handle(userEvent);
        }

        private IObservable<Unit> CancelImpl()
        {
            return Close.Handle(null);
        }
    }
}
