using Xunit;

namespace TestServerAccountingSystem
{
    [CollectionDefinition("Accounting System")]
    internal class DatabaseCollection : ICollectionFixture<RepositoryFixture>
    {
    }
}
