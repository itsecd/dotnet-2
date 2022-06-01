using Xunit;

namespace ChatServer.Tests
{
    public class UserTests
    {
        [Fact]
        public void UserSerializerTest()
        {
            string user = "test_name";

            var initUser = new User(user);
            Serializers.UserSerializer.SerializeUser(initUser);

            var deserializeUser = Serializers.UserSerializer.DeserializeUser()[0];

            Assert.Equal(initUser, deserializeUser);
        }
    }
}
