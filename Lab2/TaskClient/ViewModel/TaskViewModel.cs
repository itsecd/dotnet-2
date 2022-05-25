using System.ComponentModel;
using System.Linq;

namespace TaskClient.ViewModel
{
    public class TaskViewModel : INotifyPropertyChanged
    {
        private TaskRepositoryClient _taskRepository;
        private Task _task;

        public event PropertyChangedEventHandler PropertyChanged;

        public async System.Threading.Tasks.Task InitializeAsync(TaskRepositoryClient taskRepository, int taskId)
        {
            _taskRepository = taskRepository;

            var tasks = await _taskRepository.GetTasksAsync();
            _task = tasks.FirstOrDefault(task => task.TaskId == taskId);

            var executor = await _taskRepository.GetExecutorAsync(_task.ExecutorId);
            var ExecutorName = executor.Name;
            var ExecutorSurname = executor.Surname;

        }

        public string ExecutorName { get; set; }
        public string ExecutorSurname { get; set; }


        public string Name
        {
            get => _task.Name;
            set
            {
                if (value == _task.Name)
                {
                    return;
                }

                _task.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Description
        {
            get => _task.Description;
            set
            {
                if (value == _task.Description)
                {
                    return;
                }

                _task.Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public bool State
        {
            get => _task.TaskState;
            set
            {
                if (value == _task.TaskState)
                {
                    return;
                }

                _task.TaskState = value;
                OnPropertyChanged(nameof(State));
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
