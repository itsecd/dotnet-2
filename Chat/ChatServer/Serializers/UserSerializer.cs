using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace ChatServer.Serializers
{
    public static class UserSerializer
    {
        static string storageFileName = "DataBases/Users.xml";
        public static void SerializeUser(User user)
        {

            var users = DeserializeUser();
            if (users.Any(us => user.Name == us.Name)) return;

            users.Add(user);
            
            var xmlSerializer = new XmlSerializer(typeof(List<User>));
            using var fileStream = new FileStream(storageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileStream, users);
        }

        public static List<User> DeserializeUser()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<User>));
            using var fileStream = new FileStream(storageFileName, FileMode.Open);
            var deserializeUsers = (List<User>)xmlSerializer.Deserialize(fileStream);
            return deserializeUsers;
        }
    }
}
