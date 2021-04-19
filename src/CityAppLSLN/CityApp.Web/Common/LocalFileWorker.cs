using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using CityApp.Interfaces;
using CityApp.Web.Helpers;
using CityApp.Web.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CityApp.Web.Common
{
      public class LocalFileWorker : IFileWorker
    {
        private readonly IWebHostEnvironment environment;
        private readonly IUrlHelper urlHelper;

        public LocalFileWorker(IWebHostEnvironment environment,
            IUrlHelper urlHelper, IOptions<StorageOptions> storageOptions)
        {
            this.environment = environment;
            this.urlHelper = urlHelper;
            RootPath = storageOptions.Value.RootPath;
        }

        public async Task<string> UploadFileAsync(Stream stream, string uniqueName)
        {
            var rootPath = Path.Combine(environment.WebRootPath, RootPath); //current directory
            var path = Path.Combine(rootPath, uniqueName);

            await using var fileStream = File.Create(path);
            stream.Seek(0, SeekOrigin.Begin);
            await stream.CopyToAsync(fileStream);

            return path;
        }

        public async Task<bool> DeleteFileAsync(string uniqueName)
        {
            var rootPath = Path.Combine(environment.WebRootPath, RootPath); //current directory
            var fileName = Path.Combine(rootPath, uniqueName);
            try
            {
                await Task.Run(() => File.Delete(fileName));
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
                return false;
            }

            return true;
        }

        public async Task<List<Tuple<string, string>>> GetFilesAsync(string containerName, bool includeSubDir)
        {
            var rootPath = Path.Combine(environment.WebRootPath, RootPath); //current directory
            var dirName = Path.Combine(rootPath, containerName);
            var list = new List<Tuple<string, string>>();
            await Task.Run(() =>
            {
                foreach (var currentFileInfo in new DirectoryInfo(dirName).GetFiles(
                    "*.*", SearchOption.AllDirectories))
                {
                    list.Add(new Tuple<string, string>(currentFileInfo.Name, currentFileInfo.FullName));
                }
            });

            return list;
        }

        public async Task<Stream> DownloadFileAsync(string uniqueName)
        {
            var rootPath = Path.Combine(environment.WebRootPath, RootPath); //current directory
            var fileName = Path.Combine(rootPath, uniqueName);
            var memoryStream = new MemoryStream();
            await using var stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
            await stream.CopyToAsync(memoryStream);

            return memoryStream;
        }

        public string GetFileUrl(string uniqueName, bool exists = false)
        {
            if (string.IsNullOrEmpty(uniqueName))
                return "";
            
            var fileName = Path.Combine(RootPath, uniqueName);
            if (exists)
            {
                if (!new FileInfo(fileName).Exists)
                    throw new FileNotFoundException("File was not found!");
            }

            return urlHelper.ToAbsoluteContent(fileName);
        }

        public string RootPath { get; set; }
        public string AccountKey { get; set; }
        public string AccountName { get; set; }
    }
}