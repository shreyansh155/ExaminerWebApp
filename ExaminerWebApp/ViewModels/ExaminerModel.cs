using System.ComponentModel.DataAnnotations;
using ExaminerWebApp.Composition.Helpers;
namespace ExaminerWebApp.ViewModels
{
    public class ExaminerModel
    {
        public int Id { get; set; }

        [Required]
        [NoNumbers(ErrorMessage = "First name cannot contain numbers")]
        public string Firstname { get; set; } = null!;

        [Required]
        [NoNumbers(ErrorMessage = "Last name cannot contain numbers")]
        public string? Lastname { get; set; }

        [Required]
        [DateNotInFuture(ErrorMessage = "Please enter a valid date of birth")]
        public DateTime Dateofbirth { get; set; }

        [Required]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
         ErrorMessage = "Please enter a valid phone number.")]
        public string? Phone { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; } = null!;
      
        public string? Password { get; set; }

        public int ExaminerTypeId { get; set; }

        public string? ExaminerTypeName { get; set; }

        public string? Filepath { get; set; }

        [FileValidation(new[] { ".pdf", ".jpg", ".jpeg", ".png" })]
        public IFormFile? FormFile { get; set; }
      
        public string? Status { get; set; }

        public bool? Isdeleted { get; set; }
    }
}