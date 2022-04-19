using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override async Task StreamingFromServer(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            for(var i = 0 ; i < 10; i++)
            {
                await responseStream.WriteAsync(new HelloReply { Message = request.Name + " " + i });
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            
        }

        public override async Task<HelloReply> StreamingFromClient(IAsyncStreamReader<HelloRequest> requestStream, ServerCallContext context)
        {
            List<string> messages = new();
            while (await requestStream.MoveNext())
            {
                var message = requestStream.Current;
                _logger.LogInformation(message.Name);
                messages.Add(message.Name); 
            }
            return new HelloReply { Message = string.Join(" ", messages) };
        }
        public override async Task StreamingBothWays(IAsyncStreamReader<HelloRequest> requestStream, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            var readText = Task.Run(async () =>
           {
               while (await requestStream.MoveNext())
               {
                   var message = requestStream.Current;
                   _logger.LogInformation(message.Name);
               }
           });

            while (!readText.IsCompleted)
            {
                await responseStream.WriteAsync(new HelloReply { Message = "I`m alive" });
                await Task.Delay(TimeSpan.FromSeconds(1), context.CancellationToken);
            }
        }
    }
}
