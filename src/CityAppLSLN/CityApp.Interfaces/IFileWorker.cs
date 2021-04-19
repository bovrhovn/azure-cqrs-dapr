using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CityApp.Interfaces
{
    public interface IFileWorker
    {
        Task<string> UploadFileAsync(Stream stream, string uniqueName);
        Task<bool> DeleteFileAsync(string uniqueName);
        Task<List<Tuple<string,string>>> GetFilesAsync(string containerName, bool includeSubDir);
        Task<Stream> DownloadFileAsync(string uniqueName);
        string GetFileUrl(string uniqueName, bool exists = false);
        string RootPath { get; set; }
        string AccountKey { get; set; }
        string AccountName { get; set; }
    }
}