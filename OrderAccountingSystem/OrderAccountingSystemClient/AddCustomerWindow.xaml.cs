using Grpc.Net.Client;
using System.Windows;

namespace OrderAccountingSystemClient
{
    public partial class AddCustomerWindow : Window
    {
        private static readonly OrderAccountingSystem.AccountingSystemGreeter.AccountingSystemGreeterClient client = new(GrpcChannel.ForAddress("https://localhost:5001"));

        public AddCustomerWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var reply = client.AddCustomer(new OrderAccountingSystem.CustomerRequest
            {
                Name = this.NameInput.Text,
                Phone = this.PhoneInput.Text
            });
            this.Close();
        }
    }
}
