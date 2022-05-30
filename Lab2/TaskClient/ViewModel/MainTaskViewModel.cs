using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using TaskClient.Commands;
using TaskClient.Views;

namespace TaskClient.ViewModel
{
    public class MainTaskViewModel : INotifyPropertyChanged
    {
        private TaskRepositoryClient _taskRepository;
        public ObservableCollection<TaskViewModel> Tasks { get; } = new ObservableCollection<TaskViewModel>();

        private TaskViewModel _selectTask;
        public TaskViewModel SelectedTask
        {
            get => _selectTask;
            set
            {
                if (value == _selectTask)
                {
                    return;
                }

                _selectTask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }

        public MainTaskViewModel()
        {
            AddTaskCommand = new Command(async _ =>
            {
                TaskViewModel taskViewModel = new TaskViewModel();
                int id = taskViewModel.Id;
                taskViewModel.Mode = "Add";
                await taskViewModel.InitializeAsync(_taskRepository, id);
                TaskView taskView = new TaskView(taskViewModel);
                if (taskView.ShowDialog() == true)
                {
                    Tasks.Clear();
                    await InitializeAsync();
                }
            }, null);

            UpdateTaskCommand = new Command(_ =>
            {
                if (SelectedTask != null)
                {
                    TaskViewModel taskViewModel = Tasks.Single(tv => tv.Id == SelectedTask.Id);
                    taskViewModel.Mode = "Update";
                    TaskView taskView = new TaskView(taskViewModel);
                    taskView.ShowDialog();
                }
            }, null);

            RemoveTaskCommand = new Command(async _ =>
            {
                if (SelectedTask != null)
                {
                    await _taskRepository.RemoveTaskAsync(SelectedTask.Id);
                    Tasks.Remove(SelectedTask);
                }
            }, null);

            RemoveAllTasksCommand = new Command(async _ =>
            {
                await _taskRepository.DeleteAllTaskAsync();
                Tasks.Clear();
            }, null);

            OpenExecutorsViewCommand = new Command(async commandParameter =>
            {
                var window = (Window)commandParameter;
                ExecutorsViewModel executorsViewModel = new ExecutorsViewModel();
                await executorsViewModel.InitializeAsync(_taskRepository);
                ExecutorsView executorsView = new ExecutorsView(executorsViewModel);
                window.Hide();
                executorsView.Owner = window;
                Application.Current.MainWindow = executorsView;
                executorsView.Show();
            }, null);

        }

        public async System.Threading.Tasks.Task InitializeAsync()
        {
            _taskRepository = new TaskRepositoryClient();

            var tasks = await _taskRepository.GetTasksAsync();
            foreach (var task in tasks)
            {
                TaskViewModel taskViewModel = new TaskViewModel();
                taskViewModel.Mode = "Update";
                await taskViewModel.InitializeAsync(_taskRepository, task.TaskId);
                Tasks.Add(taskViewModel);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public Command AddTaskCommand { get; }
        public Command UpdateTaskCommand { get; }
        public Command RemoveTaskCommand { get; }
        public Command RemoveAllTasksCommand { get; }
        public Command OpenExecutorsViewCommand { get; }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
