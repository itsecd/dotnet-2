using Lab2TaskClient;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using TaskClientWPF.Commands;
using TaskClientWPF.Views;

namespace TaskClientWPF.ViewModels
{
    public class TasksViewModel : INotifyPropertyChanged
    {
        private TaskRepositoryClient _taskRepository;
        public ObservableCollection<TaskViewModel> Tasks { get; } = new ObservableCollection<TaskViewModel>();

        private TaskViewModel _selectedTask;
        public Command AddTaskCommand { get; }
        public Command UpdateTaskCommand { get; }
        public Command RemoveTaskCommand { get; }
        public Command InputNameCommand { get; }
        public TaskViewModel SelectedTask
        {
            get => _selectedTask;
            set
            {
                if (value == _selectedTask)
                {
                    return;
                }

                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }
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
                Tasks.Add(taskViewModel);
            }, null);

            UpdateTaskCommand = new Command(_ =>
            {
                if (SelectedTask != null)
                {
                    var taskViewModel = Tasks.FirstOrDefault(t => t.IdTask == SelectedTask.IdTask);
                    var taskView = new TaskView(taskViewModel);
                    taskView.ShowDialog();
                }
            }, null);

            RemoveTaskCommand = new Command(async _ =>
            {
                if (SelectedTask != null)
                {
                    await _taskRepository.RemoveTaskAsync(SelectedTask.IdTask);
                    Tasks.Remove(SelectedTask);
                }
            }, null);

            InputNameCommand = new Command(async _ =>
            {
                var inputViewModel = new InputViewModel();
                await inputViewModel.InitializeAsync(_taskRepository);
                var inputView = new InputExecutorNameView(inputViewModel);
                inputView.ShowDialog();

            }, null);
        }

        public async System.Threading.Tasks.Task InitializeAsync()
        {
            _taskRepository = new TaskRepositoryClient();

            var tasks = await _taskRepository.GetTasksAsync();
            foreach (var task in tasks)
            {
                var taskViewModel = new TaskViewModel();
                await taskViewModel.InitializeAsync(_taskRepository, task.TaskId);
                Tasks.Add(taskViewModel);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
