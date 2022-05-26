using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using TaskClient.Commands;
using TaskClient.Views;

namespace TaskClient.ViewModel
{
    public class MainTaskViewModel : INotifyPropertyChanged
    {

        private TaskRepositoryClient _taskRepository;
        public ObservableCollection<TaskViewModel> Tasks { get; } = new ObservableCollection<TaskViewModel>();

        private TaskViewModel _selectTask;

        public Command AddTaskCommand { get; }
        public Command UpdateTaskCommand { get; }
        public Command RemoveTaskCommand { get; }

        
        public MainTaskViewModel()
        {
            AddTaskCommand = new Command(async _ =>
            {
                var tasks = await _taskRepository.GetTasksAsync();
                var id = tasks.Max(task => task.TaskId) + 1;
                TaskViewModel taskViewModel = new TaskViewModel();
                await taskViewModel.InitializeAsync(_taskRepository, id);
                var taskView = new TaskView(taskViewModel);
                taskView.ShowDialog();
            }, null);

        }

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

        public event PropertyChangedEventHandler PropertyChanged;
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

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
