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
            File.Delete(@"AccountingSystem.db3");
        }
    }
}
