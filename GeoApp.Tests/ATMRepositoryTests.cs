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
        public void InsertATM()
        {
            var atm = new ATM
            {
                Id = "testATM",
                BankName = "VTB",
                Balance = 5000,
                Coords = new Coords { X = 15.025, Y = 9.023 }
            };
            ATMRepository repository = new();
            Assert.Equal(atm, repository.InsertATM(atm));
            Assert.Null(repository.InsertATM(atm));
            repository.DeleteATMById("testATM");
        }

        [Fact]
        public void GetATMById()
        {
            var atm = new ATM
            {
                Id = "testATM",
                BankName = "VTB",
                Balance = 5000,
                Coords = new Coords { X = 15.025, Y = 9.023 }
            };
            ATMRepository repository = new();
            repository.InsertATM(atm);
            Assert.Equal(atm, repository.GetATMById("testATM"));
            repository.DeleteATMById("testATM");
            Assert.Null(repository.GetATMById("testATM"));
        }

        [Fact]
        public void DeleteATMById()
        {
            var atm = new ATM
            {
                Id = "testATM",
                BankName = "VTB",
                Balance = 5000,
                Coords = new Coords { X = 15.025, Y = 9.023 }
            };
            ATMRepository repository = new();
            repository.InsertATM(atm);
            Assert.Equal(atm, repository.DeleteATMById("testATM"));
            Assert.Null(repository.DeleteATMById("testATM"));
        }

        [Fact]
        public void ChangeBalanceById()
        {
            var atm = new ATM
            {
                Id = "testATM",
                BankName = "VTB",
                Balance = 5000,
                Coords = new Coords { X = 15.025, Y = 9.023 }
            };
            ATMRepository repository = new();
            repository.InsertATM(atm);
            atm.Balance = 3000;
            Assert.Equal(atm, repository.ChangeBalanceById("testATM", 3000));
            repository.DeleteATMById("testATM");
            Assert.Null(repository.ChangeBalanceById("testATM", 999));
        }

        [Fact]
        public void GetAllATMs()
        {
            var atm1 = new ATM
            {
                Id = "testATM1",
                BankName = "VTB",
                Balance = 5000,
                Coords = new Coords { X = 15.025, Y = 9.023 }
            };
            var atm2 = new ATM
            {
                Id = "testATM2",
                BankName = "VTB",
                Balance = 5000,
                Coords = new Coords { X = 15.025, Y = 9.023 }
            };
            var expectedList = new List<ATM>();
            ATMRepository repository = new();

            repository.InsertATM(atm1);
            expectedList.Add(atm1);

            repository.InsertATM(atm2);
            expectedList.Add(atm2);

            Assert.Equal(expectedList, repository.GetAllATMs());
            repository.DeleteATMById("testATM1");
            repository.DeleteATMById("testATM2");

            expectedList.Clear();
            Assert.Equal(expectedList, repository.GetAllATMs());
        }
    }
}
