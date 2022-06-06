using Grpc.Net.Client;
using ReactiveUI;
using System;
using System.Reactive;
using Xceed.Wpf.Toolkit;

namespace TaskListKhvatskovaClient.ViewModels
{
    public partial class AddTagViewModel
    {
        public string NameInput { get; set; } = string.Empty;
        public System.Windows.Media.Color Color { get; set; } = new();
        public ReactiveCommand<Unit, Unit> Add { get; }
        public ReactiveCommand<Unit, Unit> Cancel { get; }
        public Interaction<Unit?, Unit> Close { get; } = new(RxApp.MainThreadScheduler);
        private static readonly TaskListKhvatskova.tasksList.tasksListClient Client = new(GrpcChannel.ForAddress(Properties.Settings.Default.Host));

        public AddTagViewModel()
        {

            Add = ReactiveCommand.CreateFromObservable(AddImpl);
            Cancel = ReactiveCommand.CreateFromObservable(CancelImpl);
        }

        private IObservable<Unit> AddImpl()
        {
            if (NameInput != "")
            {
                Client.AddTag(new TaskListKhvatskova.TagRequest
                {
                    Name = NameInput,
                    Color = ColorToColor(Color).Name
                });
            }
            return Close.Handle(null);
        }

        private IObservable<Unit> CancelImpl()
        {
            return Close.Handle(null);
        }

        private static System.Drawing.Color ColorToColor(System.Windows.Media.Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
