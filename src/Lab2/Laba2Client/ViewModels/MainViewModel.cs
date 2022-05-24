using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laba2Client.Commands;

namespace Laba2Client.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private OrderSystemRepository _orderRepository;
        public ObservableCollection<OrderViewModel> Orders { get; } = new();

        public Order _selectedOrder;

        public Order SelectedOrder
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
            _orderRepository = new OrderSystemRepository();

            var orders = await _orderRepository.GetAllOrders();
            foreach (var order in orders)
            {
                var orderViewModel = new OrderViewModel();
                await orderViewModel.InitializeAsync(_orderRepository, order.OrderId);
                Orders.Add(orderViewModel);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
