using Grpc.Net.Client;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using TaskListKhvatskova.Models;

namespace TaskListKhvatskovaClient.ViewModels
{
    public partial class MainWindowViewModel
    {
        public Tags SelectedTag { get; set; } = new();
        public ObservableCollection<Tags> SourceTags { get; set; } = new();
        public ReactiveCommand<Unit, Unit> AddTag { get; }
        public ReactiveCommand<Unit, Unit> DeleteTag { get; }
        public Interaction<Unit, Unit> CreateTag { get; } = new();
        public MyTask SelectedTask { get; set; } = new();
        public ObservableCollection<MyTask> SourceTasks { get; set; } = new();
        public ReactiveCommand<Unit, Unit> AddTask { get; }
        public ReactiveCommand<Unit, Unit> DeleteTask { get; }
        public Interaction<Unit, Unit> CreateTask { get; } = new();
        public Executor SelectedExecutor { get; set; } = new();
        public ObservableCollection<Executor> SourceExecutors { get; set; } = new();
        public ReactiveCommand<Unit, Unit> AddExecutor { get; }
        public ReactiveCommand<Unit, Unit> DeleteExecutor { get; }
        public Interaction<Unit, Unit> CreateExecutor { get; } = new();
        public ReactiveCommand<Unit, Unit> UpdateTable { get; }
        public bool SelectStatus { get; set; } = false;
        public ReactiveCommand<Unit, Unit> ChangeStatus { get; }
        private static readonly TaskListKhvatskova.tasksList.tasksListClient Client = new(GrpcChannel.ForAddress(Properties.Settings.Default.Host));

        public MainWindowViewModel()
        {
            AddTag = ReactiveCommand.CreateFromTask(AddTagImpl);
            DeleteTag = ReactiveCommand.Create(DeleteTagImpl);
            AddTask = ReactiveCommand.CreateFromTask(AddTaskImpl);
            DeleteTask = ReactiveCommand.Create(DeleteTaskImpl);
            AddExecutor = ReactiveCommand.CreateFromTask(AddExecutorImpl);
            DeleteExecutor = ReactiveCommand.Create(DeleteExecutorImpl);
            UpdateTable = ReactiveCommand.Create(UpdateTableImpl);
            ChangeStatus = ReactiveCommand.Create(ChangeStatusImpl);

            UpdateTableImpl();
        }

        private void UpdateTableImpl()
        {
            UpdateTagTable();
            UpdateTaskTable();
            UpdateExecutorTable();
        }

        private async Task AddTagImpl()
        {
            await CreateTag.Handle(Unit.Default);
        }

        private void DeleteTagImpl()
        {
            Client.RemoveTag(new TaskListKhvatskova.TagRequest
            {
                TagId = SelectedTag.TagId
            });
        }

        private async Task AddTaskImpl()
        {
            await CreateTask.Handle(Unit.Default);
        }

        private void DeleteTaskImpl()
        {
            Client.RemoveTask(new TaskListKhvatskova.TaskRequest
            {
                TaskId = SelectedTask.TaskId
            });
        }

        private async Task AddExecutorImpl()
        {
            await CreateExecutor.Handle(Unit.Default);
        }

        private void DeleteExecutorImpl()
        {
            Client.RemoveExecutor(new TaskListKhvatskova.ExecutorRequest
            {
                ExecutorId = SelectedExecutor.ExecutorId
            });
        }

        private void ChangeStatusImpl()
        {
            Client.UpdateTaskState(new TaskListKhvatskova.TaskRequest
            {
                TaskId = SelectedTask.TaskId,
                TaskState = SelectStatus
            });
        }

        private void UpdateTagTable()
        {
            var reply = Client.GetAllTags(new TaskListKhvatskova.NullRequest { });
            SourceTags.Clear();
            foreach (var tag in reply.Tags)
            {
                SourceTags.Add(new Tags(tag.TagId, tag.Name, Color.FromName(tag.Color)));
            }
        }

        private void UpdateTaskTable()
        {
            var reply = Client.GetAllTasks(new TaskListKhvatskova.NullRequest { });
            SourceTasks.Clear();
            foreach (var task in reply.Tasks)
            {
                var tags = string.Empty;
                foreach (var tagId in task.TagsId)
                {
                    tags += tagId + "\n";
                }
                SourceTasks.Add(new MyTask(task.TaskId, task.Name, task.Description, task.TaskState, task.ExecutorId, tags));
            }
        }

        private void UpdateExecutorTable()
        {
            var reply = Client.GetAllExecutors(new TaskListKhvatskova.NullRequest { });
            SourceExecutors.Clear();
            foreach (var executor in reply.Executors)
            {
                SourceExecutors.Add(new Executor(executor.ExecutorId, executor.Name, executor.Surname));
            }
        }
    }
}
