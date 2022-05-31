using System.Windows;
using TaskClientWPF.ViewModels;

namespace TaskClientWPF.Views
{
    /// <summary>
    /// Логика взаимодействия для TaskView.xaml
    /// </summary>
    public partial class TaskView : Window
    {
        public TaskView()
        {
            InitializeComponent();
        }

        public TaskView(TaskViewModel taskViewModel)
        {
            InitializeComponent();
            DataContext = taskViewModel;
        }
    }
}
