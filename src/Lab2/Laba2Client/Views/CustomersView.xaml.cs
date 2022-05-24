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
using System.Windows.Shapes;
using Laba2Client.ViewModels;

namespace Laba2Client.Views
{
    /// <summary>
    /// Логика взаимодействия для CustomerView.xaml
    /// </summary>
    public partial class CustomersView : Window
    {
        public CustomersView()
        {
            InitializeComponent();
        }
        public CustomersView(CustomersViewModel cutomersViewModel)
        {
            InitializeComponent();
            DataContext = cutomersViewModel;
        }
    }
}
