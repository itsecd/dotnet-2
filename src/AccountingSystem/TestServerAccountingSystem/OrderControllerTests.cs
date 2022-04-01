using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using AccountingSystem;
using Xunit;

namespace TestServerAccountingSystem
{
    public class OrderControllerTests
    {
        [Theory]
        [InlineData("api/Order")]
        public async Task AddOrder(string url)
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""orderId"":0, ""customer"":{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""},
                            ""status"": 0,""price"": 400, ""products"":{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}}");
            HttpResponseMessage response = await httpClient.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("1", responseString);
        }

        [Theory]
        [InlineData("api/Order/0")]
        public async Task GetOrderWithID(string url)
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal(@"{""orderId"":0, ""customer"":{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""},
                            ""status"": 0,""price"": 400, ""products"":{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}}", responseString);

        }

        [Theory]
        [InlineData("api/Order/all-price")]
        public async Task GetAllPrice(string url)
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync("api/Order/all-price");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("500", responseString);

        }

        [Theory]
        [InlineData("api/Order/0")]
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
        [InlineData("api/Order/0")]
        public async Task ChangeOrderStatus(string url)
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent("4");
            HttpResponseMessage response = await httpClient.PutAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("1", responseString);
        }

        [Theory]
        [InlineData("api/Order/0")]
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
