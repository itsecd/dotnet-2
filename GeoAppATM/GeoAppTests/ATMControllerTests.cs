using GeoAppATM;
using GeoAppATM.Model;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GeoAppTests
{
    public class AtmControllerTests
    {
        [Fact]
        public async Task GetAtms()
        {
            var webHost = new WebApplicationFactory<Startup>();
            var httpClient = webHost.CreateClient();

            var response = await httpClient.GetAsync("api/Atm");
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.NotEmpty(JsonConvert.DeserializeObject<List<Atm>>(responseString));
        }
        [Fact]
        public async Task GetAtmById()
        {
            var atm = new Atm
            {
                Id = "879851245",
                Name = "Сбербанк",
                Latitude = 50.1565708,
                Longitude = 53.1977097,
                Balance = 753713
            };

            var webHost = new WebApplicationFactory<Startup>();
            var httpClient = webHost.CreateClient();

            var response = await httpClient.GetAsync("api/Atm/879851245");
            var responseString = await response.Content.ReadAsStringAsync();
            var returnedAtm = JsonConvert.DeserializeObject<Atm>(responseString);
            Assert.Equal(atm.Name, returnedAtm.Name);
            Assert.Equal(atm.Latitude, returnedAtm.Latitude);
            Assert.Equal(atm.Longitude, returnedAtm.Longitude);
            Assert.Equal(atm.Id, returnedAtm.Id);
            Assert.Equal(atm.Balance, returnedAtm.Balance);
            response = await httpClient.GetAsync("api/Atm/randomId");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ChangeAtmBalance()
        {
            var atm = new Atm
            {
                Id = "646586471",
                Name = "Сбербанк",
                Latitude = 50.1214707,
                Longitude = 53.1862179,
                Balance = 0
            };

            var webHost = new WebApplicationFactory<Startup>();
            var httpClient = webHost.CreateClient();

            var response = await httpClient.PutAsync("api/Atm/646586471", new StringContent(@"777", Encoding.UTF8, "application/json"));
            var responseString = await response.Content.ReadAsStringAsync();
            var returnedAtm = JsonConvert.DeserializeObject<Atm>(responseString);
            Assert.Equal(atm.Name, returnedAtm.Name);
            Assert.Equal(atm.Latitude, returnedAtm.Latitude);
            Assert.Equal(atm.Longitude, returnedAtm.Longitude);
            Assert.Equal(atm.Id, returnedAtm.Id);
            Assert.Equal(777, returnedAtm.Balance);
            await httpClient.PutAsync("api/ATM/646586471", new StringContent(@"0", Encoding.UTF8, "application/json"));
        }
    }
}
