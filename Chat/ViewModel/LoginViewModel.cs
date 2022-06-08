using System.Threading.Tasks;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Chat.ViewModel
{
    public partial class LoginViewModel: INotifyPropertyChanged
    {
        private string _userName;
        public string UserName { 
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            } 
        }
        public ReactiveCommand<Unit, Unit> Join { get; }
        public Interaction<string, Unit> OpenChatWindow { get; } = new();
        public LoginWindowBase LogWindow;

        public LoginViewModel(LoginWindowBase logWindow)
        {
            LogWindow = logWindow;
            var canExecute = new Subject<bool>();
            Join = ReactiveCommand.CreateFromTask(JoinImpl);
        }

        private async Task JoinImpl()
        {
            if (!(UserName is string) || UserName.Length == 0)
            {
                MessageBox.Show("Input name please");
                return;
            }
            else
            {
                LogWindow.Hide();
                _ = await OpenChatWindow.Handle(UserName);
                LogWindow.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
