using Kanban_board.Model;
using Kanban_board.Repositories;
using System.Collections.Generic;
using Xunit;

namespace Kanban_board.Tests
{
    public class StatusesRepositoryTests
    {
        [Fact]
        public void GetStatusById()
        {
            var status = new Status
            {
                Id = "1",
                Name = "В процессе",
                Description = "Задачи, которые уже кто-то начал выполнять, но они ещё не выполнены",
                Priority = "yellow",
            };
            StatusesRepository repository = new();
            repository.AddStatus(status);
            var returnedStatus = repository.GetStatusById("1");
            Assert.True(returnedStatus.Equals(status));
            Assert.Null(repository.GetStatusById("wrongId"));

            repository.DeleteStatus("1");
        }

        [Fact]
        public void GetStatuses()
        {
            var status1 = new Status
            {
                Id = "1",
                Name = "2",
                Description = "3",
                Priority = "4",
            };
            var status2 = new Status
            {
                Id = "5",
                Name = "6",
                Description = "7",
                Priority = "8",
            };
            var status3 = new Status
            {
                Id = "9",
                Name = "10",
                Description = "11",
                Priority = "12",
            };
            var addedStatuses = new List<Status>();
            addedStatuses.Add(status1);
            addedStatuses.Add(status2);
            addedStatuses.Add(status3);
            StatusesRepository repository = new();
            repository.AddStatus(status1);
            repository.AddStatus(status2);
            repository.AddStatus(status3);
            var returnedStatuses = repository.GetAllStatuses();
            for (var i = 0; i < addedStatuses.Count; i++)
            {
                Assert.True(returnedStatuses[i].Equals(addedStatuses[i]));
            }

            repository.DeleteStatus("1");
            repository.DeleteStatus("5");
            repository.DeleteStatus("9");
        }

        [Fact]
        public void EditStatus()
        {
            var status = new Status
            {
                Id = "1",
                Name = "2",
                Description = "3",
                Priority = "yellow",
            };
            StatusesRepository repository = new();
            repository.AddStatus(status);
            var returnedStatus = repository.EditStatus(new Status
            {
                Id = "1",
                Name = "2",
                Description = "3",
                Priority = "red",
            });
            status.Priority = "red";
            Assert.True(returnedStatus.Equals(returnedStatus));

            status.Id = "wrongId";
            Assert.Null(repository.EditStatus(status));

            repository.DeleteStatus("1");
        }

        [Fact]
        public void DeleteStatus()
        {
            var status = new Status
            {
                Id = "1",
                Name = "2",
                Description = "3",
                Priority = "4",
            };
            StatusesRepository repository = new();
            repository.AddStatus(status);
            var returnedStatus = repository.DeleteStatus(status.Id);

            Assert.True(returnedStatus.Equals(status));
            Assert.Null(repository.DeleteStatus("wrongId"));

            repository.DeleteStatus("1");
        }

        [Fact]
        public void AddStatus()
        {
            var status = new Status
            {
                Id = "1",
                Name = "2",
                Description = "3",
                Priority = "4",
            };
            StatusesRepository repository = new();
            var returnedStatus = repository.AddStatus(status);

            Assert.True(returnedStatus.Equals(status));
            Assert.Null(repository.AddStatus(status));

            repository.DeleteStatus("1");
        }
    }
}
