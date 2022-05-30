using System.Windows;
using Laba2Client.ViewModels;

namespace Laba2Client.Views
{
    public partial class CustomersView : Window
    {
        public CustomersView()
        {
            InitializeComponent();
        }
        public CustomersView(CustomersViewModel customersViewModel)
        {
            InitializeComponent();
            DataContext = customersViewModel;
        }
    }
}