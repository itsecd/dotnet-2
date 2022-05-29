using System;
using System.Collections.Concurrent;
using System.Xml.Serialization;

namespace ChatService.Models
{
    [Serializable]
    public class Room
    {
        public string Name { get; set; }
        [XmlIgnore]
        public ConcurrentBag<User> Users { get; set; }
        public ConcurrentDictionary<string, string> Messages { get; set; }   
    }
}
