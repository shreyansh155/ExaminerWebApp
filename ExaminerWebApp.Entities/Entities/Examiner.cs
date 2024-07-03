using Microsoft.AspNetCore.Http;

namespace ExaminerWebApp.Entities.Entities
{
    public class Examiner
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; } = null!;

        public string? Phone { get; set; }

        public string Password { get; set; } = null!;

        public int ExaminerId { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? FilePath { get; set; }

        public string ExaminerTypeName { get; set; } = null!;

        public IFormFile FormFile { get; set; } = null!;

        public string Status { get; set; } = null!;

        public int StatusId { get; set; }

        public bool? IsDeleted { get; set; }
    }
}