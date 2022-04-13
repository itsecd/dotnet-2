using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2Server.Models
{
    public class User
    {
        public User(string name, int phoneNumber)
        {
            Name = name;
            PhoneNumber = phoneNumber;
        }
        public string Name { get; set; }
        public int PhoneNumber { get; set; }
    }
}
