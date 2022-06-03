using ChatService;
using ChatService.Model;
using Grpc.Net.Client;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServiceClient.ViewModel
{
    public sealed class ChatWindowViewModel
    {
        public string Text { get; set; } = string.Empty;
        public ReactiveCommand<Unit, Unit> Send { get; }
        public ReactiveCommand<Unit, Unit> Close { get; }
        public Interaction<Unit?, Unit> CloseChatWindow { get; } = new(RxApp.MainThreadScheduler);
        public ObservableCollection<string> Messages { get; set; } = new();
        public User User { get; }
        private ChatWindow ChatWindow { get; set; }
        private static readonly Chat.ChatClient Client = new(GrpcChannel.ForAddress(Properties.Settings.Default.Host));
        private readonly Grpc.Core.AsyncDuplexStreamingCall<Message, Message> room;

        public ChatWindowViewModel(User user, ChatWindow chatWindow)
        {
            ChatWindow = chatWindow;
            User = user;
            room = Client.Join();
            var canExecute = new Subject<bool>();
            Send = ReactiveCommand.CreateFromTask(() => ExclusiveWrapper(SendImpl));
            _ = ReactiveCommand.CreateFromTask(() => ExclusiveWrapper(WaitResponce));
            Close = ReactiveCommand.CreateFromTask(() => ExclusiveWrapper(CloseImpl));

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

        private async Task CloseImpl()
        {
            ChatWindow.Hide();
            _ = await CloseChatWindow.Handle(null);
            ChatWindow.Close();
            App.Current.Shutdown();
        }

        private async Task SendImpl()
        {
            await room.RequestStream.WriteAsync(new Message { RoomName = User.RoomName, UserName = User.UserName, Text = Text });
            _ = WaitResponce();
        }

        private async Task<string> WaitResponce()
        {
            while (await room.ResponseStream.MoveNext(cancellationToken: CancellationToken.None))
            {
                var response = room.ResponseStream.Current;
                Messages.Add($"{response.UserName}: {response.Text}");
            }
            return "Not Wait";
        }
    }
}
