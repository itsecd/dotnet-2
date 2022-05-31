using System.Windows;
using TaskClientWPF.ViewModels;

namespace TaskClientWPF.Views
{
    /// <summary>
    /// Логика взаимодействия для InputExecutorName.xaml
    /// </summary>
    public partial class InputExecutorNameView : Window
    {
        public InputExecutorNameView()
        {
            InitializeComponent();
        }
        public InputExecutorNameView(InputViewModel inputViewModel)
        {
            InitializeComponent();
            DataContext = inputViewModel;
        }
    }
}
