using Grpc.Net.Client;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows.Controls;

namespace OrderAccountingSystemClient.ViewModels
{
    public sealed class AddOrderViewModel
    {
        public ObservableCollection<ComboBoxItem> SourceProduct { get; set; } = new();

        public ObservableCollection<ComboBoxItem> SourceCustomer { get; set; } = new();

        public ComboBoxItem SelectOrder { get; set; } = new();

        public ComboBoxItem SelectCustomer { get; set; } = new();

        public ComboBoxItem SelectStatus { get; set; } = new() { DataContext = "0" };

        public DateTime SelectedDate { get; set; } = DateTime.Now;

        public ReactiveCommand<Unit, Unit> Add { get; }

        public ReactiveCommand<Unit, Unit> Cancel { get; }

        public Interaction<Unit?, Unit> Close { get; } = new(RxApp.MainThreadScheduler);

        private static readonly OrderAccountingSystem.AccountingSystemGreeter.AccountingSystemGreeterClient client = new(GrpcChannel.ForAddress(App.Default.Host));

        public AddOrderViewModel()
        {
            Add = ReactiveCommand.CreateFromObservable(AddImpl);
            Cancel = ReactiveCommand.CreateFromObservable(CancelImpl);
            Update_Products_Items();
            Update_Customers_Items();
        }

        private IObservable<Unit> AddImpl()
        {
            var orderRequest = new OrderAccountingSystem.OrderRequest();
            orderRequest.Customer = new OrderAccountingSystem.CustomerRequest
            {
                CustomerId = SelectCustomer.DataContext.ToString()
            };
            foreach (ComboBoxItem item in SourceProduct)
            {
                CheckBox? checkBox = item.Content as CheckBox;
                if ((bool)checkBox.IsChecked)
                {
                    orderRequest.Products.Add(new OrderAccountingSystem.ProductRequest
                    {
                        ProductId = checkBox.DataContext.ToString()
                    });
                }
            }
            orderRequest.Status = int.Parse((string)SelectStatus.DataContext);
            orderRequest.Date = SelectedDate.ToString();
            var reply = client.AddOrder(orderRequest);

            return Close.Handle(null);
        }

        private IObservable<Unit> CancelImpl()
        {
            return Close.Handle(null);
        }

        private void Update_Products_Items()
        {
            var reply = client.GetAllProducts(new OrderAccountingSystem.NullRequest { });
            foreach (var item in reply.Products)
            {
                SourceProduct.Add(new ComboBoxItem()
                {
                    Content = new CheckBox()
                    {
                        DataContext = item.ProductId,
                        Content = item.Name
                    }
                });
            }
        }

        private void Update_Customers_Items()
        {
            var reply = client.GetAllCustomers(new OrderAccountingSystem.NullRequest { });
            foreach (var item in reply.Customers)
            {
                SourceCustomer.Add(new ComboBoxItem() { DataContext = item.CustomerId, Content = item.Name });
            }
        }
    }
}
