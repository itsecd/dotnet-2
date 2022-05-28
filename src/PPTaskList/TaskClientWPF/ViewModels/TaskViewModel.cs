using System.ComponentModel;
using TaskClientWPF.Commands;
using Lab2TaskClient;
using System.Linq;
using System.Windows;

namespace TaskClientWPF.ViewModels
{
    public class TaskViewModel : INotifyPropertyChanged
    {
        private TaskRepositoryClient _taskRepository;
        private Lab2TaskClient.Task _task;
        private TagViewModel _tag;
        private ExecutorViewModel _executor;

        public int IdTask
        {
            get => _task.TaskId;
            set
            {
                if (value == _task.TaskId) return;
                _task.TaskId = value;
                OnPropertyChanged(nameof(IdTask));
            }
        }
        public string TaskHeader
        {
            get => _task?.HeaderText;
            set
            {
                if (value == _task.HeaderText) return;
                _task.HeaderText = value;
                OnPropertyChanged(nameof(TaskHeader));
            }
        }

        public string TaskDescription
        {
            get => _task?.TextDescription;
            set
            {
                if (value == _task.TextDescription) return;
                _task.TextDescription = value;
                OnPropertyChanged(nameof(TaskDescription));
            }
        }

        public int TaskExecutorId
        {
            get => _executor.ExecutorId;
            set
            {
                if (value == _executor.ExecutorId) return;
                _executor.ExecutorId = value;
                OnPropertyChanged(nameof(TaskExecutorId));
            }
        }

        public ExecutorViewModel Executor
        {
            get => _executor;
            set
            {
                if (value == _executor) return;
                _executor = value;
                OnPropertyChanged(nameof(Executor));
            }
        }

        //public List<string> Tags
        //{
        //    get => _tag; 
        //    set
        //    {
        //        if (value == _task.TextDescription) return;
        //        _task.TextDescription = value;
        //        OnPropertyChanged(nameof(TaskDescription));
        //    }
        //}

        public Command ModifiedTaskCommand { get; private set; }
        //public Command AddTaskCommand { get; private set; }

        public TaskViewModel()
        {
            _task = new Lab2TaskClient.Task();
            _executor = new ExecutorViewModel();
        }

        public async System.Threading.Tasks.Task InitializeAsync(TaskRepositoryClient taskRepository, int taskId)
        {
            _taskRepository = taskRepository;

            var tasks = await _taskRepository.GetTasksAsync();
            var task = tasks.FirstOrDefault(t => t.TaskId == taskId);
            var taskExecutor = new Executor();

            if (task == null) return;
            _task = task;


            // _task = new Lab2TaskClient.Task();
            //if (task != null)
            //{
            //    taskExecutor = await _taskRepository.GetExecutorAsync(taskId);
            //    _task = task;

            //ModifiedTaskCommand = new Command(commandParameter =>
            //{
            //    var window = (Window)commandParameter;
            //    var taskDto = new TaskDto
            //    {
            //        HeaderText = _task.HeaderText,
            //        TextDescription = _task.TextDescription,
            //        Executor = taskExecutor,
            //        TagsId = _task.TagsId,
            //    };

            //    _taskRepository.UpdateTaskAsync(taskId, taskDto);
            //    window.DialogResult = true;
            //    window.Close();
            //}, null);
            //}
            //_executor = new ExecutorViewModel(taskExecutor);

            ModifiedTaskCommand = new Command(async commandParameter =>
            {
                var window = (Window)commandParameter;
                var taskDto = new TaskDto
                {
                    HeaderText = _task.HeaderText,
                    TextDescription = _task.TextDescription,
                    Executor = taskExecutor,
                    TagsId = _task.TagsId,
                };

                await _taskRepository.UpdateTaskAsync(taskId, taskDto);
                window.DialogResult = true;
                window.Close();
            }, null);

            //ModifiedTaskCommand = new Command(commandParameter =>
            //{
            //    var window = (Window)commandParameter;
            //    var taskDto = new TaskDto
            //    {
            //        HeaderText = _task.HeaderText,
            //        TextDescription = _task.TextDescription,
            //        Executor = taskExecutor,
            //        TagsId = _task.TagsId,
            //    };
            //    _taskRepository.PostTaskAsync(taskDto);
            //    window.DialogResult = true;
            //    window.Close();
            //}, null);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
