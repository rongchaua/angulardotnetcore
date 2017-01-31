using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using WebApplication.Models;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [Route("api")]
    [EnableCors("HDCorsPolicy")]
    public class AddKnowledgeController : Controller
    {

        private IKnowledgeService knowledgeService;

        private readonly ILogger logger;
        private readonly AppSettings settings;


        public AddKnowledgeController(
            IKnowledgeService knowledgeService,
            ILoggerFactory loggerFactory,
            IOptions<AppSettings> settings
            )
        {
            this.knowledgeService = knowledgeService;
            this.logger = loggerFactory.CreateLogger(nameof(AddKnowledgeController));
            this.settings = settings.Value;
        }

        [HttpPut("collect")]
        public async Task<AddResult> AddKnowledge()
        {
            var itemId = Guid.NewGuid();
            var message = Request.Form["message"];
            var files = Request.Form.Files.ToList();
            var filePaths = await SaveFiles(itemId, files);
            var documents = ParseFiles(filePaths);
            var link = ParseFirstLink(message);
            var item = new AddKnowledgeItemModel()
            {
                Content = message,
                Link = link,
                Documents = documents
            };

            this.knowledgeService.AddKnowledgeItem(item);
            return new AddResult("OK");

        }

        DocumentType GetTypeByExtension(string extension)
        {
            switch (extension)
            {
                case "mp3":
                case "wav":
                    return DocumentType.Audio;
                case "mp4":
                case "mov":
                    return DocumentType.Video;
                case "png":
                case "jpg":
                case "gif":
                    return DocumentType.Image;
                default:
                    return DocumentType.Document;
            }
        }

        private WebsiteMetaInfo LoadMetaInfosFromYoutube()
        {
            return new WebsiteMetaInfo
            {
                Title = "Hallo Welt",
                Description = "Das ist die Beschreibung der Webseite"
            };
        }

        IEnumerable<DocumentModel> ParseFiles(IDictionary<string, Uri> files)
        {
            foreach (var item in files)
            {
                var info = new FileInfo(item.Key);
                var model = new DocumentModel()
                {
                    Name = info.Name,
                    Source = item.Value.ToString(),
                    Type = GetTypeByExtension(info.Extension).ToString()
                };

                yield return model;
            }
        }

        private LinkModel ParseFirstLink(string content)
        {
            string youtubePattern = @"^(https?\:\/\/)?(www\.)?(youtube\.com|youtu\.?be)\/.+$";
            var youtubeMatch = Regex.Match(content, youtubePattern);
            if (youtubeMatch.Length >= 1)
            {
                var info = LoadMetaInfosFromYoutube();
                return new LinkModel
                {
                    Url = youtubeMatch.Value,
                    Title = info.Title,
                    Description = info.Description
                };
            }
            return null;
        }
        private async Task<IDictionary<string, Uri>> SaveFiles(Guid id, ICollection<IFormFile> files)
        {
            this.logger.LogInformation($"Files: {files.Count}");
            var result = new Dictionary<string, Uri>();
            foreach (var file in files)
            {
                var filename = ContentDispositionHeaderValue
                                .Parse(file.ContentDisposition)
                                .FileName
                                .Trim('"');

                this.logger.LogInformation($"FileName: {filename}");

                var credentials = new StorageCredentials(this.settings.AzureStorageName, this.settings.AzureStorageKey);
                var storageAccount = new CloudStorageAccount(credentials, true);

                var fileClient = storageAccount.CreateCloudFileClient();

                var share = fileClient.GetShareReference(this.settings.AzureStorageFileShareName);

                try
                {
                    await share.CreateIfNotExistsAsync();
                    var directory = share.GetRootDirectoryReference();
                    var fileRef = directory.GetFileReference(filename);
                    await fileRef.DeleteIfExistsAsync();
                    using (var stream = file.OpenReadStream())
                    {
                        await fileRef.UploadFromStreamAsync(stream);
                    }
                    result.Add(filename, fileRef.Uri);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }

            return result;
        }
    }
}