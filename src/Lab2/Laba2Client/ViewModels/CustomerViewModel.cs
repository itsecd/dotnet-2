using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Laba2Client.Commands;

namespace Laba2Client.ViewModels
{
    public class CustomerViewModel : INotifyPropertyChanged
    {
        private OrderSystemRepository _orderSystemRepository;
        private Customer _customer;
        public int Id => _customer.Id;
        public string FullName
        {
            get => _customer.FullName;
            set
            {
                if (value == _customer.FullName)
                {
                    return;
                }

                _customer.FullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }
        public string PhoneNumber
        {
            get => _customer.PhoneNumber;
            set
            {
                if (value == _customer.PhoneNumber)
                {
                    return;
                }

                _customer.PhoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }
        public Command AddOrUpdateCustomerCommand { get; }
        public string Mode { get; set; }
        public CustomerViewModel()
        {
            _customer = new Customer();
            AddOrUpdateCustomerCommand = new Command(async commandParameter =>
            {
                var newCustomer = new Customer()
                {
                    FullName = _customer.FullName,
                    PhoneNumber = _customer.PhoneNumber
                };
                if (_orderSystemRepository == null)
                {
                    return;
                }

                if (Mode == "Adding")
                {
                    await _orderSystemRepository.AddCustomer(newCustomer);
                }
                else
                {
                    await _orderSystemRepository.ReplaceCustomer(_customer.Id, newCustomer);
                }
                var window = (Window)commandParameter;
                window.DialogResult = true;
                window.Close();
            }, null);
        }
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
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}