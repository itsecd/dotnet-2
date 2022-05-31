using PPTask.Model;
using PPTask.Repositories;
using System.Collections.Generic;
using Xunit;

namespace ServerTest
{
    public class TaskRepositoryTests
    {
        [Fact]
        public void AddTaskTest()
        {
            var task1 = new Task
            {
                HeaderText = "test1",
                TextDescription = "test11",
                ExecutorId = 1,
                TagsId = new List<int> { 1, 2 }
            };
            var repository = new JsonTaskRepository();

            repository.RemoveAllTasks();
            repository.AddTask(task1);

            Assert.Equal(1, repository.GetTasks()[0].TaskId);
            Assert.Equal("test1", repository.GetTasks()[0].HeaderText);
            Assert.Equal("test11", repository.GetTasks()[0].TextDescription);
            Assert.Equal(1, repository.GetTasks()[0].ExecutorId);
            Assert.Equal(1, repository.GetTasks()[0].TagsId[0]);
            Assert.Equal(2, repository.GetTasks()[0].TagsId[1]);

            repository.RemoveAllTasks();
        }

        [Fact]
        public void AddFullTaskTest()
        {
            var tagList = new List<int> { 1, 2 };
            var task1 = new Task(1, "test1", "test11", 1, tagList);
            var repository = new JsonTaskRepository();

            repository.RemoveAllTasks();
            repository.AddTask(task1);

            Assert.Equal(1, repository.GetTasks()[0].TaskId);
            Assert.Equal("test1", repository.GetTasks()[0].HeaderText);
            Assert.Equal("test11", repository.GetTasks()[0].TextDescription);
            Assert.Equal(1, repository.GetTasks()[0].ExecutorId);
            Assert.Equal(1, repository.GetTasks()[0].TagsId[0]);
            Assert.Equal(2, repository.GetTasks()[0].TagsId[1]);

            repository.RemoveAllTasks();
        }

        [Fact]
        public void RemoveTaskTest()
        {
            var tagList = new List<int> { 1, 2 };
            var task1 = new Task(1, "test1", "test11", 1, tagList);
            var repository = new JsonTaskRepository();

            repository.AddTask(task1);
            repository.RemoveTask(1);

            Assert.DoesNotContain(task1, repository.GetTasks());
        }

        [Fact]
        public void RemoveAllTasksTest()
        {
            var tagList = new List<int> { 1, 2 };
            var task1 = new Task(1, "test1", "test11", 1, tagList);
            var task2 = new Task(2, "test2", "test22", 2, tagList);
            var repository = new JsonTaskRepository();

            repository.AddTask(task1);
            repository.AddTask(task2);
            repository.RemoveAllTasks();

            Assert.DoesNotContain(task1, repository.GetTasks());
            Assert.DoesNotContain(task2, repository.GetTasks());
        }
    }
}
