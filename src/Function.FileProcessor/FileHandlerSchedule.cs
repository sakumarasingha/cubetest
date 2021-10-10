using System;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Function.FileProcessor
{
    public class FileHandlerSchedule
    {
        private const string FunctionName = "FileHandlerSchedule";

        private readonly IFileProcessorService _service;

        public FileHandlerSchedule(IFileProcessorService service)
        {
            this._service = service;
        }

        [FunctionName(FunctionName)]
        public async Task Run([TimerTrigger("*/1 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
 
            var dirName = Environment.GetEnvironmentVariable("DirectoryName");
            await this._service.ProcessFiles(dirName); 
        }
    }
}
