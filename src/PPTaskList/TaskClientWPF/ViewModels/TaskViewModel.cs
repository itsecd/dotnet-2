using TaskClientWPF;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using TaskClientWPF.Commands;

namespace TaskClientWPF.ViewModels
{
    public class TaskViewModel : INotifyPropertyChanged
    {
        private TaskRepositoryClient _taskRepository;
        private TaskClientWPF.Task _task;
        private string _executorName;
        private readonly List<string> _tagsStatuses = new List<string>();
        private readonly List<string> _tagsColors = new List<string>();
        private bool _addTag;
        public Command ModifiedTaskCommand { get; private set; }

        public int IdTask
        {
            get => _task.TaskId;
            set
            {
                if (value == _task.TaskId)
                {
                    return;
                }

                _task.TaskId = value;
                OnPropertyChanged(nameof(IdTask));
            }
        }
        public string TaskHeader
        {
            get => _task?.HeaderText;
            set
            {
                if (value == _task.HeaderText)
                {
                    return;
                }

                _task.HeaderText = value;
                OnPropertyChanged(nameof(TaskHeader));
            }
        }

        public string TaskDescription
        {
            get => _task?.TextDescription;
            set
            {
                if (value == _task.TextDescription)
                {
                    return;
                }

                _task.TextDescription = value;
                OnPropertyChanged(nameof(TaskDescription));
            }
        }

        public string ExecutorName
        {
            get => _executorName;
            set
            {
                if (value.StartsWith("System.Windows.Controls.ComboBoxItem: "))
                {
                    var name = value.Substring(38);
                    _executorName = name;
                    OnPropertyChanged(nameof(ExecutorName));
                    return;
                }
                _executorName = value;
                OnPropertyChanged(nameof(ExecutorName));
            }
        }

        public string TagsStatuses
        {
            get
            {
                if (_addTag)
                {
                    return _tagsStatuses[0];
                }
                var ids = _task.TagsId;
                if (ids == null || ids.Count == 0 || _taskRepository == null)
                {
                    return string.Empty;
                }

                var id = ids.FirstOrDefault();
                var tag = _taskRepository.GetTagAsync(id).Result;
                return tag.TagStatus;

            }
            set
            {
                if (_tagsStatuses.Contains(value))
                {
                    return;
                }

                if (value.StartsWith("System.Windows.Controls.ComboBoxItem: "))
                {
                    var status = value.Substring(38);
                    _tagsStatuses.Add(status);
                    OnPropertyChanged(nameof(TagsStatuses));
                    return;
                }
                _tagsStatuses.Add(value);
                OnPropertyChanged(nameof(TagsStatuses));
            }
        }
        public string TagsColors
        {
            get
            {
                if (_addTag)
                {
                    return _tagsColors[0];
                }
                var ids = _task.TagsId;
                if (ids == null || ids.Count == 0 || _taskRepository == null)
                {
                    return string.Empty;
                }

                var id = ids.FirstOrDefault();
                var tag = _taskRepository.GetTagAsync(id).Result;
                return tag.TagColour;
            }
            set
            {
                if (_tagsColors.Contains(value))
                {
                    return;
                }

                if (value.StartsWith("System.Windows.Controls.ComboBoxItem: "))
                {
                    var color = value.Substring(38);
                    _tagsColors.Add(color);
                    OnPropertyChanged(nameof(TagsColors));
                    return;
                }
                _tagsColors.Add(value);
                OnPropertyChanged(nameof(TagsColors));
            }
        }

        public TaskViewModel()
        {
            _task = new TaskClientWPF.Task()
            {
                TagsId = new List<int>()
            };
            _executorName = string.Empty;
        }

        public async System.Threading.Tasks.Task InitializeAsync(TaskRepositoryClient taskRepository, int taskId)
        {
            _taskRepository = taskRepository;
     
            Task task = null;
            ICollection<Tag> tags = null;
            ICollection<Executor> executors = null;

            if (_taskRepository != null)
            {
                var tasks = await _taskRepository.GetTasksAsync();
                task = tasks.FirstOrDefault(t => t.TaskId == taskId);
                tags = await _taskRepository.GetTagsAsync();
                executors = await _taskRepository.GetExecutorsAsync();
            }
            if(task!= null) 
            {
                _task = task;
                var executor = executors.FirstOrDefault(e => e.ExecutorId == task.ExecutorId);
                if (executor != null)
                    _executorName = executor.Name;
                else  
                    _executorName = string.Empty;
            }

            if (task == null)
            {
                ModifiedTaskCommand = new Command(async commandParameter =>
                {
                    var window = (Window)commandParameter;

                    _addTag = true;

                    foreach (var tag in tags)
                    {
                        if (tag.TagColour == TagsColors && tag.TagStatus == TagsStatuses)
                        {
                            _task.TagsId.Add(tag.TagId);
                        }
                    }
                    foreach (var ex in executors)
                    {
                        if (ex.Name == ExecutorName)
                        {
                            _task.ExecutorId = ex.ExecutorId;
                        }
                    }
                    var taskExecutor = new Executor
                    {
                        ExecutorId = _task.ExecutorId,
                        Name = ExecutorName
                    };
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

            ModifiedTaskCommand = new Command(async commandParameter =>
            {
                _addTag = true;
                var window = (Window)commandParameter;

                _task.TagsId.Clear();
                foreach (var tag in tags)
                {
                    if (tag.TagColour == TagsColors && tag.TagStatus == TagsStatuses)
                    {
                        _task.TagsId.Add(tag.TagId);
                    }
                }
                foreach (var ex in executors)
                {
                    if (ex.Name == ExecutorName)
                    {
                        _task.ExecutorId = ex.ExecutorId;
                    }
                }
                var taskExecutor = new Executor
                {
                    ExecutorId = _task.ExecutorId,
                    Name = ExecutorName
                };
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
