using System.Windows;
using TaskClientWPF.ViewModels;

namespace TaskClientWPF.Views
{
    /// <summary>
    /// Логика взаимодействия для TasksView.xaml
    /// </summary>
    public partial class TasksView : Window
    {
        public TasksView()
        {
            InitializeComponent();
        }

        public TasksView(TasksViewModel tasksViewModel)
        {
            InitializeComponent();
            DataContext = tasksViewModel;
        }
    }
}
