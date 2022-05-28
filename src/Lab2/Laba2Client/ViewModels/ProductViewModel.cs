using System.ComponentModel;
using System.Windows;
using Laba2Client.Commands;

namespace Laba2Client.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private Product _product;
        public int Num { get; set; }
        public string NameProduct
        {
            get => _product.NameProduct;
            set
            {
                if (value == _product.NameProduct) return;
                _product.NameProduct = value;
                OnPropertyChanged(nameof(NameProduct));
            }
        }
        public double CostProduct
        {
            get => _product.CostProduct;
            set
            {
                if (value == _product.CostProduct) return;
                _product.CostProduct = value;
                OnPropertyChanged(nameof(CostProduct));
            }
        }
        public Command OkProductCommand { get; }
        public Command CancelProductCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;
        public ProductViewModel()
        {
            _product = new Product()
            {
                NameProduct = string.Empty,
                CostProduct = 0
            };
            OkProductCommand = new Command(commandParameter =>
            {
                var window = (Window)commandParameter;
                window.DialogResult = true;
                window.Close();
            }, null);
            CancelProductCommand = new Command(commandParameter =>
            {
                var window = (Window)commandParameter;
                window.DialogResult = false;
                window.Close();
            }, null);
        }
        public void Initialize(Product product)
        {
            _product = product;
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}