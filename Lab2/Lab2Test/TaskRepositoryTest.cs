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
            taskRepository.RemoveAllTasks();
            var taskId = taskRepository.AddTask(TaskCreate());
            Assert.Equal(1, taskId);
            taskRepository.RemoveTask(taskId);
        }

        [Fact]
        public void AddTaskTest()
        {
            TaskRepository taskRepository = new();
            Assert.Equal(18, taskRepository.AddTask(TaskCreate(18)));
            taskRepository.RemoveTask(18);
        }

        [Fact]
        public void RemoveTaskTest()
        {
            TaskRepository repository = new();
            repository.AddTask(TaskCreate(15));
            Assert.Equal(15, repository.RemoveTask(15));
        }

        [Fact]
        public void UpdateTaskTest()
        {
            TaskRepository repository = new();
            repository.AddTask(TaskCreate(300));
            Assert.Equal(300, repository.UpdateTask(300, TaskCreate(1)));
            repository.RemoveTask(300);
        }
    }
}
