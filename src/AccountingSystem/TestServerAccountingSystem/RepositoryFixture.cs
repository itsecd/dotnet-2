using AccountingSystem.Connection;
using System;
using System.IO;

namespace TestServerAccountingSystem
{
    public class RepositoryFixture : IDisposable
    {
        public RepositoryFixture()
        {
            NHibernateSession.GenerateSchema();
        }

        public void Dispose()
        {
            File.Delete(@"bin\Debug\net5.0\AccountingSystem.db3");
        }
    }
}
