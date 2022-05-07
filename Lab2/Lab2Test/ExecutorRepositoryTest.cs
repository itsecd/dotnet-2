
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
                ExecutorId = 4,
                Name = "Pavel",
                Surname = "Pavlov"
            };
            ExecutorRepository executorRepository = new();
            Assert.Equal(4, executorRepository.AddExecutor(executor));
            executorRepository.RemoveExecutor(4);
        }

        [Fact]
        public void RemoveExecutorTest()
        {
            var executor = new Executor
            {
                ExecutorId = 3,
                Name = "Pavel",
                Surname = "Pavlov"
            };
            ExecutorRepository repository = new();
            repository.AddExecutor(executor);
            Assert.Equal(3, repository.RemoveExecutor(3));
        }

        [Fact]
        public void UpdateExecutorTest()
        {
            var executor = new Executor
            {
                ExecutorId = 9,
                Name = "Sergey",
                Surname = "Sergeev"
            };

            ExecutorRepository repository = new();
            repository.AddExecutor(executor);
            Assert.Equal(9, repository.UpdateExecutor(9, executor));
            repository.RemoveExecutor(9);
        }
    }
}
