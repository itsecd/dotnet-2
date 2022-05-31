using GeoAppATM.Repository;
using GeoAppATM.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GeoAppTests
{
    public class ATMRepositoryTests
    {
        [Fact]
        public void GetATMByID()
        {
            var atm = new GeoJsonATM
            {
                Geometry = new Geometry()
                {
                    Coordinates = new List<double> { 50.2310375, 53.2394015 }
                },
                Properties = new Properties()
                {
                    Id = "1901330894",
                    Operator = "Сбербанк",
                    Balance = 0
                }
            };
            AtmRepository repository = new();
            var returnedATM = repository.GetATMByID("1901330894");
            Assert.Equal(atm, returnedATM);
            Assert.Null(repository.GetATMByID("randomId"));
        }

        [Fact]
        public void ChangeBalanceByID()
        {
            var atm = new GeoJsonATM
            {
                Geometry = new Geometry()
                {
                    Coordinates = new List<double> { 50.1179917, 53.1866776 }
                },
                Properties = new Properties()
                {
                    Id = "904585003",
                    Operator = "Связь-банк",
                    Balance = 250000,
                }
            };
            AtmRepository repository = new();
            var returnedATM = repository.ChangeBalanceByID("904585003", 250000);
            Assert.Equal(atm, returnedATM);
            Assert.Null(repository.ChangeBalanceByID("randomId", 0));

            repository.ChangeBalanceByID("646586471", 0);
        }

        [Fact]
        public void GetAllATMs()
        {
            var atm1 = new GeoJsonATM
            {
                Geometry = new Geometry()
                {
                    Coordinates = new List<double> { 50.141252, 53.201402 }
                },
                Properties = new Properties()
                {
                    Id = "1133505644",
                    Operator = "Юниаструм",
                    Balance = 0,
                }
            };
            var atm2 = new GeoJsonATM
            {
                Geometry = new Geometry()
                {
                    Coordinates = new List<double> { 50.2310375, 53.2394015 }
                },
                Properties = new Properties()
                {
                    Id = "1901330894",
                    Operator = "Сбербанк",
                    Balance = 0,
                }
            };
            AtmRepository repository = new();

            Assert.True(repository.GetAllATM()[7].Equals(atm1));
            Assert.True(repository.GetAllATM()[8].Equals(atm2));

            Assert.Equal(91, repository.GetAllATM().Count);
        }

        [Fact]
        public void ChangeBalance()
        {
            AtmRepository repository = new();
            var atms = repository.GetAllATM();

            var tasks = atms.Select(atm => Task.Run(() =>
            {
                repository.ChangeBalanceByID(atm.Properties.Id, 100);
            })).ToArray();
            Task.WaitAll(tasks);

            tasks = atms.Select(atm => Task.Run(() =>
            {
                repository.ChangeBalanceByID(atm.Properties.Id, 0);
            })).ToArray();
            Task.WaitAll(tasks);
        }
    }
}
