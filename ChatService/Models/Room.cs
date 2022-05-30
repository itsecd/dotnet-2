using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ChatService.Models
{
    [Serializable]
    public class Room
    {
        public string Name { get; set; }
        [XmlIgnore]
        public ConcurrentBag<User> Users { get; set; } = new();
        public List<string> Messages { get; set; } = new();
    }
}
