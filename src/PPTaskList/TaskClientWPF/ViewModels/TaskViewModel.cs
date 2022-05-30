using System.ComponentModel;
using TaskClientWPF.Commands;
using Lab2TaskClient;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;

namespace TaskClientWPF.ViewModels
{
    public class TaskViewModel : INotifyPropertyChanged
    {
        private TaskRepositoryClient _taskRepository;
        private Lab2TaskClient.Task _task;
        private ExecutorViewModel _executor;
        private List<string> _tagsStatuses = new List<string>();
        private List<string> _tagsColors = new List<string>();
        private bool _addTag = false;
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
                if (_addTag == true)
                {
                    return _tagsStatuses[0];
                }
                var ids = _task.TagsId;
                if (ids == null|| ids.Count == 0 || _taskRepository == null)
                    return string.Empty;
                var id = ids.FirstOrDefault();
                var tag = _taskRepository.GetTagAsync(id).Result;
                return tag.TagStatus;

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
                if(_addTag == true)
                {
                    return _tagsColors[0];
                }
                var ids = _task.TagsId;
                if (ids == null || ids.Count == 0 || _taskRepository == null)
                    return string.Empty;
                var id = ids.FirstOrDefault();
                var tag = _taskRepository.GetTagAsync(id).Result;
                return tag.TagColour;
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
            _task = new Lab2TaskClient.Task()
            {
                TagsId = new List<int>()
            };
            _executor = new ExecutorViewModel();
        }

        public async System.Threading.Tasks.Task InitializeAsync(TaskRepositoryClient taskRepository, int taskId)
        {
            _taskRepository = taskRepository;

            var tasks = await _taskRepository.GetTasksAsync();
            var task = tasks.FirstOrDefault(t => t.TaskId == taskId);
            var taskExecutor = new Executor();

            if (task == null)
            {
                ModifiedTaskCommand = new Command(async commandParameter =>
                {
                    var window = (Window)commandParameter;

                    _addTag = true;
                    
                    var tags = await _taskRepository.GetTagsAsync();
                    foreach (var tag in tags)
                    {
                        if (tag.TagColour == TagsColors && tag.TagStatus == TagsStatuses)
                            _task.TagsId.Add(tag.TagId);
                    }

                    var taskDto = new TaskDto
                    {
                        HeaderText = _task.HeaderText,
                        TextDescription = _task.TextDescription,
                        Executor = taskExecutor,
                        TagsId = _task.TagsId
                    };
                    await _taskRepository.PostTaskAsync(taskDto);
                    _addTag = false;
                    window.DialogResult = true;
                    window.Close();
                }, null);

                return;
            }

            _task = task;
            ModifiedTaskCommand = new Command(async commandParameter =>
            {
                var window = (Window)commandParameter;

                _task.TagsId.Clear();
                _addTag = true;
                var tags = await _taskRepository.GetTagsAsync();
                foreach (var tag in tags)
                {
                    if (tag.TagColour == TagsColors && tag.TagStatus == TagsStatuses)
                    {
                        _task.TagsId.Add(tag.TagId);
                    }   
                }
                var taskDto = new TaskDto
                {
                    HeaderText = _task.HeaderText,
                    TextDescription = _task.TextDescription,
                    Executor = taskExecutor,
                    TagsId = _task.TagsId,
                };
                await _taskRepository.UpdateTaskAsync(taskId, taskDto);
                _addTag = false;
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
