using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.ComponentModel;
using System.IO;
using WebApiApp.Contracts;
using WebApiApp.Models;

namespace WebApiApp.Services
{
	public class FileService : IFileService
	{
		private IConfiguration _iConfig;

		public FileService(IConfiguration iConfig)
		{
			_iConfig = iConfig;
		}

		public async Task UploadToAzureAsync(FileModel fileModel)
		{
			var cloudStorageAccount = _iConfig.GetValue<string>("CloudStorageAccount");
			var containerReference = _iConfig.GetValue<string>("ContainerReference");

			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(cloudStorageAccount);
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
			CloudBlobContainer container = blobClient.GetContainerReference(containerReference);

			string storagePath = $"{fileModel.Email}/{fileModel.Document.FileName}";
			CloudBlockBlob blockBlob = container.GetBlockBlobReference(storagePath);
			
			await blockBlob.UploadFromStreamAsync(fileModel.Document.OpenReadStream());
		
		}
	}
}
