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
            var atmStatus = await _atmRepository.GetAtmBalanceAsync(atmId);

            _atm = atm;
            _atmBalance = new AtmBalanceViewModel(atmStatus);
            UpdateBalanceCommand = new Command(commandParameter =>
            {
                var window = (Window)commandParameter;
                _atmRepository.UpdateAtmAsync(atmId, _atmBalance.AtmBalance);
                window.DialogResult = true;
                window.Close();
            }, null);
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

        public Command UpdateBalanceCommand { get; private set; }

        public AtmBalanceViewModel AtmStatus
        {
            get => _atmBalance;
            set
            {
                if (value == _atmBalance) return;
                _atmBalance = value;
                OnPropertyChanged(nameof(AtmBalance));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
