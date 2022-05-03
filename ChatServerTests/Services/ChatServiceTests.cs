using ChatServer;
using ChatServer.Repositories;
using ChatServer.Services;
using Grpc.Core;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ChatServerTests.Services
{
    public class ChatServiceTests
    {
        [Fact()]
        public void JoinTest()
        {
            File.WriteAllText("room1.json", "{\r\n  \"Users\": [\r\n    {\r\n      \"Name\": \"user1\",\r\n      \"ID\": 1110544760\r\n    }\r\n  ],\r\n  \"History\": {}\r\n}");
            File.WriteAllText("room2.json", "{\r\n  \"Users\": [\r\n    {\r\n      \"Name\": \"user2\",\r\n      \"ID\": 1110544761\r\n    }\r\n  ],\r\n  \"History\": {}\r\n}");

            var messages = new List<Message>
            {
                new() { Command  = "create", Text = "room1", User = "user1" },
                new() { Command  = "create", Text = "room2", User = "user2" },
                new() { Command  = "create", Text = "room2", User = "user3" },
                new() { Command  = "create", Text = "room2", User = "user4" }
            };

            Task.WaitAll(messages.Select(message => Task.Run(async () =>
            {
                var asyncStreamReader = new Mock<IAsyncStreamReader<Message>>();
                var asyncStreamWriter = new Mock<IServerStreamWriter<Message>>();

                var roomsRepository = new RoomRepository();
                var usersRepository = new UserRepository();
                var chatService = new ChatService(roomsRepository, usersRepository, null);
                var isFirstCall = true;
                asyncStreamReader.Setup(x => x.MoveNext(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(() =>
                    {
                        var result = isFirstCall;
                        isFirstCall = false;
                        return result;
                    });
                asyncStreamReader.Setup(streamReader => streamReader.Current).Returns(() => message);

                await chatService.Join(asyncStreamReader.Object, asyncStreamWriter.Object, null);
            })).ToArray());
        }
    }
}