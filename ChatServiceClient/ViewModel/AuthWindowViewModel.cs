using ChatService;
using ChatServiceClient.Model;
using Grpc.Net.Client;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows;

namespace ChatServiceClient.ViewModel
{
    public partial class AuthWindowViewModel
    {
        public string RoomName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public ReactiveCommand<Unit, Unit> Join { get; }
        public ReactiveCommand<Unit, Unit> Create { get; }
        public Interaction<User, Unit> OpenChatWindow { get; } = new();
        public AuthWindowBase AuthWindow;
        private static readonly Chat.ChatClient Client = new(GrpcChannel.ForAddress(Properties.Settings.Default.Host));

        public AuthWindowViewModel(AuthWindowBase authWindow)
        {
            AuthWindow = authWindow;
            var canExecute = new Subject<bool>();
            Create = ReactiveCommand.CreateFromTask(() => ExclusiveWrapper(CreateImpl));
            Join = ReactiveCommand.CreateFromTask(() => ExclusiveWrapper(JoinImpl));

            async Task ExclusiveWrapper(Func<Task> impl)
            {
                try
                {
                    canExecute.OnNext(false);
                    await impl();
                }
                finally
                {
                    canExecute.OnNext(true);
                }
            }
            canExecute.OnNext(true);
        }

        private async Task CreateImpl()
        {
            if (UserName == "" || RoomName == "")
            {
                MessageBox.Show("Illegal Input Values");
                return;
            }
            else
            {
                using (var chat = Client.Create())
                {
                    await chat.RequestStream.WriteAsync(new Message { RoomName = RoomName, UserName = UserName, Text = "0" });
                    AuthWindow.Hide();
                    _ = await OpenChatWindow.Handle(new User(UserName, RoomName));
                    AuthWindow.Close();
                }
            }            
        }

        private async Task JoinImpl()
        {
            if (UserName == "" || RoomName == "")
            {
                MessageBox.Show("Illegal Input Values");
                return;
            }
            else
            {
                AuthWindow.Hide();
                _ = await OpenChatWindow.Handle(new User(UserName, RoomName));
                AuthWindow.Close();
                Application.Current.Shutdown();
            }
        }
    }
}
