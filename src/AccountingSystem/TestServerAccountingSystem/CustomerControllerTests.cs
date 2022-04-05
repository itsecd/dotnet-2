using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using AccountingSystem;
using Xunit;
using System;

namespace TestServerAccountingSystem
{
    public class CustomerControllerFixture : IDisposable
    {

        public readonly HttpClient httpClient = new WebApplicationFactory<Startup>().CreateClient();

        public CustomerControllerFixture()
        {
            var content = new StringContent(@"{""customerId"":44,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""}");
            httpClient.PostAsync("api/Customer", content);
        }

        public void Dispose()
        {
            httpClient.DeleteAsync("api/Customer/44");
        }
    }
    public class CustomerControllerTests : IClassFixture<CustomerControllerFixture>
    {

        [Fact]
        public async Task GetCustomerWithID()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync("api/Customer/44");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal(@"{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""}", responseString);

        }

        [Fact]
        public async Task ChangeCustomer()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""}");
            HttpResponseMessage response = await httpClient.PutAsync("api/Customer/44", content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("44", responseString);
        }

    }
}
