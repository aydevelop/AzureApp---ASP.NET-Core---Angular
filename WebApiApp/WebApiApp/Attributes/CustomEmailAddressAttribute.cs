using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WebApiApp.Attributes
{
	public class CustomEmailAddressAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(
		object value, ValidationContext validationContext)
		{
			Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
			if (!regex.IsMatch(value.ToString()))
			{
				return new ValidationResult(GetErrorMessage());
			}

			return ValidationResult.Success;
		}

		public string GetErrorMessage()
		{
			return $"Please enter a valid email";
		}
	}
}
