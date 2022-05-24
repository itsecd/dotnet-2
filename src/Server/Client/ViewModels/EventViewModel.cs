using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Server.Model;

namespace Client.ViewModels
{
    public sealed class EventViewModel : ReactiveObject
    {
        [Reactive]
        public string? EventName { get; set; } = string.Empty;

        [Reactive]
        public string? DateNTime { get; set; } = string.Empty;

        [Reactive]
        public string? EventFrequency { get; set; } = string.Empty;

        public ReactiveCommand<Unit, Unit> Ok { get; }

        public ReactiveCommand<Unit, Unit> Cancel { get; }

        public Interaction<UserEvent?, Unit> Close { get; } = new(RxApp.MainThreadScheduler);

        public EventViewModel()
        {
            var canExecute = new Subject<bool>();
            var isEnteredEventName = this.WhenAnyValue(o => o.EventName, (string? o) => (!string.IsNullOrWhiteSpace(o) && !string.IsNullOrEmpty(o)));
            var isEnteredDateNtime = this.WhenAnyValue(o => o.DateNTime, (string? o) => (!string.IsNullOrWhiteSpace(o) && !string.IsNullOrEmpty(o) && DateTime.TryParse(o, out _)));
            var isEnteredEventFrequency = this.WhenAnyValue(o => o.EventFrequency, (string? o) => (!string.IsNullOrWhiteSpace(o) && int.TryParse(o, out var data) && data > 0 && data < 8));

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
            var userEvent = new UserEvent
            {
                EventName = EventName,
                DateNTime = DateTime.Parse(DateNTime),
                EventFrequency = int.Parse(EventFrequency)
            };
            return Close.Handle(userEvent);
        }

        private IObservable<Unit> CancelImpl()
        {
            return Close.Handle(null);
        }
    }
}
