using System.ComponentModel.DataAnnotations;
using WebApiApp.Attributes;

namespace WebApiApp.Models
{
	public class FileModel
	{
		[CustomEmailAddressAttribute]
		public string Email { get; set; }


		[AllowedExtensions(new string[] { ".docx" })]
		public IFormFile Document { get; set; }
	}
}
