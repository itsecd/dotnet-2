using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Laba2Client.Commands;

namespace Laba2Client.ViewModels
{
    public class CustomersViewModel : INotifyPropertyChanged
    {
        private OrderSystemRepository _orderSystemRepository;
        public ObservableCollection<CustomerViewModel> Customers { get; } = new();

        private Customer _selectedCustomer;

        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                if (value == _selectedCustomer) return;
                _selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }
        public Command OpenMainWindowCommand { get; }

        public CustomersViewModel()
        {
            OpenMainWindowCommand = new Command(async commandParameter =>
            {
                var window = (Window)commandParameter;
                window.Hide();
                var mainWindow = window.Owner;
                Application.Current.MainWindow = mainWindow;
                mainWindow.Show();
            }, null);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public async Task InitializeAsync(OrderSystemRepository orderSystemRepository)
        {
            _orderSystemRepository = orderSystemRepository;

            var customers = await _orderSystemRepository.GetAllCustomers();
            foreach (var customer in customers)
            {
                var customerViewModel = new CustomerViewModel();
                await customerViewModel.InitializeAsync(_orderSystemRepository, customer.Id);
                Customers.Add(customerViewModel);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
