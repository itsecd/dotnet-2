
using Lab2.Models;
using Lab2.Repositories;
using Xunit;


namespace Lab2Test
{
    public class ExecutorRepositoryTest
    {
        [Fact]
        public void AddExecutorTest()
        {
            var executor = new Executor
            {
                ExecutorId = 10,
                Name = "Pavel",
                Surname = "Pavlov"
            };
            ExecutorRepository executorRepository = new();
            Assert.Equal(10, executorRepository.AddExecutor(executor));
            executorRepository.RemoveExecutor(10);
        }
        [Fact]
        public void RemoveExecutorTest()
        {
            var executor = new Executor
            {
                ExecutorId = 4,
                Name = "Pavel",
                Surname = "Pavlov"
            };
            ExecutorRepository repository = new();
            repository.AddExecutor(executor);
            Assert.Equal(4, repository.RemoveExecutor(4));
        }
        [Fact]
        public void UpdateExecutorTest()
        {
            var executor = new Executor
            {
                ExecutorId = 5,
                Name = "Sergey",
                Surname = "Sergeev"
            };

            ExecutorRepository repository = new();
            repository.AddExecutor(executor);
            Assert.Equal(5, repository.UpdateExecutor(5, executor));
            repository.RemoveExecutor(5);
        }
    }
}
