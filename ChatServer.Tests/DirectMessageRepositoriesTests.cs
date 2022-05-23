using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ChatServer.Tests
{
    public class DirectMessageRepositoriesTests
    {
        [Fact]
        public void DirectMessageSerializerTest()
        {
            string receiver = "new_test_receiver";
            var initialMessage = new Serializers.DirectMessage(receiver, "new_test_name", "new_test_message");

            Serializers.DirectMessageSerializer.SerializeMessage(initialMessage);
            var deserializedMessage = Serializers.DirectMessageSerializer.DeSerializeMessage(receiver)[0];

            Assert.Equal(initialMessage, deserializedMessage);
        }

        [Fact]
        public void DirectMessageSerializerCollectionOneReceiverTest()
        {
            string receiver = "test_receiver";
            var initialCollection = new List<Serializers.DirectMessage>() {
                new Serializers.DirectMessage(receiver, "test_name", "test_message"),
                new Serializers.DirectMessage(receiver, "test_name1", "test_message1"),
                new Serializers.DirectMessage(receiver, "test_name2", "test_message2"),
                new Serializers.DirectMessage(receiver, "test_name3", "test_message3"),
                new Serializers.DirectMessage(receiver, "test_name4", "test_message4"),
                new Serializers.DirectMessage(receiver, "test_name5", "test_message5"),
            };

            foreach (var message in initialCollection)
            {
                Serializers.DirectMessageSerializer.SerializeMessage(message);
            }

            var deserializedCollection = Serializers.DirectMessageSerializer.DeSerializeMessage(receiver);

            Assert.True(initialCollection.SequenceEqual(deserializedCollection));
            //Assert.True(areTheyEqual);
        }

        [Fact]
        public void DirectMessageSerializerCollectionDifferentReceiversTest()
        {
            var receivers = new List<string>();
            var initialCollection = new List<Serializers.DirectMessage>();
            var deserializedCollection = new List<Serializers.DirectMessage>();

            for (int i = 0; i < 6; ++i)
            {
                receivers.Add($"test_receiver{i}");
                initialCollection.Add(new Serializers.DirectMessage(receivers[i], "test_name", "test_message"));

            }
            foreach (var message in initialCollection)
            {
                Serializers.DirectMessageSerializer.SerializeMessage(message);
            }
            foreach (var receiver in receivers)
            {
                deserializedCollection.Add(Serializers.DirectMessageSerializer.DeSerializeMessage(receiver)[0]);
            }

            Assert.True(initialCollection.SequenceEqual(deserializedCollection));
        }
    }
}
