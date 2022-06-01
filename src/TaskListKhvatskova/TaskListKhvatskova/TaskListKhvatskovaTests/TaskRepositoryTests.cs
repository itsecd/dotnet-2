using TaskListKhvatskova.Models;
using TaskListKhvatskova.Repositories;
using System.Collections.Generic;
using System;
using Xunit;

namespace TaskListKhvatskovaTests
{
    public class TaskRepositoryTests
    {
        private MyTask TaskCreate(int id)
        {
            var executor = new Executor
            {
                ExecutorId = 1,
                Name = "Sergey",
                Surname = "Sergeev"
            };
            var firstTagId = 1;

            MyTask task = new MyTask
            {
                TaskId = id,
                Name = "Daily work",
                Description = "for sale",
                TaskState = false,
                ExecutorId = executor.ExecutorId,
                TagsId = new List<int>() { firstTagId }
            };
            return task;
        }

        private MyTask TaskCreate()
        {
            var executor = new Executor
            {
                ExecutorId = 1,
                Name = "Sergey",
                Surname = "Sergeev"
            };
            var firstTagId = 1;

            MyTask task = new MyTask
            {
                Name = "Daily work",
                Description = "for sale",
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
