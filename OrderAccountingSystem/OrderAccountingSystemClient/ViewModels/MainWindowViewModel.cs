using Grpc.Net.Client;
using OrderAccountingSystemClient.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OrderAccountingSystemClient.ViewModels
{
    public sealed class MainWindowViewModel
    {
        public Customer SelectedCustomer { get; set; } = new();
        public Order SelectedOrder { get; set; } = new();
        public Product SelectedProduct { get; set; } = new();
        public ComboBoxItem SelectStatus { get; set; } = new() { DataContext = "0"};
        public ObservableCollection<Customer> SourceCustomer { get; set; } = new();
        public ObservableCollection<Order> SourceOrder { get; set; } = new();
        public ObservableCollection<Product> SourceProduct { get; set; } = new();
        public ReactiveCommand<Unit, Unit> AddCustomer { get; }
        public ReactiveCommand<Unit, Unit> AddProduct { get; }
        public ReactiveCommand<Unit, Unit> AddOrder { get; }
        public ReactiveCommand<Unit, Unit> DeleteCustomer { get; }
        public ReactiveCommand<Unit, Unit> DeleteProduct { get; }
        public ReactiveCommand<Unit, Unit> DeleteOrder { get; }
        public ReactiveCommand<Unit, Unit> UpdateTable { get; }
        public ReactiveCommand<Unit, Unit> ChangeStatus { get; }
        public ReactiveCommand<Unit, Unit> GetMonthlySale { get; }
        public Interaction<Unit, Unit> CreateCustomer { get; } = new();
        public Interaction<Unit, Unit> CreateProduct { get; } = new();
        public Interaction<Unit, Unit> CreateOrder { get; } = new();
        private static readonly OrderAccountingSystem.AccountingSystemGreeter.AccountingSystemGreeterClient _client = new(GrpcChannel.ForAddress(Properties.Settings.Default.Host));

        public MainWindowViewModel()
        {
            AddCustomer = ReactiveCommand.CreateFromTask(AddCustomerImpl);
            DeleteCustomer = ReactiveCommand.Create(DeleteCustomerImpl);
            AddOrder = ReactiveCommand.CreateFromTask(AddOrderImpl);
            DeleteOrder = ReactiveCommand.Create(DeleteOrderImpl);
            AddProduct = ReactiveCommand.CreateFromTask(AddProductImpl);
            DeleteProduct = ReactiveCommand.Create(DeleteProductImpl);
            UpdateTable = ReactiveCommand.Create(UpdateTableImpl);
            ChangeStatus = ReactiveCommand.Create(ChangeStatusImpl);
            GetMonthlySale = ReactiveCommand.Create(GetMonthlySaleImpl);

            UpdateTableImpl();
        }

        private async Task AddCustomerImpl()
        {
            await CreateCustomer.Handle(Unit.Default);
        }

        private void DeleteCustomerImpl()
        {
            _client.DeleteCustomer(new OrderAccountingSystem.CustomerRequest
            {
                CustomerId = SelectedCustomer.CustomerId
            });
        }

        private async Task AddOrderImpl()
        {
            await CreateOrder.Handle(Unit.Default);
        }

        private void DeleteOrderImpl()
        {
            _client.DeleteOrder(new OrderAccountingSystem.OrderRequest
            {
                OrderId = SelectedOrder.OrderId
            });
            UpdateOrderTable();
        }

        private async Task AddProductImpl()
        {
            await CreateProduct.Handle(Unit.Default);
        }

        private void DeleteProductImpl()
        {
            _client.DeleteProduct(new OrderAccountingSystem.ProductRequest
            {
                ProductId = SelectedProduct.ProductId
            });
            UpdateProductTable();
        }
        private void GetMonthlySaleImpl()
        {
            var reply = _client.GetMonthlySale(new OrderAccountingSystem.NullRequest());
            MessageBox.Show("Monthly Sale is " + reply.Price);
        }

        private void ChangeStatusImpl()
        {
            _client.ChangeOrderStatus(new OrderAccountingSystem.OrderRequest
            {
                OrderId = SelectedOrder.OrderId,
                Status = int.Parse((string)SelectStatus.DataContext)
            });
            UpdateOrderTable();
        }

        private void UpdateTableImpl()
        {
            UpdateCustomerTable();
            UpdateOrderTable();
            UpdateProductTable();
        }

        private void UpdateCustomerTable()
        {
            var reply = _client.GetAllCustomers(new OrderAccountingSystem.NullRequest { });
            SourceCustomer.Clear();
            foreach (var customer in reply.Customers)
            {
                SourceCustomer.Add(new Customer() { CustomerId = customer.CustomerId, Name = customer.Name, Phone = customer.Phone });
            }
        }

        private void UpdateOrderTable()
        {
            var reply = _client.GetAllOrders(new OrderAccountingSystem.NullRequest { });
            SourceOrder.Clear();
            foreach (var order in reply.Orders)
            {
                var products = string.Empty;
                foreach (var product in order.Products)
                {
                    products += product.Name + "\n";
                }
                SourceOrder.Add(new Order()
                {
                    OrderId = order.OrderId,
                    Customer = order.Customer.Name,
                    Products = products,
                    Date = order.Date,
                    Price = order.Price,
                    Status = order.Status
                });
            }
        }

        private void UpdateProductTable()
        {
            var reply = _client.GetAllProducts(new OrderAccountingSystem.NullRequest { });
            SourceProduct.Clear();
            foreach (var product in reply.Products)
            {
                SourceProduct.Add(new Product() { ProductId = product.ProductId, Name = product.Name, Price = product.Price });
            }
        }
    }
}
