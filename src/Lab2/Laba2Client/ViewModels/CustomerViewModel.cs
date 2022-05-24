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
    public class CustomerViewModel : INotifyPropertyChanged
    {
        private OrderSystemRepository _orderSystemRepository;
        private Customer _customer;

        public CustomerViewModel()
        {
            _customer = new Customer();
        }
        public int Id { get => _customer.Id; }
        public string FullName
        { 
            get => _customer.FullName;
            set
            {
                if (value == _customer.FullName) return;
                _customer.FullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }
        public string PhoneNumber
        {
            get => _customer.PhoneNumber;
            set
            {
                if (value == _customer.PhoneNumber) return;
                _customer.PhoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task InitializeAsync(OrderSystemRepository orderRepository, int customerId)
        {
            _orderSystemRepository = orderRepository;

            var customers = await _orderSystemRepository.GetAllCustomers();
            var customer = customers.FirstOrDefault(customer => customer.Id == customerId);
            if (customer == null)
            {
                return;
            }
            _customer = customer;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
