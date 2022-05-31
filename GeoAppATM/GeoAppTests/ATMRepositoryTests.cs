using GeoAppATM.Model;
using GeoAppATM.Repository;
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
            AtmRepository repository = new();
            var returnedATM = repository.GetAtmByID("525794080");
            Assert.Equal(atm, returnedATM);
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
            AtmRepository repository = new();
            var returnedATM = repository.ChangeBalanceByID("646586471", 12345);
            Assert.Equal(atm, returnedATM);
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
            AtmRepository repository = new();
            var returnedAtm1 = repository.GetAtms()[2];
            Assert.Equal(atm1, returnedAtm1);
            var returnedAtm2 = repository.GetAtms()[4];
            Assert.Equal(atm2, returnedAtm2);
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
