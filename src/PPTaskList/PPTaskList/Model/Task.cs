using PPTask.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace PPTask.Controllers.Model
{
    public class Task
    {
        public int TaskId { get; set; }

        public string HeaderText { get; set; }

        public string TextDescription { get; set; }

        public Executor Executor { get; set; }

        public int ExecutorId { get; set; }

        public List <Tags> Tags { get; set; }

        public Task()
        {
            HeaderText = string.Empty;
            TextDescription = string.Empty;
        }

        public Task(string header, string text, int id)
        {
            HeaderText = header;
            TextDescription = text;
            ExecutorId = id;

            //var executorRepository = JsonExecutorRepository();

            //Executor = executorRepository.GetExecutors().Result.Where(executor => executor.ExecutorId == id).Single();
        }
    }
}
