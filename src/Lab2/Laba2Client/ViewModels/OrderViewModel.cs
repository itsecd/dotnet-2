using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Laba2Client.Commands;
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
                if (value == _customerName)
                {
                    return;
                }

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
                if (value == _customerPhone)
                {
                    return;
                }

                _customerPhone = value;
                OnPropertyChanged(nameof(CustomerPhone));
            }
        }
        public double AmountOrder
        {
            get => _order.AmountOrder;
            set
            {
                if (value == _order.AmountOrder)
                {
                    return;
                }

                _order.AmountOrder = value;
                OnPropertyChanged(nameof(AmountOrder));
            }
        }
        public DateTime Dt
        {
            get => _order.Dt.DateTime;
            set
            {
                if (value == _order.Dt)
                {
                    return;
                }

                _order.Dt = value;
                OnPropertyChanged(nameof(Dt));
            }
        }
        public string Status
        {
            get => _order.Status;
            set
            {
                if (value == _order.Status)
                {
                    return;
                }

                _order.Status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
        public List<Product> Products
        {
            get => (List<Product>)_order.Products;
            set
            {
                if (value == _order.Products)
                {
                    return;
                }

                _order.Products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        public ObservableCollection<ProductViewModel> ProductsCollection { get; } = new();
        private ProductViewModel _selectedProduct;
        public ProductViewModel SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                if (value == _selectedProduct)
                {
                    return;
                }

                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }
        public Command AddOrder { get; }
        public Command OpenCustomerViewCommand { get; }
        public Command AddProductCommand { get; }
        public Command UpdateProductCommand { get; }
        public Command RemoveAllProductsCommand { get; }
        public Command RemoveProductCommand { get; }
        public string Mode { get; set; }
        public OrderViewModel()
        {
            _order = new Order()
            {
                Dt = DateTime.Now,
                Products = new List<Product>()
            };
            AddOrder = new Command(async commandParameter =>
            {
                if (_orderSystemRepository == null)
                {
                    return;
                }

                if (Mode == "Add")
                {
                    await _orderSystemRepository.AddOrder(_order);
                }
                else
                {
                    await _orderSystemRepository.ReplaceOrder(_order.OrderId, _order);
                }
                var window = (Window)commandParameter;
                window.DialogResult = true;
                window.Close();
            }, null);
            OpenCustomerViewCommand = new Command(async _ =>
            {
                if (_orderSystemRepository == null)
                {
                    return;
                }

                var customersViewModel = new CustomersViewModel();
                await customersViewModel.InitializeAsync(_orderSystemRepository);
                customersViewModel.ModeCustomer = "Select";
                var customersView = new CustomersView(customersViewModel);
                var dialogResult = customersView.ShowDialog();
                if (!dialogResult.HasValue || !dialogResult.Value)
                {
                    return;
                }

                _order.CustomerId = customersViewModel.SelectedCustomer.Id;
                var customer = await _orderSystemRepository.GetCustomer(_order.CustomerId);
                CustomerName = customer.FullName;
                CustomerPhone = customer.PhoneNumber;
            }, (obj) => Mode == "Add");
            AddProductCommand = new Command(async _ =>
            {
                if (_orderSystemRepository == null)
                {
                    return;
                }

                var product = new Product();
                ProductViewModel productViewModel = new();
                productViewModel.Initialize(product);
                var productView = new ProductView(productViewModel);
                if (productView.ShowDialog() == true)
                {
                    if (Mode == "Add")
                    {
                        Products.Add(product);
                        ProductsCollection.Add(productViewModel);
                        productViewModel.Num = ProductsCollection.Count;
                    }
                    else
                    {
                        await _orderSystemRepository.AddProduct(_order.OrderId, product);
                        Products.Add(product);
                        productViewModel.NameProduct = product.NameProduct;
                        productViewModel.CostProduct = product.CostProduct;
                        ProductsCollection.Add(productViewModel);
                        productViewModel.Num = ProductsCollection.Count;
                    }
                }
                GetAllCostOrder();
            }, null);
            UpdateProductCommand = new Command(async _ =>
            {
                if (_orderSystemRepository == null)
                {
                    return;
                }

                if (SelectedProduct is not null)
                {
                    ProductViewModel productViewModel = ProductsCollection.Single(prodView => prodView.Num == SelectedProduct.Num);
                    var productView = new ProductView(productViewModel);
                    if (productView.ShowDialog() == true)
                    {
                        Products[SelectedProduct.Num - 1].NameProduct = productViewModel.NameProduct;
                        Products[SelectedProduct.Num - 1].CostProduct = productViewModel.CostProduct;
                        if (Mode == "Update")
                        {
                            var product = new Product
                            {
                                NameProduct = productViewModel.NameProduct,
                                CostProduct = productViewModel.CostProduct
                            };
                            await _orderSystemRepository.ReplaceProduct(_order.OrderId, SelectedProduct.Num, product);
                        }
                    }
                    GetAllCostOrder();
                }
            }, null);
            RemoveProductCommand = new Command(async _ =>
            {
                if (_orderSystemRepository == null)
                {
                    return;
                }

                if (SelectedProduct is not null)
                {
                    if (Mode == "Add")
                    {
                        Products.RemoveAt(SelectedProduct.Num - 1);
                        ProductsCollection.Remove(SelectedProduct);
                    }
                    else
                    {
                        await _orderSystemRepository.DeleteProduct(_order.OrderId, SelectedProduct.Num);
                        Products.RemoveAt(SelectedProduct.Num - 1);
                        ProductsCollection.Remove(SelectedProduct);
                        int i = 1;
                        foreach (var prod in ProductsCollection)
                        {
                            ProductsCollection[i - 1].Num = i++;
                        }
                    }
                }
                GetAllCostOrder();
            }, null);
            RemoveAllProductsCommand = new Command(async _ =>
            {
                if (_orderSystemRepository == null)
                {
                    return;
                }

                if (Mode == "Add")
                {
                    Products.Clear();
                    ProductsCollection.Clear();
                }
                else
                {
                    await _orderSystemRepository.DeleteProducts(_order.OrderId);
                    Products.Clear();
                    ProductsCollection.Clear();
                }
                GetAllCostOrder();
            }, null);
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
            int i = 1;
            foreach (var prod in _order.Products)
            {
                var productViewModel = new ProductViewModel();
                productViewModel.Initialize(prod);
                productViewModel.Num = i++;
                ProductsCollection.Add(productViewModel);
            }
            var customer = await _orderSystemRepository.GetCustomer(_order.CustomerId);
            CustomerName = customer.FullName;
            CustomerPhone = customer.PhoneNumber;
        }
        private void GetAllCostOrder()
        {
            AmountOrder = Products.Sum(product => product.CostProduct);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}