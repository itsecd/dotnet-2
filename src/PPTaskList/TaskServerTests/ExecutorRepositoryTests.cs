using PPTask.Model;
using PPTask.Repositories;
using Xunit;

namespace ServerTest
{
    public class ExecutorRepositoryTests
    {
        [Fact]
        public void AddExecutorTest()
        {
            var executor1 = new Executor
            {
                Name = "test1"
            };
            var repository = new JsonExecutorRepository();

            repository.RemoveAllExecutors();
            repository.AddExecutor(executor1);

            Assert.Equal(1, repository.GetExecutors()[0].ExecutorId);
            Assert.Equal("test1", repository.GetExecutors()[0].Name);

            repository.RemoveAllExecutors();
        }

        [Fact]
        public void AddFullExecutorTest()
        {
            var executor1 = new Executor(1, "test1");
            var repository = new JsonExecutorRepository();

            repository.RemoveAllExecutors();
            repository.AddExecutor(executor1);

            Assert.Equal(1, repository.GetExecutors()[0].ExecutorId);
            Assert.Equal("test1", repository.GetExecutors()[0].Name);

            repository.RemoveAllExecutors();
        }

        [Fact]
        public void RemoveExecutorTest()
        {
            var executor1 = new Executor(1, "test1");
            var executor2 = new Executor(2, "test2");
            var repository = new JsonExecutorRepository();

            repository.AddExecutor(executor1);
            repository.AddExecutor(executor2);
            repository.RemoveExecutor(1);

            Assert.DoesNotContain(executor1, repository.GetExecutors());
        }

        [Fact]
        public void RemoveAllExecutorsTest()
        {
            var executor1 = new Executor(1, "test1");
            var executor2 = new Executor(2, "test2");
            var repository = new JsonExecutorRepository();

            repository.AddExecutor(executor1);
            repository.AddExecutor(executor2);
            repository.RemoveAllExecutors();

            Assert.DoesNotContain(executor1, repository.GetExecutors());
            Assert.DoesNotContain(executor2, repository.GetExecutors());
        }
    }
}
