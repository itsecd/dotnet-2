using Grpc.Net.Client;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows.Controls;

namespace TaskListKhvatskovaClient.ViewModels
{
    public partial class AddTaskViewModel
    {
        public ObservableCollection<ComboBoxItem> SourceTags { get; set; } = new();
        public ObservableCollection<ComboBoxItem> SourceExecutor { get; set; } = new();
        public ComboBoxItem SelectTags { get; set; } = new();
        public ComboBoxItem SelectExecutor { get; set; } = new();
        public bool SelectStatus { get; set; } = false;
        public string NameInput { get; set; } = string.Empty;
        public string DescriptionInput { get; set; } = string.Empty;
        public ReactiveCommand<Unit, Unit> Add { get; }
        public ReactiveCommand<Unit, Unit> Cancel { get; }
        public Interaction<Unit?, Unit> Close { get; } = new(RxApp.MainThreadScheduler);
        private static readonly TaskListKhvatskova.tasksList.tasksListClient Client = new(GrpcChannel.ForAddress(Properties.Settings.Default.Host));

        public AddTaskViewModel()
        {
            Add = ReactiveCommand.CreateFromObservable(AddImpl);
            Cancel = ReactiveCommand.CreateFromObservable(CancelImpl);
            UpdateTagsItems();
            UpdateExecutorsItems();
        }

        private IObservable<Unit> AddImpl()
        {
            var taskRequest = new TaskListKhvatskova.TaskRequest();
            taskRequest.ExecutorId = int.Parse(SelectExecutor.DataContext.ToString());
            foreach (ComboBoxItem item in SourceTags)
            {
                if (item.Content is not CheckBox { IsChecked: true } checkBox) continue;
                taskRequest.TagsId.Add(int.Parse(checkBox.DataContext.ToString()));
            }
            taskRequest.TaskState = SelectStatus;
            taskRequest.Name = NameInput;
            taskRequest.Description = DescriptionInput;
            Client.AddTask(taskRequest);

            return Close.Handle(null);
        }

        private IObservable<Unit> CancelImpl()
        {
            return Close.Handle(null);
        }

        private void UpdateTagsItems()
        {
            var reply = Client.GetAllTags(new TaskListKhvatskova.NullRequest());
            foreach (var item in reply.Tags)
            {
                SourceTags.Add(new ComboBoxItem()
                {
                    Content = new CheckBox()
                    {
                        DataContext = item.TagId,
                        Content = item.Name
                    }
                });
            }
        }

        private void UpdateExecutorsItems()
        {
            var reply = Client.GetAllExecutors(new TaskListKhvatskova.NullRequest());
            foreach (var item in reply.Executors)
            {
                SourceExecutor.Add(new ComboBoxItem() { DataContext = item.ExecutorId, Content = item.Name });
            }
        }
    }
}
