using Application.Interfaces;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.File
{
    public class FileShareService : IFileShareService
    {

        private readonly ShareClient _shareClient;

        public FileShareService(ShareClient shareClient)
        {
            this._shareClient = shareClient;
        }

        public async Task DeleteFile(string fileName, string directoryName)
        {
            ShareDirectoryClient sharedirectoryclient = null;
            if (!string.IsNullOrEmpty(directoryName))
            {
                sharedirectoryclient = _shareClient.GetDirectoryClient(directoryName);
                await sharedirectoryclient.DeleteFileAsync(fileName);
            }
            else
            {
                sharedirectoryclient = _shareClient.GetRootDirectoryClient();
                await sharedirectoryclient.DeleteFileAsync(fileName);
            }
        }

        public async Task<Stream> DownloadFile(string fileName, string directoryName)
        {
            if (await _shareClient.ExistsAsync())
            {
                ShareDirectoryClient sharedirectoryclient = null;
                if (!string.IsNullOrEmpty(directoryName))
                {
                    sharedirectoryclient = _shareClient.GetDirectoryClient(directoryName);
                    await sharedirectoryclient.CreateIfNotExistsAsync();
                }
                else
                {
                    sharedirectoryclient = _shareClient.GetRootDirectoryClient();
                }
                // Ensure that the directory exists
                if (await sharedirectoryclient.ExistsAsync())
                { 
                    // Download the file
                    ShareFileClient fileClient = sharedirectoryclient.GetFileClient(fileName);
                   var file=await fileClient.DownloadAsync();
                  return  file.Value.Content;
                }
            }

            return null;
        }

        public Task ReadFiles(string directoryName)
        {
            var remaining = new Queue<ShareDirectoryClient>();
            if (!string.IsNullOrEmpty(directoryName))
            {
                remaining.Enqueue(_shareClient.GetDirectoryClient(directoryName));
            }
            else
            {
                remaining.Enqueue(_shareClient.GetRootDirectoryClient());
            }

            while (remaining.Count > 0)
            {
                ShareDirectoryClient dir = remaining.Dequeue();
                foreach (ShareFileItem item in dir.GetFilesAndDirectories())
                {
                    if (item.IsDirectory)
                    {
                        remaining.Enqueue(dir.GetSubdirectoryClient(item.Name));
                    }
                    else
                    {
                        // Download the file
                        ShareFileClient file = dir.GetFileClient(item.Name);
                     var xx=   file.Path;
                    }
                }
            }
            return null;
        }

        public async Task UploadFile(string fileName, string directoryName, string fileContents)
        {
            await _shareClient.CreateIfNotExistsAsync();
            // Ensure that the share exists
            if (await _shareClient.ExistsAsync())
            {
                ShareDirectoryClient sharedirectoryclient = null;
                if (!string.IsNullOrEmpty(directoryName))
                {
                    sharedirectoryclient = _shareClient.GetDirectoryClient(directoryName);
                    await sharedirectoryclient.CreateIfNotExistsAsync();
                }
                else
                {
                    sharedirectoryclient = _shareClient.GetRootDirectoryClient();
                }
                // Ensure that the directory exists
                if (await sharedirectoryclient.ExistsAsync())
                {
                    //write data.
                    ShareFileClient sharefileclient_in = sharedirectoryclient.CreateFile(fileName, fileContents.Length);
                    string filecontent_in = fileContents;
                    byte[] byteArray = Encoding.UTF8.GetBytes(filecontent_in);
                    MemoryStream stream1 = new MemoryStream(byteArray);
                    stream1.Position = 0;
                    sharefileclient_in.Upload(stream1);

                }


            }


        }
    }
}
