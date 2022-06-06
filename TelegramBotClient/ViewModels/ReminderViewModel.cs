using System;
using System.Reactive;
using ReactiveUI;
using TelegramBotClient.Model;

namespace TelegramBotClient.ViewModels
{
    public sealed class ReminderViewModel : ReactiveObject
    {
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? Time { get; set; } = string.Empty;
        public string? RepeatPeriod { get; set; } = string.Empty;

        public ReactiveCommand<Unit, Unit> OkCommand { get; }
        public ReactiveCommand<Unit, Unit> CanselCommand { get; }
        public Interaction<EventReminder?, Unit> CloseCommand { get; } = new(RxApp.MainThreadScheduler);

        public ReminderViewModel()
        {
            var canExecute = this.WhenAnyValue(
                o => o.Name, o => o.Description, o => o.Time, o => o.RepeatPeriod,
                (name, description, time, repeatPeriod) =>
                    !string.IsNullOrWhiteSpace(name) && 
                    !string.IsNullOrWhiteSpace(description) && 
                    !string.IsNullOrWhiteSpace(time) && DateTime.TryParse(time, out var timeData) &&
                    !string.IsNullOrWhiteSpace(repeatPeriod) && TimeSpan.TryParse(repeatPeriod, out var periodData));

            OkCommand = ReactiveCommand.CreateFromObservable(OkImpl, canExecute);
            CanselCommand = ReactiveCommand.CreateFromObservable(CancelImpl);
        }

        private IObservable<Unit> OkImpl()
        {
            var reminder = new EventReminder
            {
                Name = Name,
                Description = Description,
                Time = DateTime.Parse(Time!),
                RepeatPeriod = TimeSpan.Parse(RepeatPeriod!)
            };
            return CloseCommand.Handle(reminder);
        }

        private IObservable<Unit> CancelImpl()
        {
            return CloseCommand.Handle(null);
        }
    }
}
