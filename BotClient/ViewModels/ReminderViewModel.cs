using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Lab2Server.Models;

namespace BotClient.ViewModels
{
    public sealed class ReminderViewModel : ReactiveObject
    {
        [Reactive]
        public string? ReminderName { get; set; } = string.Empty;

        [Reactive]
        public string? DateNTime { get; set; } = string.Empty;

        [Reactive]
        public string? ReminderFrequency { get; set; } = string.Empty;

        public ReactiveCommand<Unit, Unit> Ok { get; }

        public ReactiveCommand<Unit, Unit> Cancel { get; }

        public Interaction<Reminder?, Unit> Close { get; } = new(RxApp.MainThreadScheduler);

        public ReminderViewModel()
        {
            var canExecute = new Subject<bool>();
            var isEnteredEventName = this.WhenAnyValue(o => o.ReminderName, (string? o) => !string.IsNullOrWhiteSpace(o));
            var isEnteredDateNtime = this.WhenAnyValue(o => o.DateNTime, (string? o) => !string.IsNullOrWhiteSpace(o) && DateTime.TryParse(o, out _));
            var isEnteredEventFrequency = this.WhenAnyValue(o => o.ReminderFrequency, (string? o) => !string.IsNullOrWhiteSpace(o) && int.TryParse(o, out var data) && data > 0 && data < 8);

            var canExecuteAndIsEntered = canExecute.CombineLatest(isEnteredEventName, isEnteredDateNtime, isEnteredEventFrequency,
                (canExecute, isEnteredEventName, isEnteredDateNtime, isEnteredEventFrequency) => canExecute && isEnteredEventName && isEnteredDateNtime && isEnteredEventFrequency);

            Ok = ReactiveCommand.CreateFromTask(() => ExclusiveWrapper(OkImpl), canExecuteAndIsEntered);
            Cancel = ReactiveCommand.CreateFromObservable(CancelImpl);

            async Task ExclusiveWrapper(Func<IObservable<Unit>> impl)
            {
                try
                {
                    canExecute.OnNext(false);
                    await impl();
                }
                finally
                {
                    canExecute.OnNext(false);
                }
            }
            canExecute.OnNext(true);
        }


        private IObservable<Unit> OkImpl()
        {
            var reminder = new Reminder
            {
                Name = ReminderName,
                DateTime = DateTime.Parse(DateNTime!),
                RepeatPeriod = int.Parse(ReminderFrequency!)
            };
            return Close.Handle(reminder);
        }

        private IObservable<Unit> CancelImpl()
        {
            return Close.Handle(null);
        }
    }
}