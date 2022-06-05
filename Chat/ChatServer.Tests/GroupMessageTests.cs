using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ChatServer.Tests
{
    public class GroupMessageTests
    {
        [Fact]
        public void GroupMessageSerializerTest()
        {
            string group = "test_single_group_one_message";

            var initialMessage = new Serializers.GroupMessage(group, "test_name", "test_message");
            Serializers.GroupMessageSerializer.SerializeMessage(initialMessage);

            var deserializedMessageList = Serializers.GroupMessageSerializer.DeserializeMessage(group);
            var deserializedMessage = deserializedMessageList[0];

            Assert.Equal(initialMessage, deserializedMessage);
        }

        [Fact]
        public void GroupMessageSerializerCollectionOneGroupTest()
        {
            string group = "test_single_group_many_messages";
            var initCollection = new List<Serializers.GroupMessage>() {
                new Serializers.GroupMessage(group, "test_name", "test_message"),
                new Serializers.GroupMessage(group, "test_name1", "test_message1"),
                new Serializers.GroupMessage(group, "test_name2", "test_message2"),
                new Serializers.GroupMessage(group, "test_name3", "test_message3"),
                new Serializers.GroupMessage(group, "test_name4", "test_message4"),
                new Serializers.GroupMessage(group, "test_name5", "test_message5"),
            };

            foreach (var message in initCollection)
            {
                Serializers.GroupMessageSerializer.SerializeMessage(message);
            }

            var deserializedCollection = Serializers.GroupMessageSerializer.DeserializeMessage(group);

            Assert.True(initCollection.SequenceEqual(deserializedCollection));
        }

        [Fact]
        public void GroupMessageSerializerCollectionDifferentGroupsTest()
        {
            var groups = new List<string>();
            var initialCollection = new List<Serializers.GroupMessage>();
            var deserializedCollection = new List<Serializers.GroupMessage>();

            for (int i = 0; i < 3; ++i)
            {
                groups.Add($"test_many_group{i}");
                initialCollection.Add(new Serializers.GroupMessage(groups[i], "test_name", "test_message"));

            }
            foreach (var message in initialCollection)
            {
                Serializers.GroupMessageSerializer.SerializeMessage(message);
            }
            foreach (var group in groups)
            {
                var deserializedCollectionFromOneGroup = Serializers.GroupMessageSerializer.DeserializeMessage(group);
                deserializedCollection.Add(deserializedCollectionFromOneGroup[0]);
            }

            Assert.True(initialCollection.SequenceEqual(deserializedCollection));
        }
    }
}
