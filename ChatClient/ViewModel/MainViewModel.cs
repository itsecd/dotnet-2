using ReactiveUI;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Runtime.CompilerServices;
namespace ChatClient.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ChatRoom> ?_rooms;
        public ObservableCollection<ChatRoom> Rooms
        {
            get { return _rooms; }
            set
            {
                _rooms = value;
                OnPropertyChanged(nameof(Rooms));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public ReactiveCommand<Unit, Unit> CreateCommand { get; }
        public MainViewModel() { 
            CreateCommand = ReactiveCommand.Create(CreateImpl);
        }

        private void CreateImpl() { 
            DialogWindow dialogWindow = new DialogWindow();
            dialogWindow.ShowDialog();
        }
    }
}
