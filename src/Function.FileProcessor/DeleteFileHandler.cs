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
    public class DeleteFileHandler
    {

        private readonly IFileShareService _fileService;

        public DeleteFileHandler(IFileShareService fileService)
        {
            this._fileService = fileService;
        }

        [FunctionName("DeleteFileHandler")]
        public async Task Run([ServiceBusTrigger("%ServiceBus:QueueName%", Connection = "ServiceBusConnectionString")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            var data = JsonConvert.DeserializeObject<FileMetaData>(myQueueItem);

            await this._fileService.DeleteFile(data.Name, data.DirectoryName);
        }
    }
}
