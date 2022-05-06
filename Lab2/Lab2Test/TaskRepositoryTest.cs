using Lab2.Models;
using Lab2.Repositories;
using System.Collections.Generic;
using Xunit;

namespace Lab2Test
{
    public class TaskRepositoryTest
    {
        private Task TaskCreate(int id)
        {
            var executor = new Executor
            {
                ExecutorId = 1,
                Name = "Valera",
                Surname = "Koshkin"
            };
            var firstTagId = 1;

            Task task = new Task
            {
                TaskId = id,
                Name = "Monday work",
                Description = "for sale to intermediaries",
                TaskState = false,
                ExecutorId = executor.ExecutorId,
                TagsId = new List<int>() { firstTagId }
            };
            return task;
        }

        [Fact]
        public void AddTaskTest()
        {
            TaskRepository repository = new();
            Assert.Equal(4, repository.AddTask(TaskCreate(3)));
            repository.RemoveTask(4);
        }
        [Fact]
        public void RemoveTaskTest()
        {
            TaskRepository repository = new();
            repository.AddTask(TaskCreate(1));
            Assert.Equal(1, repository.RemoveTask(1));
        }
        [Fact]
        public void UpdateTaskTest()
        {
            TaskRepository repository = new();
            repository.AddTask(TaskCreate(1));
            Assert.Equal(1, repository.UpdateTask(1, TaskCreate(1)));
            repository.RemoveTask(1);
        }

    }
}
