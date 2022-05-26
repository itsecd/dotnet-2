using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TaskClient.ViewModel
{
    public class ExecutorsViewModel : INotifyPropertyChanged
    {
        private TaskRepositoryClient _taskRepository;
        public ObservableCollection<ExecutorViewModel> Executors { get; } = new ObservableCollection<ExecutorViewModel>();

        private Executor _selectExecutor;

        public Executor SelectedExecutor
        {
            get => _selectExecutor;
            set
            {
                if (value == _selectExecutor)
                {
                    return;
                }

                _selectExecutor = value;
                OnPropertyChanged(nameof(SelectedExecutor));
            }
        }

        public async System.Threading.Tasks.Task InitializeAsync(TaskRepositoryClient taskRepository)
        {
            _taskRepository = taskRepository;

            var executors = await _taskRepository.GetExecutorsAsync();
            foreach (var executor in executors)
            {
                var executorViewModel = new ExecutorViewModel();
                await executorViewModel.InitializeAsync(_taskRepository, executor.ExecutorId);
                Executors.Add(executorViewModel);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
