using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba2Client.ViewModels
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        private OrderSystemRepository _orderRepository;
        private Order _order;

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task InitializeAsync(OrderSystemRepository orderRepository, int orderId)
        {
            _orderRepository = orderRepository;

            var orders = await _orderRepository.GetAllOrders();
            _order = orders.FirstOrDefault(order => order.OrderId == orderId);
        }
        public double AmountOrder
        {
            get => _order.AmountOrder;
            set
            {
                if (value == _order.AmountOrder) return;
                _order.AmountOrder = value;
                OnPropertyChanged(nameof(AmountOrder));
            }
        }
        public DateTime Dt
        {
            get => _order.Dt.DateTime;
            set
            {
                if (value == _order.Dt) return;
                _order.Dt = value;
                OnPropertyChanged(nameof(Dt));
            }
        }
        public string Status
        {
            get => _order.Status;
            set
            {
                if (value == _order.Status) return;
                _order.Status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
        public List<Product> Products
        {
            get => (List<Product>)_order.Products;
            set
            {
                if (value == _order.Products) return;
                _order.Products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
