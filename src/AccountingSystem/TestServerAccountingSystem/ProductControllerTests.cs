using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AccountingSystem;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Text.Json;
using Newtonsoft.Json;
using System.Text;

namespace TestServerAccountingSystem
{
    [TestFixture]
    public class ProductControllerTests
    {
        [Test]
        public async Task AddProduct()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}");
            HttpResponseMessage response = await httpClient.PostAsync("api/Product/", content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("1", responseString);
        }

        [Test]
        public async Task GetProductWithID()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync("api/Product/0");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(@"{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}", responseString);

        }

        [Test]
        public async Task ChangeProduct()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""productId"":0,""name"":""Xiaomi"",""price"":""2000"",""date"":""2022-03-31""}");
            HttpResponseMessage response = await httpClient.PutAsync("api/Product/0", content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("1", responseString);
        }

        [Test]
        public async Task RemoveProduct()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            HttpResponseMessage response = await httpClient.DeleteAsync("api/Product/0");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("1", responseString);

        }
    }
}
