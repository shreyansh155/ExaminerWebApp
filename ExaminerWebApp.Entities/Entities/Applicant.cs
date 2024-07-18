using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminerWebApp.Entities.Entities
{
    public partial class Applicant
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string? Phone { get; set; }

        public string Email { get; set; } = null!;

        public int ApplicantTypeId { get; set; }

        public string? FileName { get; set; }

        public string? FilePath { get; set; }

        public bool? IsDeleted { get; set; }

        [NotMapped]
        public IFormFile? FormFile { get; set; }
       
        [NotMapped]
        public string ApplicantType { get; set; } = null!;
      
        public virtual ApplicantType Setting { get; set; } = null!;
    }
}