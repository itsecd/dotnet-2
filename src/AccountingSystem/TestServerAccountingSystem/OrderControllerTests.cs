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
    public class OrderControllerTests
    {
        [Test]
        public async Task AddOrder()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""orderId"":0, ""customer"":{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""},
                            ""status"": 0,""price"": 400, ""products"":{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}}");
            HttpResponseMessage response = await httpClient.PostAsync("api/Order/", content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("1", responseString);
        }

        [Test]
        public async Task GetOrderWithID()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync("api/Order/0");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(@"{""orderId"":0, ""customer"":{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""},
                            ""status"": 0,""price"": 400, ""products"":{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}}", responseString);

        }

        [Test]
        public async Task GetAllPrice()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync("api/Order/all-price");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("500", responseString);

        }

        [Test]
        public async Task ChangeOrder()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""orderId"":0, ""customer"":{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""},
                            ""status"": 4,""price"": 20000, ""products"":{""productId"":0,""name"":""IPhone"",""price"":""8000"",""date"":""2022-03-30""}}");
            HttpResponseMessage response = await httpClient.PutAsync("api/Order/0", content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("1", responseString);
        }

        [Test]
        public async Task ChangeOrderStatus()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent("4");
            HttpResponseMessage response = await httpClient.PutAsync("api /Order/0", content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("1", responseString);
        }

        [Test]
        public async Task RemoveOrder()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            HttpResponseMessage response = await httpClient.DeleteAsync("api/Order/0");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("1", responseString);
        }
    }
}
