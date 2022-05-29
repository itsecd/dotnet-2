using System.ComponentModel;
using TaskClientWPF.Commands;
using Lab2TaskClient;
using System.Linq;
using System.Windows;
using System.Collections.Generic;

namespace TaskClientWPF.ViewModels
{
    public class TaskViewModel : INotifyPropertyChanged
    {
        private TaskRepositoryClient _taskRepository;
        private Lab2TaskClient.Task _task;
        private ExecutorViewModel _executor;
        private List<string> _tagsStatuses;
        private List<string> _tagsColors;
        public Command ModifiedTaskCommand { get; private set; }

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

        //public int TaskExecutorId
        //{
        //    get => _executor.ExecutorId;
        //    set
        //    {
        //        if (value == _executor.ExecutorId) return;
        //        _task.ExecutorId = value;
        //        _executor.ExecutorId = value;
        //        OnPropertyChanged(nameof(TaskExecutorId));
        //    }
        //}

        public ExecutorViewModel Executor
        {
            get => _executor;
            set
            {
                if (value == _executor) return;
                _executor = value;
                _task.ExecutorId = _executor.ExecutorId;
                OnPropertyChanged(nameof(Executor));
            }
        }

        public string TagsStatuses
        {
            get
            {
                var tagsId = _task.TagsId;
                if(tagsId != null)
                {
                    foreach (var id in tagsId)
                    {
                        var tag = _taskRepository.GetTagAsync(id).Result;
                        _tagsStatuses.Add(tag.TagStatus);
                    }
                    return _tagsStatuses[0];
                }
                return string.Empty;
            } 
            set
            {
                if (_tagsStatuses.Contains(value)) return;
                _tagsStatuses.Add(value);
                OnPropertyChanged(nameof(TagsStatuses));
            }
        }

        public string TagsColors
        {
            get
            {
                var tagsId = _task.TagsId;
                if (tagsId != null)
                {
                    foreach (var id in tagsId)
                    {
                        var tag = _taskRepository.GetTagAsync(id).Result;
                        _tagsColors.Add(tag.TagColour);
                    }
                    return _tagsColors[0];
                }
                return string.Empty;
            }
            set
            {
                if ( _tagsColors.Contains(value)) return;
                _tagsColors.Add(value);
                OnPropertyChanged(nameof(TagsColors));
            }
        }

        public TaskViewModel()
        {
            _task = new Lab2TaskClient.Task();
            _executor = new ExecutorViewModel();
            _tagsStatuses = new List<string>();
            _tagsColors = new List<string>();
        }

        public async System.Threading.Tasks.Task InitializeAsync(TaskRepositoryClient taskRepository, int taskId)
        {
            _taskRepository = taskRepository;

            var tasks = await _taskRepository.GetTasksAsync();
            var task = tasks.FirstOrDefault(t => t.TaskId == taskId);
            var taskExecutor = new Executor();

            if (task == null)
            {
                ModifiedTaskCommand = new Command(commandParameter =>
                {
                    var window = (Window)commandParameter;
                    var taskDto = new TaskDto
                    {
                        HeaderText = _task.HeaderText,
                        TextDescription = _task.TextDescription,
                        Executor = taskExecutor,
                        TagsId = _task.TagsId,
                    };
                    _taskRepository.PostTaskAsync(taskDto);
                    window.DialogResult = true;
                    window.Close();
                }, null);

                return;
            }

            _task = task;
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
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
