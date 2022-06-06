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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Chat.ViewModel;
using Microsoft.AspNetCore.SignalR.Client;
using ReactiveUI;

namespace Chat
{
    public class MainWindowBase : ReactiveWindow<MainWindowViewModel>
    {
    }
    public partial class MainWindow : MainWindowBase
    {
        public MainWindow()
        {
            InitializeComponent();

            _ = this.WhenActivated(cd =>
            {
                if (ViewModel is null)
                    return;

                /*cd.Add(ViewModel.CloseMainWindow.RegisterHandler(interaction =>
                {
                    var logWindow = new LogWindow();
                    var logViewModel = new LoginViewModel(logWindow);
                    logWindow.ViewModel = logViewModel;
                    Observable.Start(() =>
                    {
                        _ = logWindow.ShowDialog();
                    }, RxApp.MainThreadScheduler);
                }));*/
            });
        }
    }
}