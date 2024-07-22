using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ExaminerWebApp.ViewHelpers
{
    public class FileValidationAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;

        public FileValidationAttribute(string[] allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_allowedExtensions.Contains(extension))
                {
                    return new ValidationResult($"This file extension is not allowed.");
                }
            }
            return ValidationResult.Success;
        }
    }
}