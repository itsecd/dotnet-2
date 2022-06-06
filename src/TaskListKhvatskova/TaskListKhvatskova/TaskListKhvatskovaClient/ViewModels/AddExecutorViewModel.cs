using Grpc.Net.Client;
using ReactiveUI;
using System;
using System.Reactive;

namespace TaskListKhvatskovaClient.ViewModels
{
    public partial class AddExecutorViewModel
    { 
        public string NameInput { get; set; } = string.Empty;
        public string SurnameInput { get; set; } = string.Empty;
        public ReactiveCommand<Unit, Unit> Add { get; }
        public ReactiveCommand<Unit, Unit> Cancel { get; }
        public Interaction<Unit?, Unit> Close { get; } = new(RxApp.MainThreadScheduler);
        private static readonly TaskListKhvatskova.tasksList.tasksListClient Client = new(GrpcChannel.ForAddress(Properties.Settings.Default.Host));

        public AddExecutorViewModel()
        {

            Add = ReactiveCommand.CreateFromObservable(AddImpl);
            Cancel = ReactiveCommand.CreateFromObservable(CancelImpl);
        }

        private IObservable<Unit> AddImpl()
        {
            if (NameInput != "" && SurnameInput != "")
            {
                Client.AddExecutor(new TaskListKhvatskova.ExecutorRequest
                {
                    Name = NameInput,
                    Surname = SurnameInput
                });
            }
            return Close.Handle(null);
        }

        private IObservable<Unit> CancelImpl()
        {
            return Close.Handle(null);
        }
    }
}
