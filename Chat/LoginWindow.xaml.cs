using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Chat.ViewModel;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

//<v:LoginWindowBase x:Class="Chat.LoginWindow"

namespace Chat
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    /// 
    public class LoginWindowBase : ReactiveWindow<LoginViewModel>   
    {
    }
    public partial class LoginWindow : LoginWindowBase
    {
        public LoginWindow()
        {
            InitializeComponent();

            _ = this.WhenActivated(cd =>
            {
                if (ViewModel is null)
                    return;

                ViewModel.LogWindow = this;
                cd.Add(ViewModel.OpenChatWindow.RegisterHandler(interaction =>
                {
                    var chatWindow = new MainWindow();
                    var chatWindowViewModel = new MainWindowViewModel(interaction.Input, chatWindow);
                    chatWindow.ViewModel = chatWindowViewModel;
                    Observable.Start(() =>
                    {
                        _ = chatWindow.ShowDialog();
                    }, RxApp.MainThreadScheduler);
                }));
            });
        }

        /*private void Button_login(object sender, RoutedEventArgs e)
        {
            if (MessageTextName.Length > 0)
            {
                var window = new MainWindow(MessageTextName);
                window.Show();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MessageTextName = ((TextBox)sender).Text;
        }*/
    }
}
