using AccountingSystem.Connection;

namespace TestServerAccountingSystem
{
    public class RepositoryFixture
    {
        public RepositoryFixture()
        {
            NHibernateSession.GenerateSchema();
        }
    }
}
