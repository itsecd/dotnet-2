using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using AccountingSystem;
using Xunit;

namespace TestServerAccountingSystem
{
    public class CustomerControllerTests
    {
        [Theory]
        [InlineData("api/Customer/")]
        public async Task AddCustomer(string url)
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""}");
            HttpResponseMessage response = await httpClient.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("1", responseString);
        }

        [Theory]
        [InlineData("api/Customer/0")]
        public async Task GetCustomerWithID(string url)
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal(@"{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""}", responseString);

        }

        [Theory]
        [InlineData("api/Customer/0")]
        public async Task ChangeCustomer(string url)
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""}");
            HttpResponseMessage response = await httpClient.PutAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("1", responseString);
        }

        [Theory]
        [InlineData("api/Customer/0")]
        public async Task RemoveCustomer(string url)
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            HttpResponseMessage response = await httpClient.DeleteAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("1", responseString);

        }
    }
}
