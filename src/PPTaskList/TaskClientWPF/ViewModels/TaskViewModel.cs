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

        public async System.Threading.Tasks.Task InitializeAsync(TaskRepositoryClient taskRepository, int taskId)
        {
            _taskRepository = taskRepository;

            var tasks = await _taskRepository.GetTasksAsync();
            var task = tasks.FirstOrDefault(t => t.TaskId == taskId);
            var taskExecutor = await _taskRepository.GetExecutorAsync(taskId);

            _task = task;
            _executor = new ExecutorViewModel(taskExecutor);

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
