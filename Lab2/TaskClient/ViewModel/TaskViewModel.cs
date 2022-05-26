using System.Collections.Generic;
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
            var task = tasks.FirstOrDefault(t => t.TaskId == taskId);
            _task = task;
            var executor = await _taskRepository.GetExecutorAsync(_task.ExecutorId);
            ExecutorName = executor.Name;
            ExecutorSurname = executor.Surname;
            for(int i=0; i<TagsId.Count; i++)
            {
                var tagId = TagsId[i];
                var tag = await _taskRepository.GetTagAsync(tagId);
                TagName = tag.Name;
            }
        }


        public string TagName { get; set; }
        
        public List<int> TagsId
        {
            get => (List<int>)_task.TagsId;
            set
            {
                if (value == _task.TagsId) return;
                _task.TagsId = value;
                OnPropertyChanged(nameof(TagsId));
            }
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
