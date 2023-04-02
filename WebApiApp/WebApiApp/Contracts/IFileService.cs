using WebApiApp.Models;

namespace WebApiApp.Contracts
{
	public interface IFileService
	{
		Task UploadToAzureAsync(FileModel fileModel);
	}
}
