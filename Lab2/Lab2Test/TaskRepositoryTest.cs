using Xunit;
using Lab2.Repositories;
using Lab2.Models;
using System.Collections.Generic;

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
            Tags firstTag = new Tags
            {
                TagId = 1,
                Name = "for monday",
                Color = 3
            };


            Task task = new Task
            {
                TaskId = id,
                Name = "Monday work",
                Description = "for sale to intermediaries",
                TaskState = false,
                MemberOfTask = executor,
                TagsNames = new List<Tags>() { firstTag }
            };
            return task;
        }

        [Fact]
        public void AddTaskTest()
        {
            TaskRepository repository = new();
            Assert.Equal(3, repository.AddTask(TaskCreate(3)));
            repository.RemoveTask(3);
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
