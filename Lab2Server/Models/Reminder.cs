using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2Server.Models
{
    public class Reminder
    {
        public Reminder(string name, string description, DateTime dateTime)
        {
            Name = name;
            Description = description;
            DateTime = dateTime;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
    }
}
