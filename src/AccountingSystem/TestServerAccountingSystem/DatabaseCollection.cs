using Xunit;

namespace TestServerAccountingSystem
{
    [Collection("Accounting System")]
    internal class DatabaseCollection : ICollectionFixture<RepositoryFixture>
    {
    }
}
