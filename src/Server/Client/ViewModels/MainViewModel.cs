using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

using DynamicData;
using DynamicData.Binding;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using Client.Services;
using Server.Model;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;

namespace Client.ViewModels
{
    public sealed class MainViewModel : ReactiveObject
    {
        private readonly string _requestAddress = "https://localhost:44349/api/UserEvent";
        public User User { get; init; }

        public ReadOnlyObservableCollection<UserEvent> UserEvents { get; }

        [Reactive]
        public UserEvent? SelectedUserEvent { get; set; }

        #region Commands

        public ReactiveCommand<Unit, Unit> Refresh { get; }

        public ReactiveCommand<Unit, Unit> Add { get; }

        public ReactiveCommand<Unit, Unit> Edit { get; }


        public ReactiveCommand<Unit, Unit> Delete { get; }

        public Interaction<Unit, UserEvent?> CreateUserEvent { get; } = new();

        public Interaction<Unit, UserEvent?> EditUserEvent { get; } = new();


        private async Task<List<UserEvent>> GetUserEvents(string userName)
        {
            var httpClient = new HttpClient();
            var getResponse = await httpClient.GetAsync($"{_requestAddress}");
            var returnedUserEvents = JsonConvert.DeserializeObject<List<UserEvent>>(await getResponse.Content.ReadAsStringAsync());
            return new List<UserEvent>(from UserEvent userEvent in returnedUserEvents
                                       where userEvent.User.Name == userName
                                       select userEvent);
        }

        public async Task<HttpResponseMessage> PostUserEvent(UserEvent userEvent)
        {
            var httpClient = new HttpClient();
            userEvent.User = User;
            var content = new StringContent(JsonConvert.SerializeObject(userEvent), System.Text.Encoding.UTF8, "application/json");
            var postResponse = await httpClient.PostAsync(_requestAddress, content);
            return postResponse;
        }

        public async Task<HttpResponseMessage> PutUserEvent(UserEvent userEvent)
        {
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(userEvent), System.Text.Encoding.UTF8, "application/json");
            var putResponse = await httpClient.PutAsync($"{_requestAddress}/{userEvent.Id}", content);
            return putResponse;
        }

        public async Task<HttpResponseMessage> DeleteUserEvent(UserEvent userEvent)
        {
            var httpClient = new HttpClient();
            var deleteResponse = await httpClient.DeleteAsync($"{_requestAddress}/{userEvent.Id}");
            return deleteResponse;
        }

        private async Task RefreshImpl()
        {            
            var userEvents = new List<UserEvent>(await GetUserEvents(User.Name));
            _userEventListService.Update(userEvents);
        }

        private async Task EditImpl()
        {
            if (SelectedUserEvent is not { } userEvent)
                return;
            var newUserEvent = await EditUserEvent.Handle(Unit.Default);
            if (newUserEvent is null)
                return;
            var putUserEvent = new UserEvent
            {
                Id = userEvent.Id,
                User = User,
                EventName = newUserEvent.EventName,
                DateNTime = newUserEvent.DateNTime,
                EventFrequency = newUserEvent.EventFrequency,
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
            {
                User = user;
                _userEventListService = userEventListService;
                _ = _userEventListService
                    .Connect()
                    .Sort(SortExpressionComparer<UserEvent>.Ascending(o => o.EventName))
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Bind(out var userEvents)
                    .Subscribe();
                UserEvents = userEvents;

                var canExecute = new Subject<bool>();
                var isSelected = this.WhenAnyValue(o => o.SelectedUserEvent, (UserEvent? o) => o is not null);
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
}
