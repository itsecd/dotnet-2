using GeoAppAtmClient.Commands;
using GeoAppAtmClient.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace GeoAppAtmClient.ViewModels
{
    public class AtmsViewModel : INotifyPropertyChanged
    {
        private AtmRepository _atmRepository;
        public ObservableCollection<AtmViewModel> Atms { get; } = new();

        private AtmViewModel _selectedAtm;
        public AtmViewModel SelectedAtm
        {
            get => _selectedAtm;
            set
            {
                if (value == _selectedAtm) return;
                _selectedAtm = value;
                OnPropertyChanged(nameof(SelectedAtm));
            }
        }

        public async Task InitializeAsync(AtmRepository atmRepository)
        {
            _atmRepository = atmRepository;

            var atms = await _atmRepository.GetAtmsAsync();
            foreach (var atm in atms)
            {
                var atmViewModel = new AtmViewModel();
                await atmViewModel.InitializeAsync(atmRepository, atm.Id);
                Atms.Add(atmViewModel);
            }

            ShowAtmCommand = new Command(commandParameter =>
            {
                var atmInfoView = new AtmView(SelectedAtm);
                atmInfoView.ShowDialog();
            }, null);
        }

        public Command ShowAtmCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
