using System.Collections.ObjectModel;
using System.ComponentModel;
using TaskClientWPF.Commands;
using System.Threading.Tasks;
using TaskClientWPF.Views;
using Lab2TaskClient;
using System.Linq;
using System.Collections.Generic;

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

        //public string TaskExecutor
        //{
        //    get => _task?.HeaderText;
        //    set
        //    {
        //        if (value == _task.HeaderText) return;
        //        _task.HeaderText = value;
        //        OnPropertyChanged(nameof(TaskHeader));
        //    }
        //}

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

        public TaskViewModel()
        {
            _task = new Lab2TaskClient.Task();
        }

        public async System.Threading.Tasks.Task InitializeAsync(TaskRepositoryClient taskRepository, int taskId)
        {
            _taskRepository = taskRepository;

            var tasks = await _taskRepository.GetTasksAsync();
            var task = tasks.FirstOrDefault(t => t.TaskId == taskId);
            _task = task;
            if (task != null)
            {
                var taskExecutor = await _taskRepository.GetExecutorAsync(taskId);
            }

            //_executor = new ExecutorViewModel(taskExecutor);

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
