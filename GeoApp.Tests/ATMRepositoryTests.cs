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
            var ATM = new ATM
            {
                Id = "testATM",
                BankName = "VTB",
                Balance = 5000,
                Coords = new Coords { x = 15.025, y = 9.023 }
            };
            ATMRepository repository = new();
            Assert.Equal(ATM, repository.InsertATM(ATM));
            Assert.Null(repository.InsertATM(ATM));
            repository.DeleteATMById("testATM");
        }

        [Fact]
        public void GetATMById()
        {
            var ATM = new ATM
            {
                Id = "testATM",
                BankName = "VTB",
                Balance = 5000,
                Coords = new Coords { x = 15.025, y = 9.023 }
            };
            ATMRepository repository = new();
            repository.InsertATM(ATM);
            Assert.Equal(ATM, repository.GetATMById("testATM"));
            repository.DeleteATMById("testATM");
            Assert.Null(repository.GetATMById("testATM"));
        }

        [Fact]
        public void DeleteATMById()
        {
            var ATM = new ATM
            {
                Id = "testATM",
                BankName = "VTB",
                Balance = 5000,
                Coords = new Coords { x = 15.025, y = 9.023 }
            };
            ATMRepository repository = new();
            repository.InsertATM(ATM);
            Assert.Equal(ATM, repository.DeleteATMById("testATM"));
            Assert.Null(repository.DeleteATMById("testATM"));
        }

        [Fact]
        public void ChangeBalanceById()
        {
            var ATM = new ATM
            {
                Id = "testATM",
                BankName = "VTB",
                Balance = 5000,
                Coords = new Coords { x = 15.025, y = 9.023 }
            };
            ATMRepository repository = new();
            repository.InsertATM(ATM);
            ATM.Balance = 3000;
            Assert.Equal(ATM, repository.ChangeBalanceById("testATM", 3000));
            repository.DeleteATMById("testATM");
            Assert.Null(repository.ChangeBalanceById("testATM", 999));
        }

        [Fact]
        public void GetAllATMs()
        {
            var ATM1 = new ATM
            {
                Id = "testATM1",
                BankName = "VTB",
                Balance = 5000,
                Coords = new Coords { x = 15.025, y = 9.023 }
            };
            var ATM2 = new ATM
            {
                Id = "testATM2",
                BankName = "VTB",
                Balance = 5000,
                Coords = new Coords { x = 15.025, y = 9.023 }
            };
            var expectedList = new List<ATM>();
            ATMRepository repository = new();

            repository.InsertATM(ATM1);
            expectedList.Add(ATM1);

            repository.InsertATM(ATM2);
            expectedList.Add(ATM2);

            Assert.Equal(expectedList, repository.GetAllATMs());
            repository.DeleteATMById("testATM1");
            repository.DeleteATMById("testATM2");

            expectedList.Clear();
            Assert.Equal(expectedList, repository.GetAllATMs());
        }
    }
}
