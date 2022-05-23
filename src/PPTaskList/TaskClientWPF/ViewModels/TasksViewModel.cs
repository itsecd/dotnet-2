using System.Collections.ObjectModel;
using System.ComponentModel;
using TaskClientWPF.Commands;
using System.Threading.Tasks;
using TaskClientWPF.Views;


namespace TaskClientWPF.ViewModels
{
    public class TasksViewModel: INotifyPropertyChanged
    {
        private TaskRepositoryClient _taskRepository;
        public ObservableCollection<TasksViewModel> Tasks { get; } = new ObservableCollection<TasksViewModel>();

        private TasksViewModel _selectedTask;
        public TasksViewModel SelectedTask
        {
            get => _selectedTask;
            set
            {
                if (value == _selectedTask) return;
                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }

        public Command ShowTaskCommand { get; private set; }
        public async Task InitializeAsync(TaskRepositoryClient taskRepository)
        {
            _taskRepository = taskRepository;

            var tasks = await _taskRepository.GetTasksAsync();
            foreach(var task in tasks)
            {
                var taskViewModel = new TasksViewModel();
                await taskViewModel.InitializeAsync(taskRepository, task.HeaderText);
                Tasks.Add(taskViewModel);
            }
            ShowTaskCommand = new Command(commandParameter =>
            {
                var taskInfoView = new TaskView(SelectedTask);
                taskInfoView.ShowDialog();
            }, null);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
