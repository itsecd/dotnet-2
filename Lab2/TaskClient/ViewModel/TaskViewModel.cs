using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using TaskClient.Commands;

namespace TaskClient.ViewModel
{
    public class TaskViewModel : INotifyPropertyChanged
    {
        private TaskRepositoryClient _taskRepository;
        private Task _task;
        public int Id => _task.TaskId; 

        public event PropertyChangedEventHandler PropertyChanged;
        public Command AddTask { get; }
        public TaskViewModel()
        {
            _task = new Task()
            {
                Name = string.Empty, 
                Description = string.Empty,  
            };
            AddTask = new Command(async commandParameter =>
            {
                var newTask = new Task()
                {
                    Name = _task.Name,
                    Description = _task.Description,
                    TaskState = _task.TaskState,
                    Tags = _task.Tags
                    
                };
                await _taskRepository.PostTaskAsync(newTask);
                var window = (Window)commandParameter;
                window.DialogResult = true;
                window.Close();
            }, null);
        }

        public async System.Threading.Tasks.Task InitializeAsync(TaskRepositoryClient taskRepository, int taskId)
        {
            _taskRepository = taskRepository;

            var tasks = await _taskRepository.GetTasksAsync();
            var task = tasks.FirstOrDefault(t => t.TaskId == taskId);
            if (task == null)
            {
                return;
            }

            _task = task;
            var executorId = _task.ExecutorId;
            var executor = await _taskRepository.GetExecutorAsync(executorId);
            ExecutorName = executor.Name;
            ExecutorSurname = executor.Surname;
        }
        public string Mode { get; set; }
        private string _executorName;
        public string ExecutorName
        {
            get => _executorName;
            set
            {
                if (value == _executorName)
                {
                    return;
                }

                _executorName = value;
                OnPropertyChanged(nameof(ExecutorName));
            }
        }

        private string _executorSurname;
        public string ExecutorSurname
        {
            get => _executorSurname;
            set
            {
                if (value == _executorSurname)
                {
                    return;
                }

                _executorSurname = value;
                OnPropertyChanged(nameof(ExecutorSurname));
            }
        }

        public string Name
        {
            get => _task?.Name;
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

        public List<Tags> Tags
        {
            get => (List<Tags>)_task?.Tags;
            set
            {
                if (value == _task.Tags) return;
                _task.Tags = value;
                OnPropertyChanged(nameof(Tags));
            }
        }
        public string Description
        {
            get => _task?.Description;
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
