using Grpc.Net.Client;
using ReactiveUI;
using System;
using System.Reactive;

namespace OrderAccountingSystemClient.ViewModels
{
    public sealed class AddCustomerViewModel
    {
        public string NameInput { get; set; } = string.Empty;
        public string PhoneInput { get; set; } = string.Empty;
        public ReactiveCommand<Unit, Unit> Add { get; }
        public ReactiveCommand<Unit, Unit> Cancel { get; }
        public Interaction<Unit?, Unit> Close { get; } = new(RxApp.MainThreadScheduler);
        private static readonly OrderAccountingSystem.AccountingSystemGreeter.AccountingSystemGreeterClient _client = new(GrpcChannel.ForAddress(Properties.Settings.Default.Host));

        public AddCustomerViewModel()
        {
            Add = ReactiveCommand.CreateFromObservable(AddImpl);
            Cancel = ReactiveCommand.CreateFromObservable(CancelImpl);
        }

        private IObservable<Unit> AddImpl()
        {
            _ = _client.AddCustomer(new OrderAccountingSystem.CustomerRequest
            {
                Name = NameInput,
                Phone = PhoneInput
            });

            return Close.Handle(null);
        }

        private IObservable<Unit> CancelImpl()
        {
            return Close.Handle(null);
        }
    }
}
