using System.Collections.Generic;

namespace PPTask.Controllers.Model
{
    public class Tags
    {
        private readonly List<string> _statuses = new List<string>
        {
            "Immediately", "Remake", "Finalize","Done", "Not ready yet"
        };

        private readonly List<string> _colours = new List<string>
        {
            "Green", "Red", "Yellow"
        };

        private List<string> _tags;

        public List<string> TagList 
        {
            get => _tags;
            set
            {
                for(var i = 0; i < value.Count; i++)
                {
                    if (_statuses.Exists(x => x == value[i]) || _colours.Exists(x => x == value[i]))
                    {
                        _tags.Add (value[i]);
                    }
                }
            }
        }

        public Tags() { }

        public Tags( List<string> tags)
        {
            TagList = tags;
        }
    }
}
