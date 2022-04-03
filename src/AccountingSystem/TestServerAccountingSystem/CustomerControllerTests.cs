using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using AccountingSystem;
using Xunit;

namespace TestServerAccountingSystem
{
    public class CustomerControllerTests
    {
        [Fact]
        public async Task AddCustomer()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""}");
            HttpResponseMessage response = await httpClient.PostAsync("api/Customer", content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("0", responseString);
        }

        [Fact]
        public async Task GetCustomerWithID()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync("api/Customer/0");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal(@"{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""}", responseString);

        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task ChangeCustomer(int id)
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""customerId"":0,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""}");
            HttpResponseMessage response = await httpClient.PutAsync("api/Customer/" + id, content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal(id.ToString(), responseString);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task RemoveCustomer(int id)
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            HttpResponseMessage response = await httpClient.DeleteAsync("api/Customer/" + id);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal(id.ToString(), responseString);

        }
    }
}
