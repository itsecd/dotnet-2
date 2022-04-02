using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using AccountingSystem;
using Xunit;

namespace TestServerAccountingSystem
{
    public class OrderControllerTests
    {
        [Fact]
        public async Task AddOrder()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""orderId"":0, ""customer"":{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""},
                            ""status"": 0,""price"": 400, ""products"":{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}}");
            HttpResponseMessage response = await httpClient.PostAsync("api/Order", content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("1", responseString);
        }

        [Fact]
        public async Task GetOrderWithID()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync("api/Order/0");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal(@"{""orderId"":0, ""customer"":{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""},
                            ""status"": 0,""price"": 400, ""products"":{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}}", responseString);

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

        [Theory]
        [InlineData("api/Order/0")]
        [InlineData("api/Order/1")]
        public async Task ChangeOrder(string url)
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""orderId"":0, ""customer"":{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""},
                            ""status"": 4,""price"": 20000, ""products"":{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}}");
            HttpResponseMessage response = await httpClient.PutAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("1", responseString);
        }

        [Theory]
        [InlineData("api/Order/status-0")]
        [InlineData("api/Order/status-1")]
        public async Task ChangeOrderStatus(string url)
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent("4");
            HttpResponseMessage response = await httpClient.PatchAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("1", responseString);
        }

        [Theory]
        [InlineData("api/Order/0")]
        [InlineData("api/Order/1")]
        public async Task RemoveOrder(string url)
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            HttpResponseMessage response = await httpClient.DeleteAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("1", responseString);
        }
    }
}
