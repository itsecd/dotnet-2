using GeoApp.Repository;
using GeoApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GeoApp.Tests
{
    public class ATMRepositoryTests
    {
        [Fact]
        public void GetATMById()
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
                    Balance = 0
                }
            };
            ATMRepository repository = new();
            var returnedATM = repository.GetATMById("646586471");
            Assert.True(returnedATM.Equals(atm));
            Assert.Null(repository.GetATMById("randomId"));
        }

        [Fact]
        public void ChangeBalanceById()
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
                    Balance = 3000,
                }
            };
            ATMRepository repository = new();
            var returnedATM = repository.ChangeBalanceById("646586471", 3000);
            Assert.True(returnedATM.Equals(atm));
            Assert.Null(repository.ChangeBalanceById("randomId", 0));

            repository.ChangeBalanceById("646586471", 0);
        }

        [Fact]
        public void GetAllATMs()
        {
            var atm1 = new JsonATM
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
            var atm2 = new JsonATM
            {
                Geometry = new Geometry()
                {
                    Coordinates = new List<double> { 50.1178883, 53.1874092 }
                },
                Properties = new Properties()
                {
                    Id = "904585000",
                    Operator = "ТрансКредитБанк",
                    Balance = 0,
                }
            };
            ATMRepository repository = new();

            Assert.True(repository.GetAllATMs()[1].Equals(atm1));
            Assert.True(repository.GetAllATMs()[4].Equals(atm2));

            Assert.Equal(91, repository.GetAllATMs().Count);
        }

        [Fact]
        public void ChangeBalance()
        {
            ATMRepository repository = new();
            var atms = repository.GetAllATMs();

            var tasks = atms.Select(atm => Task.Run(() =>
            {
                repository.ChangeBalanceById(atm.Properties.Id, 100);
            })).ToArray();
            Task.WaitAll(tasks);

            tasks = atms.Select(atm => Task.Run(() =>
            {
                repository.ChangeBalanceById(atm.Properties.Id, 0);
            })).ToArray();
            Task.WaitAll(tasks);
        }
    }
}
