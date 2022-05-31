using GeoAppATM;
using GeoAppATM.Model;
using GeoAppATM.Repository;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace GeoAppTests
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

            AtmRepository repository = new();
            Assert.Equal(repository.GetAllATM(), JsonSerializer.Deserialize<List<GeoJsonATM>>(responseString));
        }
        [Fact]
        public async Task GetATMById()
        {
            var atm = new GeoJsonATM
            {
                Geometry = new Geometry()
                {
                    Coordinates = new List<double> { 50.1487157, 53.2169159 }
                },
                Properties = new Properties()
                {
                    Id = "2213571183",
                    Operator = "Тинькофф Банк",
                    Balance = 0,
                }
            };

            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();

            HttpResponseMessage response = await httpClient.GetAsync("api/ATM/2213571183");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.True(JsonSerializer.Deserialize<GeoJsonATM>(responseString).Equals(atm));
            response = await httpClient.GetAsync("api/ATM/randomId");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ChangeATMBalance()
        {
            var atm = new GeoJsonATM
            {
                Geometry = new Geometry()
                {
                    Coordinates = new List<double> { 50.1680796, 53.1996886 }
                },
                Properties = new Properties()
                {
                    Id = "525794080",
                    Operator = "Сбербанк",
                    Balance = 123456,
                }
            };

            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>();
            HttpClient httpClient = webHost.CreateClient();

            HttpResponseMessage response = await httpClient.PutAsync("api/ATM/525794080", new StringContent(@"123456", Encoding.UTF8, "application/json"));
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.True(JsonSerializer.Deserialize<GeoJsonATM>(responseString).Equals(atm));

            response = await httpClient.PutAsync("api/ATM/randomId", new StringContent(@"123456", Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            await httpClient.PutAsync("api/ATM/525794080", new StringContent(@"0", Encoding.UTF8, "application/json"));
        }
    }
}
