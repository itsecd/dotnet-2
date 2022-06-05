using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace ChatServer.Serializers
{
    public static class UserSerializer
    {
        public static void SerializeUser(User user)
        {
            var users = DeserializeUser();

            var storageFileName = "DataBases/";
            if (Directory.Exists(storageFileName))
            {
                Directory.CreateDirectory(storageFileName);
            }
            storageFileName = "DataBases/Users.xml";

            if (users.Any(us => user.Name == us.Name))
            {
                return;
            }

            users.Add(user);

            var xmlSerializer = new XmlSerializer(typeof(List<User>));
            using var fileStream = new FileStream(storageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileStream, users);
        }

        public static List<User> DeserializeUser()
        {
            var storageFileName = "DataBases/Users.xml";
            var deserializeUsers = new List<User>();

            if (File.Exists(storageFileName))
            {
                var xmlSerializer = new XmlSerializer(typeof(List<User>));
                using var fileStream = new FileStream(storageFileName, FileMode.Open);
                deserializeUsers = (List<User>)xmlSerializer.Deserialize(fileStream);
                return deserializeUsers;
            }
            return deserializeUsers;
        }
    }
}
