using System.Collections.Generic;

namespace PPTask.Controllers.Model
{
    public class Task
    {
        public int TaskId { get; set; }

        public string HeaderText { get; set; }

        public string TextDescription { get; set; }

        public Executor Executor { get; set; }

        public List <Tags> Tags { get; set; }

        private static readonly List<string> _statuses = new List<string>
        {
            "Done", "Not ready yet"
        };
        public Task()
        {
            HeaderText = string.Empty;
            TextDescription = string.Empty;
        }

        public Task(string header, string text)
        {
            HeaderText = header;
            TextDescription = text;
        }
    }
}
