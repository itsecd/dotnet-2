using Lab2.Models;
using Lab2.Repositories;
using System;
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

        private Task TaskCreate()
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
                Name = "Monday work",
                Description = "for sale to intermediaries",
                TaskState = false,
                ExecutorId = executor.ExecutorId,
                TagsId = new List<int>() { firstTagId }
            };
            return task;
        }

        [Fact]
        public void AddTaskWithoutIdTest()
        {
            TaskRepository taskRepository = new();
            if (taskRepository.GetTasks().Count == 0)
            {
                TaskCreate().TaskId = 1;
                Assert.Equal(1, taskRepository.AddTask(TaskCreate()));
                taskRepository.RemoveTask(1);
            }
            else
            {
                int count = taskRepository.GetTasks().Count;
                Assert.Equal(count + 1, taskRepository.AddTask(TaskCreate()));
                taskRepository.RemoveTask(count + 1);
            }
        }

        [Fact]
        public void AddTaskTest()
        {
            TaskRepository taskRepository = new();
            if (taskRepository.GetTasks().FindIndex(t => t.TaskId == 17) == -1)
            {
                Assert.Equal(17, taskRepository.AddTask(TaskCreate(17)));
                taskRepository.RemoveTask(17);
            }
            else
            {
                throw new Exception("This ID already exists");
            }
        }

        [Fact]
        public void RemoveTaskTest()
        {
            TaskRepository repository = new();
            repository.AddTask(TaskCreate(5));
            Assert.Equal(5, repository.RemoveTask(5));
        }
    }
}
