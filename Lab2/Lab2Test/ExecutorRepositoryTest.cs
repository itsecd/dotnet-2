
using Lab2.Models;
using Lab2.Repositories;
using System;
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
            if (executorRepository.GetExecutors().Count == 0)
            {
                executor.ExecutorId = 1;
                Assert.Equal(1, executorRepository.AddExecutor(executor));
                executorRepository.RemoveExecutor(1);
            }
            else
            {
                int count = executorRepository.GetExecutors().Count;
                Assert.Equal(count + 1, executorRepository.AddExecutor(executor));
                executorRepository.RemoveExecutor(count + 1);
            }
        }

        [Fact]
        public void AddExecutorTest()
        {
            var executor = new Executor
            {
                ExecutorId = 99,
                Name = "Pavel",
                Surname = "Pavlov"
            };
            ExecutorRepository executorRepository = new();
            if (executorRepository.GetExecutors().FindIndex(t => t.ExecutorId == 99) == -1)
            {
                Assert.Equal(99, executorRepository.AddExecutor(executor));
                executorRepository.RemoveExecutor(99);
            }
            else
            {
                throw new Exception("This ID already exists");
            }
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
    }
}
