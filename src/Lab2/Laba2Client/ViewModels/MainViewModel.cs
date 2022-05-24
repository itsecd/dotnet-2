using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
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

        public Command AddOrderCommand { get; }
        public Command UpdateOrderCommand { get; }
        public Command OpenCustomerViewCommand { get; }

        public MainViewModel()
        {
            AddOrderCommand = new Command(async _ =>
            {
                var orders = await _orderSystemRepository.GetAllOrders();
                var id = orders.Max(order => order.OrderId) + 1;
                OrderViewModel orderViewModel = new();
                await orderViewModel.InitializeAsync(_orderSystemRepository, id);
                var orderView = new OrderView(orderViewModel);
                orderView.ShowDialog();
            }, null);

            UpdateOrderCommand = new Command(_ =>
            {
                var orderView = new OrderView(Orders.Single(ordView => ordView.Id == SelectedOrder.Id));
                orderView.ShowDialog();
            }, null);
            OpenCustomerViewCommand = new Command(async commandParameter =>
            {
                var window = (Window)commandParameter;
                var customersViewModel = new CustomersViewModel();
                await customersViewModel.InitializeAsync(_orderSystemRepository);
                var customersView = new CustomersView(customersViewModel);
                window.Hide();
                customersView.Owner = window;
                Application.Current.MainWindow = customersView;
                customersView.Show();
            }, null);

        }
        public OrderViewModel SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                if (value == _selectedOrder) return;
                _selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public async Task InitializeAsync()
        {
            _orderSystemRepository = new OrderSystemRepository();

            var orders = await _orderSystemRepository.GetAllOrders();
            foreach (var order in orders)
            {
                var orderViewModel = new OrderViewModel();
                await orderViewModel.InitializeAsync(_orderSystemRepository, order.OrderId);
                Orders.Add(orderViewModel);
            }
            
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
