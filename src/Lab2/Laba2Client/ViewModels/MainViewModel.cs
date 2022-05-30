using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Laba2Client.Commands;
using Laba2Client.Views;

namespace Laba2Client.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private OrderSystemRepository _orderSystemRepository;
        public ObservableCollection<OrderViewModel> Orders { get; } = new();

        private OrderViewModel _selectedOrder;
        public OrderViewModel SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                if (value == _selectedOrder)
                {
                    return;
                }

                _selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
            }
        }
        public Command AddOrderCommand { get; }
        public Command UpdateOrderCommand { get; }
        public Command RemoveOrderCommand { get; }
        public Command RemoveAllOrdersCommand { get; }
        public Command OpenCustomerViewCommand { get; }
        public Command OpenMonthlyReportCommand { get; }
        public MainViewModel()
        {
            AddOrderCommand = new Command(async _ =>
            {
                if (_orderSystemRepository == null)
                {
                    return;
                }

                OrderViewModel orderViewModel = new();
                await orderViewModel.InitializeAsync(_orderSystemRepository, 0);
                orderViewModel.Mode = "Add";
                var orderView = new OrderView(orderViewModel);
                if (orderView.ShowDialog() == true)
                {
                    Orders.Clear();
                    await InitializeAsync();
                }
            }, null);
            UpdateOrderCommand = new Command(_ =>
            {
                if (SelectedOrder is not null)
                {
                    var orderViewModel = Orders.Single(ordView => ordView.Id == SelectedOrder.Id);
                    orderViewModel.Mode = "Update";
                    var orderView = new OrderView(orderViewModel);
                    orderView.ShowDialog();
                }
            }, null);
            OpenCustomerViewCommand = new Command(async commandParameter =>
            {
                if (_orderSystemRepository == null)
                {
                    return;
                }

                var window = (Window)commandParameter;
                var customersViewModel = new CustomersViewModel();
                await customersViewModel.InitializeAsync(_orderSystemRepository);
                customersViewModel.ModeCustomer = "Another";
                var customersView = new CustomersView(customersViewModel);
                window.Hide();
                customersView.Owner = window;
                Application.Current.MainWindow = customersView;
                customersView.Show();
            }, null);
            RemoveOrderCommand = new Command(async _ =>
            {
                if (_orderSystemRepository == null)
                {
                    return;
                }

                if (SelectedOrder is not null)
                {
                    await _orderSystemRepository.DeleteOrder(SelectedOrder.Id);
                    Orders.Remove(SelectedOrder);
                }
            }, null);
            RemoveAllOrdersCommand = new Command(async _ =>
            {
                if (_orderSystemRepository == null)
                {
                    return;
                }

                await _orderSystemRepository.DeleteAllOrder();
                Orders.Clear();
            }, null);
            OpenMonthlyReportCommand = new Command(async _ =>
            {
                if (_orderSystemRepository == null)
                {
                    return;
                }

                var products = await _orderSystemRepository.MonthlyReport();
                var totalCost = await _orderSystemRepository.MonthlyTotalCost();
                var monthlyReportViewModel = new MonthlyReportViewModel();
                monthlyReportViewModel.Initialize(products, totalCost);
                var monthlyView = new MonthlyReportView(monthlyReportViewModel);
                monthlyView.ShowDialog();
            }, null);
        }
        public async Task InitializeAsync()
        {
            _orderSystemRepository = new OrderSystemRepository();
            var orders = await _orderSystemRepository.GetAllOrders();
            foreach (var order in orders)
            {
                var orderViewModel = new OrderViewModel();
                await orderViewModel.InitializeAsync(_orderSystemRepository, order.OrderId);
                orderViewModel.Mode = "Update";
                Orders.Add(orderViewModel);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}