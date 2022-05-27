using System.Collections.ObjectModel;
using System.ComponentModel;
using TaskClientWPF.Commands;
using System.Threading.Tasks;
using TaskClientWPF.Views;
using Lab2TaskClient;
using System.Linq;
using System.Windows;

namespace TaskClientWPF.ViewModels
{
    public class TasksViewModel: INotifyPropertyChanged
    {
        private TaskRepositoryClient _taskRepository;
        public ObservableCollection<TaskViewModel> Tasks { get; } = new ObservableCollection<TaskViewModel>();

        private TaskViewModel _selectedTask;
        public TaskViewModel SelectedTask
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
        public Command AddTaskCommand { get; private set; }
        public Command UpdateTaskCommand { get; private set; }
        public Command RemoveTaskCommand { get; private set; }
        public Command RemoveAllTasksCommand { get; private set; }

        public TasksViewModel()
        {
            AddTaskCommand = new Command(async _ =>
            {
                var tasks = await _taskRepository.GetTasksAsync();
                var id = tasks.Max(t => t.TaskId) + 1;
                var taskViewModel = new TaskViewModel();
                await taskViewModel.InitializeAsync(_taskRepository, id);
                var taskView = new TaskView(taskViewModel);
                taskView.ShowDialog();
            }, null);

            UpdateTaskCommand = new Command(_ =>
            {
                if(SelectedTask != null)
                {
                    var taskViewModel = Tasks.Single(t => t.IdTask == SelectedTask.IdTask);
                    var taskView = new TaskView(taskViewModel);
                    taskView.ShowDialog();
                }
            }, null);
        }

        public async System.Threading.Tasks.Task InitializeAsync()
        {
            _taskRepository = new TaskRepositoryClient();

            var tasks = await _taskRepository.GetTasksAsync();
            foreach(var task in tasks)
            {
                var taskViewModel = new TaskViewModel();
                await taskViewModel.InitializeAsync(_taskRepository, task.TaskId);
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
