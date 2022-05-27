using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OrderAccountingSystemClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void Add_Customer_Click(object sender, RoutedEventArgs e)
        {
            AddCustomerWindow customerWindow = new AddCustomerWindow();
            customerWindow.Show();
        }

        private void Add_Product_Click(object sender, RoutedEventArgs e)
        {
            AddProductWindow productWindow = new AddProductWindow();    
            productWindow.Show();
        }
    }
}
