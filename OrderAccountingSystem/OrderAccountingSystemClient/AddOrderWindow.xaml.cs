using Grpc.Net.Client;
using System;
using System.Windows;
using System.Windows.Controls;

namespace OrderAccountingSystemClient
{
    public partial class AddOrderWindow : Window
    {
        private static readonly OrderAccountingSystem.AccountingSystemGreeter.AccountingSystemGreeterClient client = new(GrpcChannel.ForAddress(App.Default.Host));

        public AddOrderWindow()
        {
            InitializeComponent();
        }
        private void Add_Order_Loaded(object sender, RoutedEventArgs e)
        {
            Update_Products_Items();
            Update_Customers_Items();
            DatePicker.SelectedDate = DateTime.Now;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var orderRequest = new OrderAccountingSystem.OrderRequest();
            orderRequest.Customer = new OrderAccountingSystem.CustomerRequest
            {
                CustomerId = ((ComboBoxItem)CustomerComboBox.SelectedItem).DataContext.ToString()
            };
            foreach (ComboBoxItem item in ProductComboBox.Items)
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
            orderRequest.Status = int.Parse((string)((ComboBoxItem)StatusComboBox.SelectedItem).DataContext);
            orderRequest.Date = DatePicker.SelectedDate.ToString();
            var reply = client.AddOrder(orderRequest);
            this.Close();
        }

        private void Update_Products_Items()
        {
            var reply = client.GetAllProducts(new OrderAccountingSystem.NullRequest { });
            foreach (var item in reply.Products)
            {
                ProductComboBox.Items.Add(new ComboBoxItem()
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
                CustomerComboBox.Items.Add(new ComboBoxItem() { DataContext = item.CustomerId, Content = item.Name });
            }
            CustomerComboBox.SelectedIndex = 0;
        }
    }
}
