using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Laba2Client.Properties;

namespace Laba2Client.ViewModels
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        private OrderSystemRepository _orderSystemRepository;
        private Order _order;

        public OrderViewModel()
        {
            _order = new Order()
            {
                Dt = DateTime.Now,
                Products = new List<Product>()
            };
        }
        public int Id { get => _order.OrderId; }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task InitializeAsync(OrderSystemRepository orderRepository, int orderId)
        {
            _orderSystemRepository = orderRepository;

            var orders = await _orderSystemRepository.GetAllOrders();
            var order = orders.FirstOrDefault(order => order.OrderId == orderId);
            if (order == null)
            {
                return;
            }
            _order = order;
            var customer = await _orderSystemRepository.GetCustomer(_order.CustomerId);
            CustomerName = customer.FullName;
            CustomerPhone = customer.PhoneNumber;
        }


        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
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
