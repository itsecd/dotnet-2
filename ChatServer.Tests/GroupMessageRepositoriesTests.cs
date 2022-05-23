using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ChatServer.Tests
{
    public class GroupMessageRepositoriesTests
    {
        [Fact]
        public void GroupMessageSerializerTest() {
            string group = "new_test_group";
            var initialMessage = new Serializers.GroupMessage(group, "new_test_name", "new_test_message");

            Serializers.GroupMessageSerializer.SerializeMessage(initialMessage);
            var deserializedMessage = Serializers.GroupMessageSerializer.DeSerializeMessage(group)[0];

            Assert.Equal(initialMessage, deserializedMessage);
        }

        [Fact]
        public void GroupMessageSerializerCollectionOneGroupTest()
        {
            string group = "test_group";
            var initialCollection = new List<Serializers.GroupMessage>() { 
                new Serializers.GroupMessage(group, "test_name", "test_message"),
                new Serializers.GroupMessage(group, "test_name1", "test_message1"),
                new Serializers.GroupMessage(group, "test_name2", "test_message2"),
                new Serializers.GroupMessage(group, "test_name3", "test_message3"),
                new Serializers.GroupMessage(group, "test_name4", "test_message4"),
                new Serializers.GroupMessage(group, "test_name5", "test_message5"),
            };

            foreach (var message in initialCollection) 
            {
                Serializers.GroupMessageSerializer.SerializeMessage(message);
            }

            var deserializedCollection = Serializers.GroupMessageSerializer.DeSerializeMessage(group);

            Assert.True(initialCollection.SequenceEqual(deserializedCollection));
            //Assert.True(areTheyEqual);
        }

        [Fact]
        public void GroupMessageSerializerCollectionDifferentGroupsTest()
        {
            var groups = new List<string>();
            var initialCollection = new List<Serializers.GroupMessage>();
            var deserializedCollection = new List<Serializers.GroupMessage>();

            for (int i = 0; i < 6; ++i)
            {
                groups.Add($"test_group{i}");
                initialCollection.Add(new Serializers.GroupMessage(groups[i], "test_name", "test_message"));

            }
            foreach (var message in initialCollection)
            {
                Serializers.GroupMessageSerializer.SerializeMessage(message);
            }
            foreach (var group in groups)
            {
                deserializedCollection.Add(Serializers.GroupMessageSerializer.DeSerializeMessage(group)[0]);
            }

            Assert.True(initialCollection.SequenceEqual(deserializedCollection));
        }
    }
}

