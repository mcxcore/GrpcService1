using Google.Protobuf;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService
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

        public override async Task<FileResponse> Transfer(FileRequest request, ServerCallContext context)
        {
            Console.WriteLine(request.FileType);
            Console.WriteLine(request.File.Length);
            var bytes= request.File.ToByteArray();
            await System.IO.File.WriteAllBytesAsync("D:\\"+Guid.NewGuid().ToString()+"."+request.FileType, bytes);
            return new FileResponse() {Message=true };
        }
    }
}
