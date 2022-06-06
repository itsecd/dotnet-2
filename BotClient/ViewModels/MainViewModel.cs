using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using BotClient.Services;
using Newtonsoft.Json;
using Lab2Server.Models;
using BotClient.Properties;

namespace BotClient.ViewModels
{
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly string _serverAddress = Settings1.Default.OpenApiServer;
        public User User { get; init; }

        public ReadOnlyObservableCollection<Reminder> UserEvents { get; }

        [Reactive]
        public Reminder? SelectedUserEvent { get; set; }

        #region Commands

        public ReactiveCommand<Unit, Unit> Refresh { get; }

        public ReactiveCommand<Unit, Unit> Add { get; }

        public ReactiveCommand<Unit, Unit> Edit { get; }


        public ReactiveCommand<Unit, Unit> Delete { get; }

        public Interaction<Unit, Reminder?> CreateUserEvent { get; } = new();

        public Interaction<Unit, Reminder?> EditUserEvent { get; } = new();

        private async Task<List<Reminder>> GetUserEvents(string userName)
        {
            var httpClient = new HttpClient();
            var getResponse = await httpClient.GetAsync($"{_serverAddress}/api/UserEvent");
            var returnedUserEvents = JsonConvert.DeserializeObject<List<Reminder>>(await getResponse.Content.ReadAsStringAsync());          //ToDo
            return new List<Reminder>(User.ReminderList);
        }

        public async Task<HttpResponseMessage> PostUserEvent(Reminder userEvent)
        {
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(userEvent), System.Text.Encoding.UTF8, "application/json");             //ToDo
            var postResponse = await httpClient.PostAsync($"{_serverAddress}/api/UserEvent", content);
            return postResponse;
        }

        public async Task<HttpResponseMessage> PutUserEvent(Reminder userEvent)
        {
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(userEvent), System.Text.Encoding.UTF8, "application/json");
            var putResponse = await httpClient.PutAsync($"{_serverAddress}/api/UserEvent/{userEvent.Id}", content);
            return putResponse;
        }

        public async Task<HttpResponseMessage> DeleteUserEvent(Reminder userEvent)
        {
            var httpClient = new HttpClient();
            var deleteResponse = await httpClient.DeleteAsync($"{_serverAddress}/api/UserEvent/{userEvent.Id}");
            return deleteResponse;
        }

        private async Task RefreshImpl()
        {
            var userEvents = new List<Reminder>(User.ReminderList);
            _userEventListService.Update(userEvents);
        }

        private async Task EditImpl()
        {
            if (SelectedUserEvent is not { } userEvent)
                return;
            var newUserEvent = await EditUserEvent.Handle(Unit.Default);
            if (newUserEvent is null)
                return;
            var putUserEvent = new Reminder
            {
                Name = newUserEvent.Name,
                Description = newUserEvent.Description,
                DateTime = newUserEvent.DateTime,
                RepeatPeriod = newUserEvent.RepeatPeriod,
            };
            var result = await PutUserEvent(putUserEvent);
            if (result.IsSuccessStatusCode)
            {
                _userEventListService.Add(putUserEvent);
            }
            else return;
        }

        private async Task AddImpl()
        {
            var userEvent = await CreateUserEvent.Handle(Unit.Default);
            if (userEvent is null)
                return;
            var result = await PostUserEvent(userEvent);
            if (result.IsSuccessStatusCode)
            {
                _userEventListService.Add(userEvent);
            }
            else return;
        }

        private async Task DeleteImpl()
        {
            if (SelectedUserEvent is not { } userEvent)
                return;
            var result = await DeleteUserEvent(userEvent);
            if (result.IsSuccessStatusCode)
            {
                _userEventListService.Remove(userEvent);
            }
            else return;
        }

        private readonly UserEventListService _userEventListService;

        #endregion

        public MainViewModel(UserEventListService userEventListService, User user)
        {
            User = user;
            _userEventListService = userEventListService;
            _ = _userEventListService
                .Connect()
                .Sort(SortExpressionComparer<Reminder>.Ascending(o => o.Name))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out var userEvents)
                .Subscribe();
            UserEvents = userEvents;

            var canExecute = new Subject<bool>();
            var isSelected = this.WhenAnyValue(o => o.SelectedUserEvent, (Reminder? o) => o is not null);
            var canExecuteAndIsSelected = canExecute.CombineLatest(isSelected,
                (canExecute, isSelected) => canExecute && isSelected);

            Refresh = ReactiveCommand.CreateFromTask(() => ExclusiveWrapper(RefreshImpl), canExecute);
            Add = ReactiveCommand.CreateFromTask(() => ExclusiveWrapper(AddImpl), canExecute);
            Edit = ReactiveCommand.CreateFromTask(() => ExclusiveWrapper(EditImpl), canExecuteAndIsSelected);
            Delete = ReactiveCommand.CreateFromTask(() => ExclusiveWrapper(DeleteImpl), canExecuteAndIsSelected);

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
    }
}