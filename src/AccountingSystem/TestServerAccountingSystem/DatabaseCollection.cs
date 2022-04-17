using Xunit;

namespace TestServerAccountingSystem
{
    [CollectionDefinition("Accounting System")]
    public class DatabaseCollection : ICollectionFixture<RepositoryFixture>
    {
    }
}
