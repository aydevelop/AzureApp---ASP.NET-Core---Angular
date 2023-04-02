using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApiApp.Attributes;
using WebApiApp.Contracts;
using WebApiApp.Models;

namespace WebApiApp.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class HomeController : ControllerBase
	{
		private readonly IFileService _fileService;

		public HomeController(IFileService fileService)
		{
			_fileService = fileService;
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromForm] FileModel fileModel)
		{
			if (fileModel != null)
			{
				await _fileService.UploadToAzureAsync(fileModel);
				return StatusCode(201);
			}

			return StatusCode(400);
		}
	}
}