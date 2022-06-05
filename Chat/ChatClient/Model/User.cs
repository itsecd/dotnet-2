using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChatClient.Model
{
    public class User : INotifyPropertyChanged
    {
        private string userName { get; set; }

        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged(UserName);
            }
        }

        public User(string userName)
        {
            UserName = userName;
        }

        public event PropertyChangedEventHandler PropertyChanged; //Событие, которое будет вызвано при изменении модели 
        public void OnPropertyChanged([CallerMemberName] string prop = "") //Метод, который скажет ViewModel, что нужно передать виду новые данные
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
