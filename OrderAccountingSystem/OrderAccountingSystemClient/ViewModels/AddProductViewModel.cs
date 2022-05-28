using Grpc.Net.Client;
using ReactiveUI;
using System;
using System.Reactive;
using System.Text.RegularExpressions;

namespace OrderAccountingSystemClient.ViewModels
{
    public sealed class AddProductViewModel
    {

        public string NameInput { get; set; } = string.Empty;

        public string PriceInput { get; set; } = string.Empty;

        public string ErrorTextBlock { get; set; } = string.Empty;

        public ReactiveCommand<Unit, Unit> Add { get; }

        public ReactiveCommand<Unit, Unit> Cancel { get; }

        public Interaction<Unit?, Unit> Close { get; } = new(RxApp.MainThreadScheduler);

        private static readonly OrderAccountingSystem.AccountingSystemGreeter.AccountingSystemGreeterClient client = new(GrpcChannel.ForAddress(App.Default.Host));

        public AddProductViewModel()
        {
            
            Add = ReactiveCommand.CreateFromObservable(AddImpl);
            Cancel = ReactiveCommand.CreateFromObservable(CancelImpl);
        }

        private IObservable<Unit> AddImpl()
        {
            if (!(new Regex("[^0-9,]+").IsMatch(PriceInput))&&PriceInput!="")
            {
                var reply = client.AddProduct(new OrderAccountingSystem.ProductRequest
                {
                    Name = NameInput,
                    Price = double.Parse(PriceInput)
                });
                return Close.Handle(null);
            }
            else
            {
                ErrorTextBlock = "Incorrect!";
            }
            return Close.Handle(null);
        }

        private IObservable<Unit> CancelImpl()
        {
            return Close.Handle(null);
        }
    }
}
