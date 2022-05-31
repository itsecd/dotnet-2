using Lab2TaskClient;
using System.Collections.ObjectModel;
using System.Linq;
using TaskClientWPF.Commands;

namespace TaskClientWPF.ViewModels
{
    public class InputViewModel
    {
        private TaskRepositoryClient _taskRepository;
        public ObservableCollection<TaskViewModel> SelectedTasks { get; } = new ObservableCollection<TaskViewModel>();
        public string InputExecutorName { get; set; }
        public Command FindTasksCommand { get; private set; }

        public async System.Threading.Tasks.Task InitializeAsync(TaskRepositoryClient taskRepository)
        {
            _taskRepository = taskRepository;
            FindTasksCommand = new Command(async commandParameter =>
            {
                SelectedTasks.Clear();
                var tasks = await _taskRepository.GetTasksAsync();
                var executors = await _taskRepository.GetExecutorsAsync();
                var executor = new Executor();

                foreach (var ex in executors)
                {
                    if (ex.Name == InputExecutorName)
                    {
                        executor = await _taskRepository.GetExecutorAsync(ex.ExecutorId);
                        break;
                    }
                }
                var selectedTasks = tasks.Where(t => t.ExecutorId == executor.ExecutorId);

                foreach (var task in selectedTasks)
                {
                    var taskViewModel = new TaskViewModel();
                    await taskViewModel.InitializeAsync(_taskRepository, task.TaskId);
                    SelectedTasks.Add(taskViewModel);
                }

            }, null);
        }
    }
}
