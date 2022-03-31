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
    public class CustomerControllerTests
    {
        [Test]
        public async Task AddCustomer()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            //var model = new { customerId = 0, name = "ANTON", phone = "88005553535", address = "SAMARA" };
            //var content = new StringContent(JsonConvert.SerializeObject(model));
            //var jsonString = "{\"customerId\":1,\"name\":\"ANTON\",\"phone\":\"88005553535\",\"address\":\"SAMARA\"}";
            //var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var content = new StringContent(@"{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""}");
            HttpResponseMessage response = await httpClient.PostAsync("api /Customer/", content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("1", responseString);
        }

        [Test]
        public async Task GetCustomerWithID()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync("api/Customer/0");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(@"{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""}", responseString);

        }

        [Test]
        public async Task ChangeCustomer()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            //var model = new { customerId = 0, name = "BATON", phone = "8005553535", address = "SMR" };
            //var content = new StringContent(JsonConvert.SerializeObject(model));
            var content = new StringContent(@"{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""}");
            HttpResponseMessage response = await httpClient.PutAsync("api/Customer/0", content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("1", responseString);
        }

        [Test]
        public async Task RemoveCustomer()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            HttpResponseMessage response = await httpClient.DeleteAsync("api/Customer/0");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("1", responseString);

        }
    }
}
