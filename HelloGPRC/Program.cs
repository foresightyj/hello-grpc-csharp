using Grpc.Core;
using HelloWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloGPRC
{

    //https://github.com/grpc/grpc/blob/master/src/csharp/BUILD-INTEGRATION.md
    public class HelloServiceImpl : HelloService.HelloServiceBase
    {
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply()
            {
                Message = "Hello from dot net: " + request.Name,
            });
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                throw new InvalidOperationException("args.Length must be 2 but is " + args.Length);
            }
            var command = args[0];
            var port = 8088;
            if (command.ToLower() == "server")
            {
                Server server = new Server
                {
                    Services = { HelloService.BindService(new HelloServiceImpl()) },
                    Ports = { new ServerPort("localhost", port, ServerCredentials.Insecure) }
                };
                server.Start();

                Console.WriteLine("RouteGuide server listening on port " + port);
                Console.WriteLine("Press any key to stop the server...");
                Console.ReadKey();

                server.ShutdownAsync().Wait();
            }
            else if (command.ToLower() == "client")
            {
                Channel channel = new Channel($"localhost:{port}", ChannelCredentials.Insecure);
                var client = new HelloService.HelloServiceClient(channel);

                // YOUR CODE GOES HERE
                var reply = client.SayHello(new HelloRequest
                {
                    Name = "jiannan"
                });

                Console.WriteLine("Reply: " + reply.Message);
                channel.ShutdownAsync().Wait();
            }
            else
            {
                throw new InvalidOperationException("do not support command: " + command);
            }
        }
    }
}
