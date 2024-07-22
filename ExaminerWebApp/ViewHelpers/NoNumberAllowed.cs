using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ExaminerWebApp.ViewHelpers
{
    public class NoNumbersAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            var stringValue = value.ToString();
            if (Regex.IsMatch(stringValue, @"\d"))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }
    }
}