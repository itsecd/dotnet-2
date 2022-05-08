using GeoApp.Model;
using GeoApp.Repository;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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

            HttpResponseMessage response = await httpClient.GetAsync("api/ATM");
            var responseString = await response.Content.ReadAsStringAsync();

            ATMRepository repository = new();
            Assert.Equal(repository.GetAllATMs(), JsonSerializer.Deserialize<List<JsonATM>>(responseString));
        }

        [Fact]
        public async Task GetATMById()
        {
            var atm = new JsonATM
            {
                Geometry = new Geometry()
                {
                    Coordinates = new List<double> { 50.1214707, 53.1862179 }
                },
                Properties = new Properties()
                {
                    Id = "646586471",
                    Operator = "Сбербанк",
                    Balance = 0,
                }
            };

            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();
            
            HttpResponseMessage response = await httpClient.GetAsync("api/ATM/646586471");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.True(JsonSerializer.Deserialize<JsonATM>(responseString).Equals(atm));
            
            response = await httpClient.GetAsync("api/ATM/randomId");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ChangeATMBalance()
        {
            var atm = new JsonATM
            {
                Geometry = new Geometry()
                {
                    Coordinates = new List<double> { 50.1214707, 53.1862179 }
                },
                Properties = new Properties()
                {
                    Id = "646586471",
                    Operator = "Сбербанк",
                    Balance = 555,
                }
            };

            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();

            HttpResponseMessage response = await httpClient.PutAsync("api/ATM/646586471", new StringContent(@"555", Encoding.UTF8, "application/json"));
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.True(JsonSerializer.Deserialize<JsonATM>(responseString).Equals(atm));

            response = await httpClient.PutAsync("api/ATM/randomId", new StringContent(@"555", Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            await httpClient.PutAsync("api/ATM/646586471", new StringContent(@"0", Encoding.UTF8, "application/json"));
        }
    }
}
