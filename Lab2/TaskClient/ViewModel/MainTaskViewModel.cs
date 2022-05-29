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
        public Command AddTaskCommand { get; }
        public Command UpdateTaskCommand { get; }
        public Command RemoveTaskCommand { get; }
        public Command RemoveAllTasksCommand { get; }
        public Command OpenExecutorsViewCommand { get; }

        public MainTaskViewModel()
        {
            AddTaskCommand = new Command(async _ =>
            {
                var tasks = await _taskRepository.GetTasksAsync();
                var id = tasks.Max(task => task.TaskId) + 1;
                TaskViewModel taskViewModel = new TaskViewModel();
                taskViewModel.Mode = "Add";
                await taskViewModel.InitializeAsync(_taskRepository, id);
           
                var taskView = new TaskView(taskViewModel);
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
                    var taskViewModel = Tasks.Single(tv => tv.Id == SelectedTask.Id);
                    taskViewModel.Mode = "Update";
                    var taskView = new TaskView(taskViewModel);
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
                var executorsViewModel = new ExecutorsViewModel();
                await executorsViewModel.InitializeAsync(_taskRepository);
                var executorsView = new ExecutorsView(executorsViewModel);
                window.Hide();
                executorsView.Owner = window;
                Application.Current.MainWindow = executorsView;
                executorsView.Show();
            }, null);

        }

      

        public event PropertyChangedEventHandler PropertyChanged;
        public async System.Threading.Tasks.Task InitializeAsync()
        {
            _taskRepository = new TaskRepositoryClient();

            var tasks = await _taskRepository.GetTasksAsync();
            foreach (var task in tasks)
            {
                var taskViewModel = new TaskViewModel();
                await taskViewModel.InitializeAsync(_taskRepository, task.TaskId);
                taskViewModel.Mode = "Update";
                Tasks.Add(taskViewModel);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
