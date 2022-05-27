using Grpc.Net.Client;
using System.Text.RegularExpressions;
using System.Windows;

namespace OrderAccountingSystemClient
{
    public partial class AddProductWindow : Window
    {
        private static readonly OrderAccountingSystem.AccountingSystemGreeter.AccountingSystemGreeterClient client = new(GrpcChannel.ForAddress("https://localhost:5001"));

        public AddProductWindow()
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
            if (CorrectPrice())
            {
                var reply = client.AddProduct(new OrderAccountingSystem.ProductRequest
                {
                    Name = this.NameInput.Text,
                    Price = double.Parse(this.PriceInput.Text)
                });
                this.Close();
            }
            else
            {
                this.ErrorTextBlock.Text = "Incorrect!";
            }
        }
        private bool CorrectPrice()
        {
            return !(new Regex("[^0-9,]+").IsMatch(this.PriceInput.Text));
        }
    }
}
