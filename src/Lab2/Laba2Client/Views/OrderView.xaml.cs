using System.Windows;
using Laba2Client.ViewModels;

namespace Laba2Client.Views
{
    public partial class OrderView : Window
    {
        public OrderView()
        {
            InitializeComponent();
        }
        public OrderView(OrderViewModel orderViewModel)
        {
            InitializeComponent();
            DataContext = orderViewModel;
        }
    }
}