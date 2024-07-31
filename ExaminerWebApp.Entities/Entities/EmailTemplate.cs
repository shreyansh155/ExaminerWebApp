using System.ComponentModel.DataAnnotations;

namespace ExaminerWebApp.Entities.Entities
{
    public partial class EmailTemplate
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Email Template name must be less than 50 characters.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500, ErrorMessage = "Description must be less than 500 characters.")]
        public string Description { get; set; } = null!;

        [MaxLength(500, ErrorMessage = "Template must be less than 500 characters.")]
        public string? Template { get; set; }

        public bool IsDefault { get; set; } = false;

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}