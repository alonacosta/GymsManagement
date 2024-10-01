using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace GymManagement.Helpers
{
    public class BlobHelper : IBlobHelper
    {       
        private readonly BlobServiceClient _blobServiceClient;

        public BlobHelper(IConfiguration configuration)
        {
            string keys = configuration["Blob:ConnectionString"];
            _blobServiceClient = new BlobServiceClient(keys);           
        }
        public async Task<Guid> UploadBlobAsync(IFormFile file, string containerName)
        {
            Stream stream = file.OpenReadStream();
            return await UploadStreamAsync(stream, containerName);
        }

        private async Task<Guid> UploadStreamAsync(Stream stream, string containerName)
        {
            Guid name = Guid.NewGuid();
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(containerName);
            await container.CreateIfNotExistsAsync(PublicAccessType.Blob);
            BlobClient blobClient = container.GetBlobClient(name.ToString());
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = "application/octet-stream" });
            return name;
        }
    }
}
