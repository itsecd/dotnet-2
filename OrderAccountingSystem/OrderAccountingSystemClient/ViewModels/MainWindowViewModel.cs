using Grpc.Net.Client;
using OrderAccountingSystemClient.Models;
using ReactiveUI;
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
        public ComboBoxItem SelectStatus { get; set; } = new() { DataContext = "0" };
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
        private static readonly OrderAccountingSystem.AccountingSystemGreeter.AccountingSystemGreeterClient Client = new(GrpcChannel.ForAddress(Properties.Settings.Default.Host));

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
            Client.DeleteCustomer(new OrderAccountingSystem.CustomerRequest
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
            Client.DeleteOrder(new OrderAccountingSystem.OrderRequest
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
            Client.DeleteProduct(new OrderAccountingSystem.ProductRequest
            {
                ProductId = SelectedProduct.ProductId
            });
            UpdateProductTable();
        }
        private void GetMonthlySaleImpl()
        {
            var reply = Client.GetMonthlySale(new OrderAccountingSystem.NullRequest());
            MessageBox.Show("Monthly Sale is " + reply.Price);
        }

        private void ChangeStatusImpl()
        {
            Client.ChangeOrderStatus(new OrderAccountingSystem.OrderRequest
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
            var reply = Client.GetAllCustomers(new OrderAccountingSystem.NullRequest());
            SourceCustomer.Clear();
            foreach (var customer in reply.Customers)
            {
                SourceCustomer.Add(new Customer(customer.CustomerId, customer.Name, customer.Phone));
            }
        }

        private void UpdateOrderTable()
        {
            var reply = Client.GetAllOrders(new OrderAccountingSystem.NullRequest());
            SourceOrder.Clear();
            foreach (var order in reply.Orders)
            {
                var products = string.Empty;
                foreach (var product in order.Products)
                {
                    products += product.Name + "\n";
                }
                SourceOrder.Add(new Order(order.OrderId, order.Customer.Name, products, order.Date, order.Price, order.Status));
            }
        }

        private void UpdateProductTable()
        {
            var reply = Client.GetAllProducts(new OrderAccountingSystem.NullRequest());
            SourceProduct.Clear();
            foreach (var product in reply.Products)
            {
                SourceProduct.Add(new Product(product.ProductId, product.Name, product.Price));
            }
        }
    }
}
