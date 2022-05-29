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

        private static readonly OrderAccountingSystem.AccountingSystemGreeter.AccountingSystemGreeterClient client = new(GrpcChannel.ForAddress(App.Default.Host));

        public MainWindowViewModel()
        {
            AddCustomer = ReactiveCommand.CreateFromTask(AddCustomerImpl);
            DeleteCustomer = ReactiveCommand.Create(DeleteCustomerImpl);
            AddOrder = ReactiveCommand.CreateFromTask(AddOrderImpl);
            DeleteOrder = ReactiveCommand.Create(DeleteOrderImpl);
            AddProduct = ReactiveCommand.CreateFromTask(AddProductImpl);
            DeleteProduct = ReactiveCommand.Create(DeleteProductImpl);
            UpdateTable = ReactiveCommand.Create(UpdateTableImpl);
            ChangeStatus = ReactiveCommand.Create(ChangeStatuImpl);
            GetMonthlySale = ReactiveCommand.Create(GetMonthlySaleImpl);

            UpdateTableImpl();
        }

        private async Task AddCustomerImpl()
        {
            await CreateCustomer.Handle(Unit.Default);
        }

        private void DeleteCustomerImpl()
        {
            if (SelectedCustomer != null)
            {
                client.DeleteCustomer(new OrderAccountingSystem.CustomerRequest
                {
                    CustomerId = SelectedCustomer.CustomerId
                });
            }
            Update_Customer_Table();
        }

        private async Task AddOrderImpl()
        {
            await CreateOrder.Handle(Unit.Default);
        }

        private void DeleteOrderImpl()
        {
            if (SelectedOrder != null)
            {
                client.DeleteOrder(new OrderAccountingSystem.OrderRequest
                {
                    OrderId = SelectedOrder.OrderId
                });
                Update_Order_Table();
            }
        }

        private async Task AddProductImpl()
        {
            await CreateProduct.Handle(Unit.Default);
        }

        private void DeleteProductImpl()
        {
            if (SelectedProduct != null)
            {
                client.DeleteProduct(new OrderAccountingSystem.ProductRequest
                {
                    ProductId = SelectedProduct.ProductId
                });
            }
            Update_Product_Table();
        }
        private void GetMonthlySaleImpl()
        {
            var reply = client.GetMonthlySale(new OrderAccountingSystem.NullRequest { });
            MessageBox.Show("Monthly Sale is " + reply.Price.ToString());
        }

        private void ChangeStatuImpl()
        {
            if (SelectedOrder != null)
            {
                client.ChangeOrderStatus(new OrderAccountingSystem.OrderRequest
                {
                    OrderId = SelectedOrder.OrderId,
                    Status = int.Parse((string)SelectStatus.DataContext)
                });
                Update_Order_Table();
            }
        }

        private void UpdateTableImpl()
        {
            Update_Customer_Table();
            Update_Order_Table();
            Update_Product_Table();
        }

        private void Update_Customer_Table()
        {
            var reply = client.GetAllCustomers(new OrderAccountingSystem.NullRequest { });
            SourceCustomer.Clear();
            foreach (var customer in reply.Customers)
            {
                SourceCustomer.Add(new Customer() { CustomerId = customer.CustomerId, Name = customer.Name, Phone = customer.Phone });
            }
        }

        private void Update_Order_Table()
        {
            var reply = client.GetAllOrders(new OrderAccountingSystem.NullRequest { });
            SourceOrder.Clear();
            foreach (var order in reply.Orders)
            {
                String products = string.Empty;
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

        private void Update_Product_Table()
        {
            var reply = client.GetAllProducts(new OrderAccountingSystem.NullRequest { });
            SourceProduct.Clear();
            foreach (var product in reply.Products)
            {
                SourceProduct.Add(new Product() { ProductId = product.ProductId, Name = product.Name, Price = product.Price });
            }
        }
    }
}
