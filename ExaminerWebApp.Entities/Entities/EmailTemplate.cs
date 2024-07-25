using System.ComponentModel.DataAnnotations;

namespace ExaminerWebApp.Entities.Entities
{
    public partial class EmailTemplate
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Please enter Email Template name")]
        [MaxLength(50, ErrorMessage = "Email Template name must be less than 50 characters.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Please enter Email Description")]
        public string Description { get; set; } = null!;

        public string? Template { get; set; }

        public bool IsDefault { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? IsDeleted { get; set; }

    }
}