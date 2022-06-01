using Xunit;

namespace ChatServer.Tests
{
    public class GroupListTests
    {
        [Fact]
        public void GroupListSerializerTest()
        {
            var groupName = "test_groupName";
            var name = "test_name";

            var initGroupList = new Serializers.GroupList(groupName, name);
            Serializers.GroupListSerializer.SerializeGroup(initGroupList);

            var deserializedGroupList = Serializers.GroupListSerializer.DeserializeGroup(groupName)[0];

            Assert.Equal(initGroupList, deserializedGroupList);

        }
    }
}
