using Google.Protobuf;
using Grpc.Net.Client;
using GrpcService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcClient.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public async Task<bool> test()
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new Greeter.GreeterClient(channel);
            var bytes = System.IO.File.ReadAllBytes("D:\\Downloads\\111.html");
            var result = await client.TransferAsync(new FileRequest() { File = ByteString.CopyFrom(bytes), FileType = "html" });
            return result.Message;
        }

        public async Task<bool> bigfile()
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new BigFileHandler.BigFileHandlerClient(channel);
            var uploadResult = client.UploadBigFile();
            var uploadPath = "D:\\Downloads\\海洋科学-专业学位.pdf";
            using (var fileStream = System.IO.File.OpenRead(uploadPath))
            {
                var sended = 0L;
                var totalLength = fileStream.Length;
                var buffer = new byte[1024 * 1024]; // 每次最多发送 1M 的文件内容
                while (sended < totalLength)
                {
                    var length = await fileStream.ReadAsync(buffer);
                    sended += length;

                    var request = new RequestInfo()
                    {
                        File = ByteString.CopyFrom(buffer),
                        FileType = ".pdf"
                    };
                    await uploadResult.RequestStream.WriteAsync(request);
                }
            }
            await uploadResult.RequestStream.CompleteAsync();

            return true;
        }
    }
}
