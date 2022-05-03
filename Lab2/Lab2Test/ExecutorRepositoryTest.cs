
using Xunit;
using Lab2.Repositories;
using Lab2.Models;


namespace Lab2Test
{
    public class ExecutorRepositoryTest
    {
        [Fact]
        public void AddExecutorTest()
        {
            var executor = new Executor
            {
                ExecutorId = 3,
                Name = "Pavel",
                Surname = "Pavlov"
            };
            var executorRepository = new ExecutorRepository();
            Assert.Equal(3, executorRepository.AddExecutor(executor));
            executorRepository.RemoveExecutor(3);
        }
        [Fact]
        public void RemoveExecutorTest()
        {
            var executor = new Executor
            {
                ExecutorId = 8,
                Name = "Pavel",
                Surname = "Pavlov"
            };
            ExecutorRepository repository = new();
            repository.AddExecutor(executor);
            Assert.Equal(8, repository.RemoveExecutor(8));
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
