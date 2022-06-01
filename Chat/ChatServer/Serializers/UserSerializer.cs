using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ChatServer.Serializers
{
    public static class UserSerializer
    {
        static string StorageFileName = "DataBases/Users.xml";
        public static void SerializeUser(User user)
        {

            List<User> users = new List<User>();
            users = DeserializeUser();
            foreach(User us in users)
            {
                if (user.Name == us.Name) return;
            }

            users.Add(user);
            
            var xmlSerializer = new XmlSerializer(typeof(List<User>));
            using var fileStream = new FileStream(StorageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileStream, users);
        }

        public static List<User> DeserializeUser()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<User>));
            using var fileStream = new FileStream(StorageFileName, FileMode.Open);
            var deserializeUsers = (List<User>)xmlSerializer.Deserialize(fileStream);
            return deserializeUsers;
        }
    }
}
