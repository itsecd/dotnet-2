using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using AccountingSystem;
using Xunit;
using System.Net;
using System.Text;

namespace TestServerAccountingSystem
{
    public class CustomerControllerTests
    {

        [Fact]
        public async Task AddCustomer()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""customerId"":44,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""}", Encoding.UTF8, "application/json");
            HttpResponseMessage firstResponse = await httpClient.PostAsync("api/Customer", content);
            var responseString = await firstResponse.Content.ReadAsStringAsync();
            Assert.Equal("44", responseString);
            HttpResponseMessage secondResponse = await httpClient.PostAsync("api/Customer", content);
            Assert.Equal(HttpStatusCode.InternalServerError, secondResponse.StatusCode);
            await httpClient.DeleteAsync("api/Customer/44");

        }

        [Fact]
        public async Task GetCustomerWithID()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""customerId"":55,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""}", Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/Customer", content);
            HttpResponseMessage firstResponse = await httpClient.GetAsync("api/Customer/55");
            var responseString = await firstResponse.Content.ReadAsStringAsync();
            Assert.Equal(@"{""customerId"":55,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""}", responseString);
            HttpResponseMessage secondResponse = await httpClient.GetAsync("api/Customer/66");
            Assert.Equal(HttpStatusCode.NotFound, secondResponse.StatusCode);
            await httpClient.DeleteAsync("api/Customer/55");
        }

        [Fact]
        public async Task ChangeCustomer()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            await httpClient.PostAsync("api/Customer", new StringContent(@"{""customerId"":77,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""}", Encoding.UTF8, "application/json"));
            var content = new StringContent(@"{""customerId"":0,""name"":""ANT"",""phone"":""88005553"",""address"":""SMR""}", Encoding.UTF8, "application/json");
            HttpResponseMessage firstResponse = await httpClient.PutAsync("api/Customer/77", content);
            var responseString = await firstResponse.Content.ReadAsStringAsync();
            Assert.Equal("77", responseString);
            HttpResponseMessage secondResponse = await httpClient.PutAsync("api/Customer/88", content);
            Assert.Equal(HttpStatusCode.NotFound, secondResponse.StatusCode);
            await httpClient.DeleteAsync("api/Customer/77");
        }
        [Fact]
        public async Task DeleteCustomer()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            await httpClient.PostAsync("api/Customer", new StringContent(@"{""customerId"":99,""name"":""ANTON"",""phone"":""88005553535"",""address"":""SAMARA""}", Encoding.UTF8, "application/json"));
            HttpResponseMessage response = await httpClient.DeleteAsync("api/Customer/99");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("99", responseString);
            HttpResponseMessage secondResponse = await httpClient.DeleteAsync("api/Customer/110");
            Assert.Equal(HttpStatusCode.NotFound, secondResponse.StatusCode);
        }

    }
}
