
using Lab2.Models;
using Lab2.Repositories;
using Xunit;


namespace Lab2Test
{
    public class ExecutorRepositoryTest
    {
        [Fact]
        public void AddExecutorWithoutIdTest()
        {
            ExecutorRepository executorRepository = new();
            var executor = new Executor
            {
                Name = "Pavel",
                Surname = "Pavlov"
            };
            executorRepository.RemoveAllExecutors();
            var executorId = executorRepository.AddExecutor(executor);
            Assert.Equal(1, executorId);
            executorRepository.RemoveExecutor(executorId);
        }

        [Fact]
        public void AddExecutorTest()
        {
            var executor = new Executor
            {
                ExecutorId = 111,
                Name = "Pavel",
                Surname = "Pavlov"
            };
            ExecutorRepository executorRepository = new();
            Assert.Equal(111, executorRepository.AddExecutor(executor));
            executorRepository.RemoveExecutor(111);
        }

        [Fact]
        public void RemoveExecutorTest()
        {
            var executor = new Executor
            {
                ExecutorId = 17,
                Name = "Pavel",
                Surname = "Pavlov"
            };
            ExecutorRepository repository = new();
            repository.AddExecutor(executor);
            Assert.Equal(17, repository.RemoveExecutor(17));
        }

        [Fact]
        public void UpdateExecutorTest()
        {
            var executor = new Executor
            {
                ExecutorId = 88,
                Name = "Sergey",
                Surname = "Sergeev"
            };

            ExecutorRepository repository = new();
            repository.AddExecutor(executor);
            Assert.Equal(88, repository.UpdateExecutor(88, executor));
            repository.RemoveExecutor(88);
        }
    }
}
