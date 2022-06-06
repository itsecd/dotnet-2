using System.ComponentModel;

namespace GeoAppAtmClient.ViewModels
{
    public class AtmBalanceViewModel: INotifyPropertyChanged
    {
        public Atm AtmBalance { get; set; }

        public AtmBalanceViewModel()
        {
            AtmBalance = new Atm();
        }

        public AtmBalanceViewModel(Atm atmBalance)
        {
            AtmBalance = atmBalance;
        }

        public int Balance
        {
            get => AtmBalance.Balance;
            set
            {
                if (value == AtmBalance.Balance) return;
                AtmBalance.Balance = value;
                OnPropertyChanged(nameof(Balance));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
