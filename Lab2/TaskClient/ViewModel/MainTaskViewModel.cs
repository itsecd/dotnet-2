using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TaskClient.ViewModel
{
    public class MainTaskViewModel : INotifyPropertyChanged
    {

        private TaskRepositoryClient _taskRepository;
        public ObservableCollection<TaskViewModel> Tasks { get; } = new ObservableCollection<TaskViewModel>();


        public Task _selectTask;

        public Task SelectedTask
        {
            get => _selectTask;
            set
            {
                if (value == _selectTask)
                {
                    return;
                }

                _selectTask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public async System.Threading.Tasks.Task InitializeAsync()
        {
            _taskRepository = new TaskRepositoryClient();

            var tasks = await _taskRepository.GetTasksAsync();
            foreach (var task in tasks)
            {
                var taskViewModel = new TaskViewModel();
                await taskViewModel.InitializeAsync(_taskRepository, task.TaskId);
                Tasks.Add(taskViewModel);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
