using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace MSO_Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(new HelloRequest{Name = "GreeterClient"});
            Console.WriteLine(reply.Message);
            Console.ReadKey();
        }
    }
}
