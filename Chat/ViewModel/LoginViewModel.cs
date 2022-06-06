using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;
using System.Windows.Controls;

namespace Chat.ViewModel
{
    public partial class LoginViewModel
    {
        public string UserName { get; set; } = string.Empty;
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
            if (UserName == "")
            {
                MessageBox.Show("Illegal Input Values");
                return;
            }
            else
            {
                LogWindow.Hide();
                _ = await OpenChatWindow.Handle(UserName);
                LogWindow.Close();
            }
        }
    }
}
