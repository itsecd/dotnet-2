using GeoAppAtmClient.Commands;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GeoAppAtmClient.ViewModels
{
    public class AtmViewModel : INotifyPropertyChanged
    {
        private AtmRepository _atmRepository;
        private Atm _atm;
        private AtmBalanceViewModel _atmBalance;

        public async Task InitializeAsync(AtmRepository atmRepository, string atmId)
        {

            _atmRepository = atmRepository;

            var atms = await _atmRepository.GetAtmsAsync();
            var atm = atms.FirstOrDefault(atm => atm.Id == atmId);
            if (atm == null)
            {
                return;
            }
            var atmBalance = await _atmRepository.GetAtmAsync(atmId);

            _atm = atm;
            _atmBalance = new AtmBalanceViewModel(atmBalance);
            UpdateBalanceCommand = new Command(commandParameter =>
            {
                if (commandParameter is not Window window) return;
                window = (Window)commandParameter;
                _atmRepository.ChangeAtmBalanceAsync(atmId, _atmBalance.Balance);
                window.DialogResult = true;
                window.Close();
            }, _ => true);
        }

        public string Name
        {
            get => _atm?.Name;
            set
            {
                if (value == _atm.Name) return;
                _atm.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public AtmBalanceViewModel AtmBalance
        {
            get => _atmBalance;
            set
            {
                if (value == _atmBalance) return;
                _atmBalance = value;
                OnPropertyChanged(nameof(AtmBalance));
            }
        }

        public Command UpdateBalanceCommand { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
