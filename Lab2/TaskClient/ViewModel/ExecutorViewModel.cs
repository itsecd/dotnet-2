using System.ComponentModel;
using System.Linq;

namespace TaskClient.ViewModel
{
    public class ExecutorViewModel : INotifyPropertyChanged
    {
        private TaskRepositoryClient _taskRepository;
        private Executor _executor;

        public int Id => _executor.ExecutorId;

        public ExecutorViewModel()
        {
            _executor = new Executor();
        }

        public string Name
        {
            get => _executor?.Name;
            set
            {
                if (value == _executor.Name)
                {
                    return;
                }
                _executor.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Surname
        {
            get => _executor?.Surname;
            set
            {
                if (value == _executor.Surname)
                {
                    return;
                }

                _executor.Surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public async System.Threading.Tasks.Task InitializeAsync(TaskRepositoryClient taskRepository, int executorId)
        {
            _taskRepository = taskRepository;
            var executors = await _taskRepository.GetExecutorsAsync();
            var executor = executors.FirstOrDefault(ex => ex.ExecutorId == executorId);
            if (executor == null)
            {
                return;
            }
            _executor = executor;
        }
    }
}
