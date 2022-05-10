using System;
using System.Threading;
using System.Threading.Tasks;
using Server.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Server.Model;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Server.Services
{
    public class EventNotifyHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<EventNotifyHostedService> _logger;
        private readonly IJSONUserEventRepository _userEventRepository;
        private Timer _timer;

        public EventNotifyHostedService(ILogger<EventNotifyHostedService> logger,
                                    IJSONUserEventRepository userEventRepository)
        {
            _logger = logger;
            _userEventRepository = userEventRepository;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Event notify hosted service running.");
            _userEventRepository.LoadData();
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(31));
            return Task.CompletedTask;
        }
        
        private void DoWork(object state)
        {
            CheckTheOccurrenceOfEvent();
        }
        
        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Event notify hosted service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        
        public void Dispose() => _timer?.Dispose();

        private void CheckTheOccurrenceOfEvent()
        {
            List<UserEvent> userEvents = (List<UserEvent>)_userEventRepository.GetUserEvents();
            DateTime date = DateTime.Now.Date;
            TimeSpan time = DateTime.Now.TimeOfDay;
            bool isOccured = false;
            var responses = (from userEvent in userEvents
                       where ((userEvent.DateNTime.Date - date).Days % userEvent.EventFrequency) == 0
                       select NotifyAboutEvent(userEvent).Result);
            foreach(var response in responses)
            {
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"The user is notified of the occurrence of the event");
                    isOccured = true;
                }
            }
            if (!isOccured)
            {
                _logger.LogInformation("No event has occurred");
            }
        }

        private static async Task<HttpResponseMessage> NotifyAboutEvent(UserEvent userEvent)
        {
            var client = new HttpClient();
            var getResponse = await client.GetAsync($"api/User/{userEvent.User.Id}");
            var user = JsonConvert.DeserializeObject<User>(await getResponse.Content.ReadAsStringAsync());
            if (user.Toggle)
            {
                var response = await client.PostAsync($"https://localhost:443/send",
                                                    new StringContent(JsonConvert.SerializeObject(userEvent),
                                                    Encoding.UTF8,
                                                    "application/json"));
                return response;
            }
            else return null;
        }
    }
}