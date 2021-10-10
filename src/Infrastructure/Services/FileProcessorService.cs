using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class FileProcessorService : IFileProcessorService
    {
        private readonly IFileShareService _fileService;

        public FileProcessorService(IFileShareService fileService)
        {
            this._fileService = fileService;
        }

        public async Task ProcessFiles(string directory)
        {
            await this._fileService.ReadFiles(directory);

 
            //Read all the files

            //Match the pattern

            //Send a message with action
            
        }
    }
}
