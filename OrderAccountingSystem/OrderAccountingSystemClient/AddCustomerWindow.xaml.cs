using Grpc.Net.Client;
using System.Windows;
using WpfApp1;

namespace OrderAccountingSystemClient
{
    public partial class AddCustomerWindow : Window
    {
        private static readonly OrderAccountingSystem.AccountingSystemGreeter.AccountingSystemGreeterClient client = new(GrpcChannel.ForAddress(App.Default.Host));

        public AddCustomerWindow()
        {
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
