using ChatService;
using Grpc.Net.Client;
using ReactiveUI;
using System;
using System.Reactive;

namespace ChatServiceClient.ViewModel
{
    public partial class AuthWindowViewModel
    {
        public string RoomName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public ReactiveCommand<Unit, Unit> Join { get; }
        public ReactiveCommand<Unit, Unit> Create { get; }
        public ReactiveCommand<Unit, Unit> Cancel { get; }
        public Interaction<Unit?, Unit> Close { get; } = new(RxApp.MainThreadScheduler);
        private static readonly Chat.ChatClient Client = new(GrpcChannel.ForAddress(Properties.Settings.Default.Host));

        public AuthWindowViewModel()
        {
            Create = ReactiveCommand.CreateFromObservable(CreateImpl);
            Join = ReactiveCommand.CreateFromObservable(JoinImpl);
            Cancel = ReactiveCommand.CreateFromObservable(CancelImpl);
        }

        private IObservable<Unit> CreateImpl()
        {
            Client.Create();


            return Close.Handle(null);
        }

        private IObservable<Unit> JoinImpl()
        {
            Client.Create();


            return Close.Handle(null);
        }

        private IObservable<Unit> CancelImpl()
        {
            return Close.Handle(null);
        }
    }
}
