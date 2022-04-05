using System.Collections.Generic;

namespace PPTaskList.Controllers.Model
{
    public class TaskList
    {
        public int TaskId { get; set; }

        public string HeaderText { get; set; }

        public string TextDescription { get; set; }

        private static readonly List<string> _statuses = new List<string>
        {
            "Done", "Not ready yet"
        };
        public TaskList()
        {
            HeaderText = "Empty";
            TextDescription = "Empty";
        }

        public TaskList(string header, string text)
        {
            HeaderText = header;
            TextDescription = text;
        }
    }
}
