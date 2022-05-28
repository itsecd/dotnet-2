using System.Windows;
using Laba2Client.ViewModels;

namespace Laba2Client.Views
{
    public partial class ProductView : Window
    {
        public ProductView()
        {
            InitializeComponent();
        }
        public ProductView(ProductViewModel productViewModel)
        {
            InitializeComponent();
            DataContext = productViewModel;
        }
    }
}