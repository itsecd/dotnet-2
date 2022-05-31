using GeoAppATM.Model;
using GeoAppATM.Repository;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GeoAppTests
{
    public class AtmRepositoryTests
    {
        [Fact]
        public void GetAtmByID()
        {
            var atm = new Atm
            {
                Id = "525794080",
                Name = "Сбербанк",
                Latitude = 50.1680796,
                Longitude = 53.1996886,
                Balance = 250000
            };
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json").Build();
            AtmRepository repository = new(config);
            var returnedAtm = repository.GetAtmByID("525794080");
            Assert.Equal(atm.Name, returnedAtm.Name);
            Assert.Equal(atm.Latitude, returnedAtm.Latitude);
            Assert.Equal(atm.Longitude, returnedAtm.Longitude);
            Assert.Equal(atm.Id, returnedAtm.Id);
            Assert.Equal(atm.Balance, returnedAtm.Balance);
            Assert.Null(repository.GetAtmByID("randomId"));
        }

        [Fact]
        public void ChangeBalanceByID()
        {
            var atm = new Atm
            {
                Id = "646586471",
                Name = "Сбербанк",
                Latitude = 50.1214707,
                Longitude = 53.1862179,
                Balance = 0
            };
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json").Build();
            AtmRepository repository = new(config);
            var returnedAtm = repository.ChangeBalanceByID("646586471", 12345);
            Assert.Equal(atm.Name, returnedAtm.Name);
            Assert.Equal(atm.Latitude, returnedAtm.Latitude);
            Assert.Equal(atm.Longitude, returnedAtm.Longitude);
            Assert.Equal(atm.Id, returnedAtm.Id);
            Assert.Equal(12345, returnedAtm.Balance);
            Assert.Null(repository.ChangeBalanceByID("randomId", 0));
            repository.ChangeBalanceByID("646586471", 0);
        }

        [Fact]
        public void GetAtms()
        {
            var atm1 = new Atm
            {
                Id = "879851245",
                Name = "Сбербанк",
                Latitude = 50.1565708,
                Longitude = 53.1977097,
                Balance = 753713
            };
            var atm2 = new Atm
            {
                Id = "904585000",
                Name = "ТрансКредитБанк",
                Latitude = 50.1178883,
                Longitude = 53.1874092,
                Balance = 18750
            };
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json").Build();
            AtmRepository repository = new(config);
            var atms = repository.GetAtms();
            var returnedAtm1 = atms[2];
            Assert.Equal(atm1.Name, returnedAtm1.Name);
            Assert.Equal(atm1.Latitude, returnedAtm1.Latitude);
            Assert.Equal(atm1.Longitude, returnedAtm1.Longitude);
            Assert.Equal(atm1.Id, returnedAtm1.Id);
            Assert.Equal(atm1.Balance, returnedAtm1.Balance);
            var returnedAtm2 = atms[4];
            Assert.Equal(atm2.Name, returnedAtm2.Name);
            Assert.Equal(atm2.Latitude, returnedAtm2.Latitude);
            Assert.Equal(atm2.Longitude, returnedAtm2.Longitude);
            Assert.Equal(atm2.Id, returnedAtm2.Id);
            Assert.Equal(atm2.Balance, returnedAtm2.Balance);
            Assert.Equal(91, repository.GetAtms().Count);
        }

        [Fact]
        public void ChangeBalance()
        {
            AtmRepository repository = new();
            var atms = repository.GetAtms();

            var tasks = atms.Select(atm => Task.Run(() =>
            {
                repository.ChangeBalanceByID(atm.Id, 100);
            })).ToArray();
            Task.WaitAll(tasks);

            tasks = atms.Select(atm => Task.Run(() =>
            {
                repository.ChangeBalanceByID(atm.Id, 0);
            })).ToArray();
            Task.WaitAll(tasks);
        }
    }
}
