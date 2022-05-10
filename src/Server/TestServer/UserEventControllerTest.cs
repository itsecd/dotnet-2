using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Server;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestServer
{
    public class UserEventControllerTest
    {
        private static UserEvent CreateUserEvent(int id, string eventName, DateTime dateTime, int eventFrequency)
        {
            var user = new User()
            {
                Id = 1,
                Name = "user",
                ChatId = 11111111,
                Toggle = true
            };
            return new UserEvent()
            {
                Id = id,
                User = user,
                EventName = eventName,
                DateNTime = dateTime,
                EventFrequency = eventFrequency
            };
        }

        [Fact]
        public async Task PostUserEventsTestAsync()
        {
            var webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            await httpClient.DeleteAsync("api/UserEvent");

            var date = DateTime.Now;
            var userEvent = CreateUserEvent(1, "testEvent", date, 1);
            var content = new StringContent(JsonConvert.SerializeObject(userEvent), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/UserEvent", content);

            var getResponse = await httpClient.GetAsync("api/UserEvent/1");
            var returnedUserEvent = JsonConvert.DeserializeObject<UserEvent>(await getResponse.Content.ReadAsStringAsync());

            Assert.Equal(userEvent, returnedUserEvent);

            await httpClient.DeleteAsync("api/UserEvent/1");
        }

        [Fact]
        public async Task GetUserEventTestAsync()
        {
            var webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            await httpClient.DeleteAsync("api/UserEvent");

            var date = DateTime.Now;
            var userEvent = CreateUserEvent(1, "testEvent", date, 1);
            var content = new StringContent(JsonConvert.SerializeObject(userEvent), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/UserEvent", content);

            var getResponse = await httpClient.GetAsync("api/UserEvent/1");
            var returnedUserEvent = JsonConvert.DeserializeObject<UserEvent>(await getResponse.Content.ReadAsStringAsync());

            Assert.Equal(userEvent, returnedUserEvent);
            HttpResponseMessage getFailedResponse = await httpClient.GetAsync("api/UserEvent/73");
            Assert.Equal(HttpStatusCode.NotFound, getFailedResponse.StatusCode);

            await httpClient.DeleteAsync("api/UserEvent/1");
        }

        [Fact]
        public async Task GetUserEventsTestAsync()
        {
            var webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            await httpClient.DeleteAsync("api/UserEvent");

            var date1 = DateTime.Now;
            var date2 = date1.AddDays(1);
            var userEvent1 = CreateUserEvent(1, "testEvent1", date1, 1);
            var userEvent2 = CreateUserEvent(2, "testEvent2", date2, 1);
            var userEvents = new List<UserEvent>() { userEvent1, userEvent2 };

            var content = new StringContent(JsonConvert.SerializeObject(userEvent1), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/UserEvent", content);
            content = new StringContent(JsonConvert.SerializeObject(userEvent2), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/UserEvent", content);

            var getResponse = await httpClient.GetAsync("api/UserEvent");
            var returnedUserEvents = JsonConvert.DeserializeObject<List<UserEvent>>(await getResponse.Content.ReadAsStringAsync());
            Assert.Equal(userEvents, returnedUserEvents);

            await httpClient.DeleteAsync("api/UserEvent/1");
            getResponse = await httpClient.GetAsync("api/UserEvent");
            returnedUserEvents = JsonConvert.DeserializeObject<List<UserEvent>>(await getResponse.Content.ReadAsStringAsync());
            Assert.NotEqual(userEvents, returnedUserEvents);

            await httpClient.DeleteAsync("api/UserEvent");
        }
        
        [Fact]
        public async Task UpdateUserEventTestAsync()
        {
            var webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            await httpClient.DeleteAsync("api/UserEvent");

            var date = DateTime.Now;
            var userEvent = CreateUserEvent(1, "testEvent", date, 1);
            var content = new StringContent(JsonConvert.SerializeObject(userEvent), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/UserEvent", content);

            userEvent.EventName = "updateTestEvent";
            content = new StringContent(JsonConvert.SerializeObject(userEvent), Encoding.UTF8, "application/json");
            await httpClient.PutAsync("api/UserEvent/1", content);

            var getResponse = await httpClient.GetAsync("api/UserEvent/1");
            var returnedUserEvent = JsonConvert.DeserializeObject<UserEvent>(await getResponse.Content.ReadAsStringAsync());

            Assert.Equal(userEvent, returnedUserEvent);
            await httpClient.DeleteAsync("api/UserEvent/1");
        }
        
        [Fact]
        public async Task DeleteUserEventTestAsync()
        {
            var webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            await httpClient.DeleteAsync("api/UserEvent");

            var date = DateTime.Now;
            var userEvent = CreateUserEvent(1, "testEvent", date, 1);
            var content = new StringContent(JsonConvert.SerializeObject(userEvent), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/UserEvent", content);

            var getResponse = await httpClient.GetAsync("api/UserEvent/1");
            var returnedUserEvent = JsonConvert.DeserializeObject<UserEvent>(await getResponse.Content.ReadAsStringAsync());
            Assert.Equal(userEvent, returnedUserEvent);

            await httpClient.DeleteAsync("api/UserEvent/1");

            HttpResponseMessage getFailedResponse = await httpClient.GetAsync("api/UserEvent/1");
            Assert.Equal(HttpStatusCode.NotFound, getFailedResponse.StatusCode);
        }
        
        [Fact]
        public async Task DeleteUsersTestAsync()
        {
            var webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            await httpClient.DeleteAsync("api/UserEvent");

            var date1 = DateTime.Now;
            var date2 = date1.AddDays(1);
            var userEvent1 = CreateUserEvent(1, "testEvent1", date1, 1);
            var userEvent2 = CreateUserEvent(2, "testEvent2", date2, 1);
            var userEvents = new List<UserEvent>() { userEvent1, userEvent2 };

            var content = new StringContent(JsonConvert.SerializeObject(userEvent1), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/UserEvent", content);
            content = new StringContent(JsonConvert.SerializeObject(userEvent2), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/UserEvent", content);

            var getResponse = await httpClient.GetAsync("api/UserEvent");
            var returnedUserEvents = JsonConvert.DeserializeObject<List<UserEvent>>(await getResponse.Content.ReadAsStringAsync());
            Assert.Equal(userEvents, returnedUserEvents);

            await httpClient.DeleteAsync("api/UserEvent");
            getResponse = await httpClient.GetAsync("api/UserEvent");
            returnedUserEvents = JsonConvert.DeserializeObject<List<UserEvent>>(await getResponse.Content.ReadAsStringAsync());
            Assert.Empty(returnedUserEvents);
            HttpResponseMessage getFailedResponse = await httpClient.GetAsync("api/UserEvent/1");
            Assert.Equal(HttpStatusCode.NotFound, getFailedResponse.StatusCode);
        }
    }
}