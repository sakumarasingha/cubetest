using System;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Function.FileProcessor
{
    public class MoveFileHandler
    {

        private readonly IFileShareService _fileService;

        public MoveFileHandler(IFileShareService fileService)
        {
            this._fileService = fileService;
        }


        [FunctionName("MoveFileHandler")]
        public async Task Run([ServiceBusTrigger("%ServiceBus:QueueName%", Connection = "ServiceBusConnectionString")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            var data = JsonConvert.DeserializeObject<FileMetaData>(myQueueItem);

            //get file content

            //move the file

            //Send delete message to service bus


            var moveDir = Environment.GetEnvironmentVariable("MoveDirectoryName");
            await this._fileService.UploadFile(data.Name, moveDir, "");
        }
    }
}
