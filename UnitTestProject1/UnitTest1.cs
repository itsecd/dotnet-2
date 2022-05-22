using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Server;
using Server.Model;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestServer
{
    public class UserControllerTest
    {
        private static User CreateUser(int id, string name, long chatId)
        {
            return new User()
            {
                Id = id,
                Name = name,
                ChatId = chatId,
                Toggle = true
            };
        }

        [Fact]
        public async Task PostUserTestAsync()
        {
            var webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            await httpClient.DeleteAsync("api/User");

            var user = CreateUser(1, "testUser", 11111111);
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/User", content);

            var getResponse = await httpClient.GetAsync("api/User/1");
            var returnedUser = JsonConvert.DeserializeObject<User>(await getResponse.Content.ReadAsStringAsync());

            Assert.Equal(user, returnedUser);

            await httpClient.DeleteAsync("api/User/1");
        }

        [Fact]
        public async Task GetUserTestAsync()
        {
            var webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            await httpClient.DeleteAsync("api/User");

            var user = CreateUser(1, "testUser", 11111111);
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/User", content);

            var getResponse = await httpClient.GetAsync("api/User/1");
            var returnedUser = JsonConvert.DeserializeObject<User>(await getResponse.Content.ReadAsStringAsync());

            Assert.Equal(user, returnedUser);
            HttpResponseMessage getFailedResponse = await httpClient.GetAsync("api/User/73");
            Assert.Equal(HttpStatusCode.NotFound, getFailedResponse.StatusCode);

            await httpClient.DeleteAsync("api/User/1");
        }

        [Fact]
        public async Task GetUsersTestAsync()
        {
            var webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            await httpClient.DeleteAsync("api/User");

            var user1 = CreateUser(1, "testUser1", 11111111);
            var user2 = CreateUser(2, "testUser2", 22222222);
            var users = new List<User> { user1, user2 };

            var content = new StringContent(JsonConvert.SerializeObject(user1), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/User", content);
            content = new StringContent(JsonConvert.SerializeObject(user2), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/User", content);

            var getResponse = await httpClient.GetAsync("api/User");
            var returnedUsers = JsonConvert.DeserializeObject<List<User>>(await getResponse.Content.ReadAsStringAsync());
            Assert.Equal(users, returnedUsers);

            await httpClient.DeleteAsync("api/User/1");
            getResponse = await httpClient.GetAsync("api/User");
            returnedUsers = JsonConvert.DeserializeObject<List<User>>(await getResponse.Content.ReadAsStringAsync());
            Assert.NotEqual(users, returnedUsers);

            await httpClient.DeleteAsync("api/User");
        }

        [Fact]
        public async Task UpdateUserTestAsync()
        {
            var webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            await httpClient.DeleteAsync("api/User");

            var user = CreateUser(1, "testUser", 11111111);
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/User", content);

            user.Name = "updateTestUser";
            content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            await httpClient.PutAsync("api/User/1", content);

            var getResponse = await httpClient.GetAsync("api/User/1");
            var returnedUser = JsonConvert.DeserializeObject<User>(await getResponse.Content.ReadAsStringAsync());

            Assert.Equal(user, returnedUser);
            await httpClient.DeleteAsync("api/User/1");
        }

        [Fact]
        public async Task DeleteUserTestAsync()
        {
            var webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            await httpClient.DeleteAsync("api/User");

            var user = CreateUser(1, "testUser", 11111111);
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/User", content);

            var getResponse = await httpClient.GetAsync("api/User/1");
            var returnedUser = JsonConvert.DeserializeObject<User>(await getResponse.Content.ReadAsStringAsync());
            Assert.Equal(user, returnedUser);

            await httpClient.DeleteAsync("api/User/1");

            HttpResponseMessage getFailedResponse = await httpClient.GetAsync("api/User/1");
            Assert.Equal(HttpStatusCode.NotFound, getFailedResponse.StatusCode);
        }

        [Fact]
        public async Task DeleteUsersTestAsync()
        {
            var webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            await httpClient.DeleteAsync("api/User");

            var user1 = CreateUser(1, "testUser1", 11111111);
            var user2 = CreateUser(1, "testUser2", 22222222);
            var users = new List<User>() { user1, user2 };
            var content = new StringContent(JsonConvert.SerializeObject(user1), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/User", content);
            content = new StringContent(JsonConvert.SerializeObject(user2), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/User", content);

            var getResponse = await httpClient.GetAsync("api/User");
            var returnedUsers = JsonConvert.DeserializeObject<List<User>>(await getResponse.Content.ReadAsStringAsync());
            Assert.Equal(users, returnedUsers);

            await httpClient.DeleteAsync("api/User");
            getResponse = await httpClient.GetAsync("api/User");
            returnedUsers = JsonConvert.DeserializeObject<List<User>>(await getResponse.Content.ReadAsStringAsync());
            Assert.Empty(returnedUsers);
            HttpResponseMessage getFailedResponse = await httpClient.GetAsync("api/User/1");
            Assert.Equal(HttpStatusCode.NotFound, getFailedResponse.StatusCode);
        }
    }
}
