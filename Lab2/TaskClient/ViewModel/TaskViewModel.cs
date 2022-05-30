using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using TaskClient.Commands;
using TaskClient.Views;

namespace TaskClient.ViewModel
{
    public class TaskViewModel : INotifyPropertyChanged
    {
        private TaskRepositoryClient _taskRepository;
        private Task _task;
        public int Id => _task.TaskId;

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

        public event PropertyChangedEventHandler PropertyChanged;
        public Command AddTask { get; }
        public Command OpenExecutorsViewCommand { get; }
        public Command AddTagCommand { get; }
        public ObservableCollection<TagViewModel> TagCollection { get; } = new ObservableCollection<TagViewModel>();
        public TaskViewModel()
        {
            _task = new Task()
            {
                Tags = new List<Tags>()
            };
            AddTask = new Command(async commandParameter =>
            {
                if (Mode == "Add")
                {
                    await _taskRepository.PostTaskAsync(_task);
                }
                else
                {
                    await _taskRepository.PutTaskAsync(_task.TaskId, _task);
                }
                var window = (Window)commandParameter;
                window.DialogResult = true;
                window.Close();
            }, null);
            OpenExecutorsViewCommand = new Command(async _ =>
            {
                var executorsViewModel = new ExecutorsViewModel();
                await executorsViewModel.InitializeAsync(_taskRepository);
                executorsViewModel.ModeExecutor = "Select";
                var executorsView = new ExecutorsView(executorsViewModel);
                if ((bool)executorsView.ShowDialog())
                {
                    _task.ExecutorId = executorsViewModel.SelectedExecutor.Id;
                    var executor = await _taskRepository.GetExecutorAsync(_task.ExecutorId);
                    ExecutorName = executor.Name;
                    ExecutorSurname = executor.Surname;
                }
            }, (obj) => Mode == "Add");
            AddTagCommand = new Command(async _ =>
            {
                var tag = new Tags();
                TagViewModel tagViewModel = new TagViewModel();
                tagViewModel.Initialize(tag);
                var tagView = new TagView(tagViewModel);
                if (tagView.ShowDialog() == true)
                {
                    if (Mode == "Add")
                    {
                        Tags.Add(tag);
                        TagCollection.Add(tagViewModel);
                        tagViewModel.Num = TagCollection.Count;
                    }
                    else
                    {
                        await _taskRepository.AddTag(_task.TaskId, tag);
                        Tags.Add(tag);
                        tagViewModel.NameTag = tag.Name;
                        TagCollection.Add(tagViewModel);
                        tagViewModel.Num = TagCollection.Count;
                    }
                };
            }, null);
        }

        public string Mode { get; set; }
        public async System.Threading.Tasks.Task InitializeAsync(TaskRepositoryClient taskRepository, int taskId)
        {
            _taskRepository = taskRepository;

            if (Mode == "Add")
            {
                ExecutorName = string.Empty;
                ExecutorSurname = string.Empty;
                return;
            }
            _task = await _taskRepository.GetTaskAsync(taskId);
            Executor executor = await _taskRepository.GetExecutorAsync(_task.ExecutorId);
            ExecutorName = executor.Name;
            ExecutorSurname = executor.Surname;
            int i = 1;
            foreach (var prod in _task.Tags)
            {
                var tagViewModel = new TagViewModel();
                tagViewModel.Initialize(prod);
                tagViewModel.Num = i++;
                TagCollection.Add(tagViewModel);
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
                if (value == _task.Tags)
                {
                    return;
                }

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
            get => (bool)(_task?.TaskState);
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
