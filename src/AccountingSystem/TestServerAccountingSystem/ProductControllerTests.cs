using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using AccountingSystem;
using Xunit;
using System;

namespace TestServerAccountingSystem
{
    public class ProductControllerFixture : IDisposable
    {


        public readonly HttpClient httpClient = new WebApplicationFactory<Startup>().CreateClient();

        public ProductControllerFixture()
        {
            AddProduct();            
        }

        public async Task AddProduct()
        {
            var content = new StringContent(@"{""productId"":25,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}");
            HttpResponseMessage response = await httpClient.PostAsync("api/Product", content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("25", responseString);

        }

        public void Dispose()
        {
            DeleteOrder();
        }

        public async Task DeleteOrder()
        {
            HttpResponseMessage response = await httpClient.DeleteAsync("api/Product/25");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("25", responseString);
        }
    }

    public class ProductControllerTests : IClassFixture<ProductControllerFixture>
    {

        [Fact]
        public async Task GetProductWithID()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync("api/Product/25");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal(@"{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}", responseString);

        }

        [Fact]
        public async Task ChangeProduct()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""productId"":0,""name"":""Xiaomi"",""price"":""2000"",""date"":""2022-03-31""}");
            HttpResponseMessage response = await httpClient.PutAsync("api/Product/25", content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("25", responseString);
        }

        [Fact]
        public async Task RemoveProduct()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            HttpResponseMessage response = await httpClient.DeleteAsync("api/Product/25");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("25", responseString);

        }
    }
}
