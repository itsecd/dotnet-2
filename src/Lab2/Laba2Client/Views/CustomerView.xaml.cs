using System.Windows;
using Laba2Client.ViewModels;

namespace Laba2Client.Views
{
    public partial class CustomerView : Window
    {
        public CustomerView()
        {
            InitializeComponent();
        }
        public CustomerView(CustomerViewModel customerViewModel)
        {
            InitializeComponent();
            DataContext = customerViewModel;
        }
    }
}
