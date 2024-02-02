using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;

namespace GrpcService
{
    public class BigFileService:BigFileHandler.BigFileHandlerBase
    {
        public override async Task<ResponseInfo> UploadBigFile(IAsyncStreamReader<RequestInfo> requestStream, ServerCallContext context)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            var requests = new Queue<RequestInfo>();
            while (await requestStream.MoveNext())
            {
                var request = requestStream.Current;
                requests.Enqueue(request);
            }

            var first = requests.Peek();
            var fileExt = first.FileType;
            var fileName = $"{Guid.NewGuid().ToString()}{fileExt}";
            var filePath = "D:\\"+fileName;

            var fileFolder = Directory.GetParent(filePath);
            if (fileFolder != null && !fileFolder.Exists)
                fileFolder.Create();

            using (var fileStream = File.Open(filePath, FileMode.Append, FileAccess.Write))
            {
                var received = 0L;
                while (requests.Count() > 0)
                {
                    var current = requests.Dequeue();
                    var buffer = current.File.ToByteArray();
                    fileStream.Seek(received, SeekOrigin.Begin);
                    await fileStream.WriteAsync(buffer);
                    received += buffer.Length;
                }

                return new ResponseInfo() {Message=""};
            }
        }
    }
}
