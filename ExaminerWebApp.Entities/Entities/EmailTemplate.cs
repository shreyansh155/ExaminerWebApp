using System.ComponentModel.DataAnnotations;

namespace ExaminerWebApp.Entities.Entities
{
    public partial class EmailTemplate
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public string? Template { get; set; }

        public bool? IsDefault { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int EmailTemplateType { get; set; }

        public bool? IsDeleted { get; set; }

    }
}