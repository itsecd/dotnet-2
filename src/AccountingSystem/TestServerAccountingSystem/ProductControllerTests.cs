using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using AccountingSystem;
using Xunit;

namespace TestServerAccountingSystem
{
    public class ProductControllerTests
    {
        [Fact]
        public async Task AddProduct()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}");
            HttpResponseMessage response = await httpClient.PostAsync("api/Product", content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("0", responseString);
        }

        [Fact]
        public async Task GetProductWithID()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync("api/Product/0");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal(@"{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}", responseString);

        }

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        public async Task ChangeProduct(int id)
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""productId"":0,""name"":""Xiaomi"",""price"":""2000"",""date"":""2022-03-31""}");
            HttpResponseMessage response = await httpClient.PutAsync("api/Product/" + id, content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal(id.ToString(), responseString);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        public async Task RemoveProduct(int id)
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            HttpResponseMessage response = await httpClient.DeleteAsync("api/Product/" + id);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal(id.ToString(), responseString);

        }
    }
}
