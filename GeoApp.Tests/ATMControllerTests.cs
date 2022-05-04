using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GeoApp.Tests
{
    public class ATMControllerTests
    {
        [Fact]
        public async Task GetATMs()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""id"":""testATM1"",""bankName"":""Alfa-bank1"",""balance"":123,""coords"":{""x"":1.2,""y"":1.3}}", Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/ATM", content);
            content = new StringContent(@"{""id"":""testATM2"",""bankName"":""Alfa-bank2"",""balance"":1234,""coords"":{""x"":1.4,""y"":1.5}}", Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/ATM", content);
            content = new StringContent(@"{""id"":""testATM3"",""bankName"":""Alfa-bank3"",""balance"":12345,""coords"":{""x"":1.6,""y"":1.7}}", Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/ATM", content);

            HttpResponseMessage response = await httpClient.GetAsync("api/ATM");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal(@"[{""id"":""testATM1"",""bankName"":""Alfa-bank1"",""balance"":123,""coords"":{""x"":1.2,""y"":1.3}},{""id"":""testATM2"",""bankName"":""Alfa-bank2"",""balance"":1234,""coords"":{""x"":1.4,""y"":1.5}},{""id"":""testATM3"",""bankName"":""Alfa-bank3"",""balance"":12345,""coords"":{""x"":1.6,""y"":1.7}}]", responseString);

            await httpClient.DeleteAsync("api/ATM/testATM1");
            await httpClient.DeleteAsync("api/ATM/testATM2");
            await httpClient.DeleteAsync("api/ATM/testATM3");
        }

        [Fact]
        public async Task GetATMById()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""id"":""testATM"",""bankName"":""Alfa-bank5"",""balance"":44444,""coords"":{""x"":1.234,""y"":1.456}}", Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/ATM", content);
            HttpResponseMessage response = await httpClient.GetAsync("api/ATM/testATM");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal(@"{""id"":""testATM"",""bankName"":""Alfa-bank5"",""balance"":44444,""coords"":{""x"":1.234,""y"":1.456}}", responseString);
            response = await httpClient.GetAsync("api/ATM/asdsadsa");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            await httpClient.DeleteAsync("api/ATM/testATM");
        }

        [Fact]
        public async Task PostATM()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""id"":""testATM"",""bankName"":""Alfa-bank5"",""balance"":44444,""coords"":{""x"":1.234,""y"":1.456}}", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync("api/ATM", content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal(@"{""id"":""testATM"",""bankName"":""Alfa-bank5"",""balance"":44444,""coords"":{""x"":1.234,""y"":1.456}}", responseString);
            
            response = await httpClient.PostAsync("api/ATM", content); // already exists
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            await httpClient.DeleteAsync("api/ATM/testATM");
        }

        [Fact]
        public async Task ChangeATMBalance()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""id"":""testATM"",""bankName"":""Alfa-bank5"",""balance"":44444,""coords"":{""x"":1.234,""y"":1.456}}", Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/ATM", content);

            HttpResponseMessage response = await httpClient.PutAsync("api/ATM/testATM", new StringContent(@"555", Encoding.UTF8, "application/json"));
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal(@"{""id"":""testATM"",""bankName"":""Alfa-bank5"",""balance"":555,""coords"":{""x"":1.234,""y"":1.456}}", responseString);


            response = await httpClient.PutAsync("api/ATM/testATM23232", new StringContent(@"555", Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            await httpClient.DeleteAsync("api/ATM/testATM");
        }

        [Fact]
        public async Task DeleteATM()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            var content = new StringContent(@"{""id"":""testATM"",""bankName"":""Alfa-bank1"",""balance"":123,""coords"":{""x"":1.2,""y"":1.3}}", Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/ATM", content);

            HttpResponseMessage response = await httpClient.DeleteAsync("api/ATM/testATM");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal(@"{""id"":""testATM"",""bankName"":""Alfa-bank1"",""balance"":123,""coords"":{""x"":1.2,""y"":1.3}}", responseString);

            response = await httpClient.DeleteAsync("api/ATM/testATM");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
