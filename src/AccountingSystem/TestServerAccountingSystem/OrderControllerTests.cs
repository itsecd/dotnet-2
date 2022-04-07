using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using AccountingSystem;
using Xunit;
using System;
using System.Net;

namespace TestServerAccountingSystem
{
    public class OrderControllerTests
    {
        [Fact]
        public async Task AddOrder()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""orderId"":44, ""customer"":{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""},
                            ""status"": 0,""price"": 400,""date"":""2022-03-30"", ""products"":{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}}");
            HttpResponseMessage firstResponse = await httpClient.PostAsync("api/Order", content);
            var responseString = await firstResponse.Content.ReadAsStringAsync();
            Assert.Equal("44", responseString);
            HttpResponseMessage secondResponse = await httpClient.PostAsync("api/Order", content);
            Assert.Equal(HttpStatusCode.InternalServerError, secondResponse.StatusCode);
            await httpClient.DeleteAsync("api/Order/44");
        }

        [Fact]
        public async Task GetOrderWithID()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""orderId"":55, ""customer"":{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""},
                            ""status"": 0,""price"": 400,""date"":""2022-03-30"", ""products"":{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}}");
            await httpClient.PostAsync("api/Order", content);
            HttpResponseMessage firstResponse = await httpClient.GetAsync("api/Order/55");
            var responseString = await firstResponse.Content.ReadAsStringAsync();
            Assert.Equal(@"{""orderId"":55, ""customer"":{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""},
                            ""status"": 0,""price"": 400,""date"":""2022-03-30"", ""products"":{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}}", responseString);
            HttpResponseMessage secondResponse = await httpClient.GetAsync("api/Order/66");
            Assert.Equal(HttpStatusCode.NotFound, secondResponse.StatusCode);
            await httpClient.DeleteAsync("api/Order/55");
        }

        [Fact]
        public async Task GetAllPrice()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync("api/Order/all-price");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("500", responseString);

        }

        [Fact]
        public async Task ChangeOrder()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            await httpClient.PostAsync("api/Order", new StringContent(@"{""orderId"":77, ""customer"":{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""},
                            ""status"": 0,""price"": 400,""date"":""2022-03-30"", ""products"":{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}}"));
            var content = new StringContent(@"{""orderId"":77, ""customer"":{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""},
                            ""status"": 4,""price"": 20000,""date"":""2022-03-30"", ""products"":{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}}");
            HttpResponseMessage firstResponse = await httpClient.PutAsync("api/Order/77", content);
            var responseString = await firstResponse.Content.ReadAsStringAsync();
            Assert.Equal("77", responseString);
            HttpResponseMessage secondResponse = await httpClient.PutAsync("api/Order/88", content);
            Assert.Equal(HttpStatusCode.NotFound, secondResponse.StatusCode);
            await httpClient.DeleteAsync("api/Order/77");
        }

        [Fact]
        public async Task ChangeOrderStatus()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            await httpClient.PostAsync("api/Order", new StringContent(@"{""orderId"":99, ""customer"":{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""},
                            ""status"": 0,""price"": 400,""date"":""2022-03-30"", ""products"":{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}}"));
            var content = new StringContent(@"{""orderId"":99, ""customer"":{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""},
                            ""status"": 9,""price"": 20000,""date"":""2022-03-30"", ""products"":{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}}");
            HttpResponseMessage firstResponse = await httpClient.PatchAsync("api/Order/status-99", content);
            var responseString = await firstResponse.Content.ReadAsStringAsync();
            Assert.Equal("99", responseString);
            HttpResponseMessage secondResponse = await httpClient.PatchAsync("api/Order/status-110", content);
            Assert.Equal(HttpStatusCode.NotFound, secondResponse.StatusCode);
            await httpClient.DeleteAsync("api/Order/99");
        }

        [Fact]
        public async Task RemoveOrder()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            await httpClient.PostAsync("api/Order", new StringContent(@"{""orderId"":120, ""customer"":{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""},
                            ""status"": 0,""price"": 400,""date"":""2022-03-30"", ""products"":{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}}"));
            HttpResponseMessage firstResponse = await httpClient.DeleteAsync("api/Order/120");
            var responseString = await firstResponse.Content.ReadAsStringAsync();
            Assert.Equal("120", responseString);
            HttpResponseMessage secondResponse = await httpClient.DeleteAsync("api/Order/130");
            Assert.Equal(HttpStatusCode.NotFound, secondResponse.StatusCode);
        }
    }
}
