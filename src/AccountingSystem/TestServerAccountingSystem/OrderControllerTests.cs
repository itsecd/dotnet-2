using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using AccountingSystem;
using Xunit;
using System;

namespace TestServerAccountingSystem
{
    public class OrderControllerFixture : IDisposable
    {

        public readonly HttpClient httpClient = new WebApplicationFactory<Startup>().CreateClient();

        public OrderControllerFixture()
        {
            AddOrder();           
        }

        public async Task AddOrder()
        {
            var content = new StringContent(@"{""orderId"":49, ""customer"":{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""},
                            ""status"": 0,""price"": 400,""date"":""2022-03-30"", ""products"":{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}}");
            HttpResponseMessage response = await httpClient.PostAsync("api/Order", content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("49", responseString);

        }

        public void Dispose()
        {
            DeleteOrder();
        }

        public async Task DeleteOrder()
        {
            HttpResponseMessage response = await httpClient.DeleteAsync("api/Order/49");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("49", responseString);
        }
    }
    public class OrderControllerTests : IClassFixture<OrderControllerFixture>
    {

        [Fact]
        public async Task GetOrderWithID()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync("api/Order/49");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal(@"{""orderId"":49, ""customer"":{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""},
                            ""status"": 0,""price"": 400,""date"":""2022-03-30"", ""products"":{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}}", responseString);

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
            var content = new StringContent(@"{""orderId"":0, ""customer"":{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""},
                            ""status"": 4,""price"": 20000,""date"":""2022-03-30"", ""products"":{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}}");
            HttpResponseMessage response = await httpClient.PutAsync("api/Order/49", content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("49", responseString);
        }

        [Fact]
        public async Task ChangeOrderStatus()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""orderId"":0, ""customer"":{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""},
                            ""status"": 9,""price"": 20000,""date"":""2022-03-30"", ""products"":{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}}");
            HttpResponseMessage response = await httpClient.PatchAsync("api/Order/status-49", content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("49", responseString);
        }

    }
}
