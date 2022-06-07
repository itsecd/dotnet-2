using System.ComponentModel;
using BotClient.Commands;
using BotClient.Views;

namespace BotClient.ViewModels
{
    public class MainViewModel: INotifyPropertyChanged
    {
        private User _user;
        public User User
        {
            get => _user;
            set
            {
                if (value == _user) return;
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        public MainViewModel()
        {
            AddCall = new Command(_ =>
            {
                AddReminderWindow addReminderWindow = new AddReminderWindow(new AddReminderViewModel { UserId = _user.UserId} );
                addReminderWindow.Show();
            }, _=>true);
        }

        public Command AddCall { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}