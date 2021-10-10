using System.IO;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFileShareService
    {
        Task ReadFiles(string directoryName);
        Task UploadFile(string fileName, string directoryName, string fileContents);
        Task DeleteFile(string fileName, string directoryName);
        Task<Stream> DownloadFile(string fileName, string directoryName);

    }
}
