using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ePizzaHub.UI.Interfaces;

namespace ePizzaHub.UI.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private string connectionString;
        private string containerName;
        private IConfiguration _config;
        private IKeyVaultService _keyVaultService;
        private IWebHostEnvironment _env;
        public BlobStorageService(IConfiguration config, IKeyVaultService keyVaultService, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
            _keyVaultService = keyVaultService;

            if (_env.IsDevelopment())
            {
                connectionString = _config["Storage:Connection"]; //for appsettings.json
            }
            else
            {
                connectionString = _keyVaultService.GetSecret("StorageConnection").Result;
            }
            containerName = _config["Storage:Container"];
        }

        private string GenerateFileName(string fileName)
        {
            string[] strName = fileName.Split('.');
            string strFileName = DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmssfff") + "." + strName[strName.Length - 1];
            return strFileName;
        }

        public async Task<string> UploadFileToBlobAsync(string strFileName, Stream content, string contentType)
        {
            string fileName = this.GenerateFileName(strFileName);
            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
            container.CreateIfNotExists();

            BlobClient blob = container.GetBlobClient(fileName);
            await blob.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType });
            string uri = blob.Uri.ToString().Replace(_config["Storage:ImageAddress"],"");
            return uri;
        }

        public async void DeleteBlobData(string fileUrl)
        {
            Uri uriObj = new Uri(fileUrl);
            string BlobName = Path.GetFileName(uriObj.LocalPath);

            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
            BlobClient blob = container.GetBlobClient(BlobName);
            await blob.DeleteAsync();
        }
    }
}