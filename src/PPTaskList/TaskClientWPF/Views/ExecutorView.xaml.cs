using System.Windows;
using TaskClientWPF.ViewModels;

namespace TaskClientWPF.Views
{
    /// <summary>
    /// Логика взаимодействия для ExecutorView.xaml
    /// </summary>
    public partial class ExecutorView : Window
    {
        public ExecutorView()
        {
            InitializeComponent();
        }

        public ExecutorView(ExecutorViewModel executorViewModel)
        {
            InitializeComponent();
            DataContext = executorViewModel;
        }
    }
}
