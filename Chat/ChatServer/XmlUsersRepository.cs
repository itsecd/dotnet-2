using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ChatServer
{
    public class XmlUsersRepository
    {
        private List<User> _users;

        private const string StorageFileName = "DataBaseUsers.xml";

        private void WriteToFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<User>));
            using var fileStream = new FileStream(StorageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileStream, _users);
        }

        private void ReadFromFile()
        {
            if (_users != null) return;

            if (!File.Exists(StorageFileName))
            {
                _users = new List<User>();
                return;
            }
            var xmlSerializer = new XmlSerializer(typeof(List<User>));
            using var fileStream = new FileStream(StorageFileName, FileMode.Open);
            _users = (List<User>)xmlSerializer.Deserialize(fileStream);
        }

        public void AddUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            ReadFromFile();
            _users.Add(user);
            WriteToFile();
        }
    }
}
