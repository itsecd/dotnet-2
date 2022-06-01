using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ChatServer.Tests
{
    public class DirectMessageTests
    {
        [Fact]
        public void DirectMessageSerializerTest()
        {
            string receiver = "test_single_receiver_one_message";

            var initMessage = new Serializers.DirectMessage(receiver, "test_name", "test_message");
            Serializers.DirectMessageSerializer.SerializeMessage(initMessage);

            var deserializeMessage = Serializers.DirectMessageSerializer.DeserializeMessage(receiver)[0];

            Assert.Equal(initMessage, deserializeMessage);
        }

        [Fact]
        public void DirectMessageSerializerCollectionOneReceiverTest()
        {
            string receiver = "test_single_receiver_many_messages";
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

            var deserializedCollection = Serializers.DirectMessageSerializer.DeserializeMessage(receiver);

            Assert.True(initialCollection.SequenceEqual(deserializedCollection));
        }

        [Fact]
        public void DirectMessageSerializerCollectionDifferentReceiversTest()
        {
            var receivers = new List<string>();
            var initCollection = new List<Serializers.DirectMessage>();
            var deserializedCollection = new List<Serializers.DirectMessage>();

            for (int i = 0; i < 3; ++i)
            {
                receivers.Add($"test_many_receiver{i}");
                initCollection.Add(new Serializers.DirectMessage(receivers[i], "test_name", "test_message"));

            }
            foreach (var message in initCollection)
            {
                Serializers.DirectMessageSerializer.SerializeMessage(message);
            }
            foreach (var receiver in receivers)
            {
                deserializedCollection.Add(Serializers.DirectMessageSerializer.DeserializeMessage(receiver)[0]);
            }

            Assert.True(initCollection.SequenceEqual(deserializedCollection));
        }


    }
}
