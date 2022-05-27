using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Laba2Client.Commands;
using System.Windows;
using Laba2Client.Views;

namespace Laba2Client.ViewModels
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        private OrderSystemRepository _orderSystemRepository;
        private Order _order;
        public int Id => _order.OrderId;
        private string _customerName;
        public string CustomerName
        {
            get => _customerName;
            set
            {
                if (value == _customerName) return;
                _customerName = value;
                OnPropertyChanged(nameof(CustomerName));
            }
        }
        private string _customerPhone;
        public string CustomerPhone
        {
            get => _customerPhone;
            set
            {
                if (value == _customerPhone) return;
                _customerPhone = value;
                OnPropertyChanged(nameof(CustomerPhone));
            }
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
        public Command AddOrder { get; }
        public Command OpenCustomerViewCommand { get; }
        public string Mode { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public OrderViewModel()
        {
            _order = new Order()
            {
                Dt = DateTime.Now,
                Products = new List<Product>()
            };
            AddOrder = new Command(async commandParameter =>
            {
                var newOrder = new Order()
                {
                    AmountOrder = _order.AmountOrder,
                    Status = _order.Status,
                    Products = _order.Products,
                    Dt = _order.Dt,
                };
                if (Mode == "Add")
                {
                    await _orderSystemRepository.AddOrder(newOrder);
                }
                else
                {
                    await _orderSystemRepository.ReplaceOrder(_order.OrderId, newOrder);
                }
                var window = (Window)commandParameter;
                window.DialogResult = true;
                window.Close();
            }, null);
            OpenCustomerViewCommand = new Command(async _ =>
            {
                var customersViewModel = new CustomersViewModel();
                await customersViewModel.InitializeAsync(_orderSystemRepository);
                customersViewModel.ModeCustomer = "Select";
                var customersView = new CustomersView(customersViewModel);
                if ((bool)customersView.ShowDialog())
                {
                    _order.CustomerId = customersViewModel.SelectedCustomer.Id;
                    var customer = await _orderSystemRepository.GetCustomer(_order.CustomerId);
                    CustomerName = customer.FullName;
                    CustomerPhone = customer.PhoneNumber;
                }
            }, (obj) => Mode == "Add");
        }
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
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
