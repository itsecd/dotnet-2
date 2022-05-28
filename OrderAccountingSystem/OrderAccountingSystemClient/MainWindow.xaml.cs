using Grpc.Net.Client;
using OrderAccountingSystemClient.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace OrderAccountingSystemClient
{
    public partial class MainWindow : Window
    {
        private static readonly OrderAccountingSystem.AccountingSystemGreeter.AccountingSystemGreeterClient client = new(GrpcChannel.ForAddress(App.Default.Host));

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Start_Page_Loaded(object sender, RoutedEventArgs e)
        {
            Update_Customer_Table();
            Update_Order_Table();
            Update_Product_Table();
        }

        private void Add_Customer_Click(object sender, RoutedEventArgs e)
        {
            AddCustomerWindow customerWindow = new AddCustomerWindow();
            customerWindow.Show();
            Update_Customer_Table();
        }

        private void Delete_Customer_Click(object sender, RoutedEventArgs e)
        {
            if (CustomersTable.SelectedItem != null)
            {
                client.DeleteCustomer(new OrderAccountingSystem.CustomerRequest
                {
                    CustomerId = ((Customer)CustomersTable.SelectedItem).CustomerId.ToString()
                });
                Update_Customer_Table();
            }
        }

        private void Add_Product_Click(object sender, RoutedEventArgs e)
        {
            AddProductWindow productWindow = new AddProductWindow();
            productWindow.Show();
            Update_Order_Table();
        }

        private void Delete_Product_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsTable.SelectedItem != null)
            {
                client.DeleteProduct(new OrderAccountingSystem.ProductRequest
                {
                    ProductId = ((Product)ProductsTable.SelectedItem).ProductId.ToString()
                });
                Update_Product_Table();
            }
        }

        private void Add_Order_Click(object sender, RoutedEventArgs e)
        {
            AddOrderWindow orderWindow = new AddOrderWindow();
            orderWindow.Show();
            Update_Product_Table();
        }

        private void Change_Status_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersTable.SelectedItem != null)
            {
                DataGridCellInfo cellInfo = OrdersTable.SelectedCells[0];

                DataGridBoundColumn column = cellInfo.Column as DataGridBoundColumn;

                FrameworkElement element = new FrameworkElement() { DataContext = cellInfo.Item };
                BindingOperations.SetBinding(element, TagProperty, column.Binding);
                client.ChangeOrderStatus(new OrderAccountingSystem.OrderRequest
                {
                    OrderId = element.Tag.ToString(),
                    Status = int.Parse((string)((ComboBoxItem)StatusComboBox.SelectedItem).DataContext)
                });
                Update_Order_Table();
            }
        }

        private void Delete_Order_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersTable.SelectedItem != null)
            {
                DataGridCellInfo cellInfo = OrdersTable.SelectedCells[0];

                DataGridBoundColumn column = cellInfo.Column as DataGridBoundColumn;

                FrameworkElement element = new FrameworkElement() { DataContext = cellInfo.Item };
                BindingOperations.SetBinding(element, TagProperty, column.Binding);

                client.DeleteOrder(new OrderAccountingSystem.OrderRequest
                {
                    OrderId = element.Tag.ToString()

                });
                Update_Order_Table();
            }
        }

        private void Update_Table_Click(object sender, RoutedEventArgs e)
        {
            Update_Customer_Table();
            Update_Order_Table();
            Update_Product_Table();
        }

        private void Get_Monthly_Sale_Click(object sender, RoutedEventArgs e)
        {
            var reply = client.GetMonthlySale(new OrderAccountingSystem.NullRequest { });
            MessageBox.Show("Monthly Sale is " + reply.Price.ToString());
        }

        private void Update_Customer_Table()
        {
            var reply = client.GetAllCustomers(new OrderAccountingSystem.NullRequest { });
            List<Customer> customers = new List<Customer>();
            foreach (var customer in reply.Customers)
            {
                customers.Add(new Customer(Guid.Parse(customer.CustomerId), customer.Name, customer.Phone));
            }
            CustomersTable.ItemsSource = customers;
        }

        private void Update_Order_Table()
        {
            var reply = client.GetAllOrders(new OrderAccountingSystem.NullRequest { });
            List<Order> orders = new List<Order>();
            foreach (var order in reply.Orders)
            {
                List<Product> products = new List<Product>();
                foreach (var product in order.Products)
                {
                    products.Add(new Product(Guid.Parse(product.ProductId), product.Name, product.Price));
                }
                orders.Add(new Order(
                    Guid.Parse(order.OrderId),
                    new Customer(Guid.Parse(order.Customer.CustomerId), order.Customer.Name, order.Customer.Phone),
                    products,
                    order.Status,
                    order.Date.ToString()
                    ));
            }
            var itemSource = orders.Select(x => new
            {
                Id = x.OrderId.ToString(),
                CustomerName = x.Customer.Name,
                ProductName = String.Join("\n", x.Products.Select(f => f.Name).ToArray()),
                Status = x.Status.ToString(),
                Price = x.Price.ToString(),
                Date = x.Date.ToString()
            }).ToList();
            OrdersTable.ItemsSource = itemSource;
        }

        private void Update_Product_Table()
        {
            var reply = client.GetAllProducts(new OrderAccountingSystem.NullRequest { });
            List<Product> products = new List<Product>();
            foreach (var product in reply.Products)
            {
                products.Add(new Product(Guid.Parse(product.ProductId), product.Name, product.Price));
            }
            ProductsTable.ItemsSource = products;
        }
    }
}
