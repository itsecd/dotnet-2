using System.Windows;
using TaskClient.ViewModel;

namespace TaskClient.Views
{
    /// <summary>
    /// Логика взаимодействия для ExecutorsView.xaml
    /// </summary>
    public partial class ExecutorsView : Window
    {
        public ExecutorsView()
        {
            InitializeComponent();
        }
        public ExecutorsView(ExecutorsViewModel executorsViewModel)
        {
            InitializeComponent();
            DataContext = executorsViewModel;
        }
    }
}
