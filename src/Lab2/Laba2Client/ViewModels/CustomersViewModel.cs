using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Laba2Client.Commands;
using Laba2Client.Views;

namespace Laba2Client.ViewModels
{
    public class CustomersViewModel : INotifyPropertyChanged
    {
        private OrderSystemRepository _orderSystemRepository;
        public ObservableCollection<CustomerViewModel> Customers { get; } = new();
        private CustomerViewModel _selectedCustomer;
        public CustomerViewModel SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                if (value == _selectedCustomer)
                {
                    return;
                }
                _selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }
        public Command OpenMainWindowCommand { get; }
        public Command AddCustomerCommand { get; }
        public Command UpdateCustomerCommand { get; }
        public Command RemoveCustomerCommand { get; }
        public Command SelectCustomerCommand { get; }
        public Command RemoveAllCustomersCommand { get; }
        public Command CancelCustomerCommand { get; }
        public string ModeCustomer { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public CustomersViewModel()
        {
            OpenMainWindowCommand = new Command(commandParameter =>
            {
                var window = (Window)commandParameter;
                window.Hide();
                var mainWindow = window.Owner;
                Application.Current.MainWindow = mainWindow;
                mainWindow.Show();
            }, (obj) => ModeCustomer == "Another");
            RemoveCustomerCommand = new Command(async _ =>
            {
                if (SelectedCustomer is not null)
                {
                    await _orderSystemRepository.DeleteCustomer(SelectedCustomer.Id);
                    Customers.Remove(SelectedCustomer);
                }
            }, (obj) => ModeCustomer == "Another");
            RemoveAllCustomersCommand = new Command(async _ =>
            {
                await _orderSystemRepository.DeleteAllCustomer();
                Customers.Clear();
            }, (obj) => ModeCustomer == "Another");
            AddCustomerCommand = new Command(async _ =>
            {
                var customers = await _orderSystemRepository.GetAllCustomers();
                var id = customers.Max(customer => customer.Id) + 1;
                CustomerViewModel customerViewModel = new();
                await customerViewModel.InitializeAsync(_orderSystemRepository, id);
                customerViewModel.ModeCust = "Adding";
                var customerView = new CustomerView(customerViewModel);
                if (customerView.ShowDialog() == true)
                {
                    Customers.Clear();
                    await InitializeAsync(_orderSystemRepository);
                }
            }, null);
            UpdateCustomerCommand = new Command(_ =>
            {
                if (SelectedCustomer is not null)
                {
                    var customerViewModel = Customers.Single(custView => custView.Id == SelectedCustomer.Id);
                    customerViewModel.ModeCust = "Updating";
                    var customerView = new CustomerView(customerViewModel);
                    customerView.ShowDialog();
                }
            }, (obj) => ModeCustomer == "Another");
            SelectCustomerCommand = new Command(commandParameter =>
            {
                var window = (Window)commandParameter;
                window.DialogResult = true;
                window.Close();
            }, (obj) => ModeCustomer == "Select");
            CancelCustomerCommand = new Command(commandParameter =>
            {
                var window = (Window)commandParameter;
                window.DialogResult = false;
                window.Close();
            }, (obj) => ModeCustomer == "Select");
        }
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