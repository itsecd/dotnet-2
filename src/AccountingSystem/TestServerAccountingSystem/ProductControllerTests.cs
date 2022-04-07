using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using AccountingSystem;
using Xunit;
using System;
using System.Net;

namespace TestServerAccountingSystem
{
    public class ProductControllerTests
    {
        [Fact]
        public async Task AddProduct()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""productId"":44,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}");
            HttpResponseMessage firstResponse = await httpClient.PostAsync("api/Product", content);
            var responseString = await firstResponse.Content.ReadAsStringAsync();
            Assert.Equal("44", responseString);
            HttpResponseMessage secondResponse = await httpClient.PostAsync("api/Product", content);
            Assert.Equal(HttpStatusCode.InternalServerError, secondResponse.StatusCode);
            await httpClient.DeleteAsync("api/Product/44");
        }

        [Fact]
        public async Task GetProductWithID()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""productId"":55,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}");
            await httpClient.PostAsync("api/Product", content);
            HttpResponseMessage firstResponse = await httpClient.GetAsync("api/Product/55");
            var responseString = await firstResponse.Content.ReadAsStringAsync();
            Assert.Equal(@"{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}", responseString);
            HttpResponseMessage secondResponse = await httpClient.GetAsync("api/Product/66");
            Assert.Equal(HttpStatusCode.NotFound, secondResponse.StatusCode);
            await httpClient.DeleteAsync("api/Product/55");
        }

        [Fact]
        public async Task ChangeProduct()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            await httpClient.PostAsync("api/Product", new StringContent(@"{""productId"":77,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}"));
            var content = new StringContent(@"{""productId"":0,""name"":""Xiaomi"",""price"":""2000"",""date"":""2022-03-31""}");
            HttpResponseMessage firstResponse = await httpClient.PutAsync("api/Product/77", content);
            var responseString = await firstResponse.Content.ReadAsStringAsync();
            Assert.Equal("77", responseString);
            HttpResponseMessage secondResponse = await httpClient.PutAsync("api/Product/88", content);
            Assert.Equal(HttpStatusCode.NotFound, secondResponse.StatusCode);
            await httpClient.DeleteAsync("api/Product/77");
        }

        [Fact]
        public async Task RemoveProduct()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""productId"":99,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}");
            await httpClient.PostAsync("api/Product", content);
            HttpResponseMessage firstResponse = await httpClient.DeleteAsync("api/Product/99");
            var responseString = await firstResponse.Content.ReadAsStringAsync();
            Assert.Equal("99", responseString);
            HttpResponseMessage secondResponse = await httpClient.DeleteAsync("api/Product/110");
            Assert.Equal(HttpStatusCode.NotFound, secondResponse.StatusCode);
        }
    }
}
