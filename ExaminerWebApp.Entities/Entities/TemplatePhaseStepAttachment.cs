using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminerWebApp.Entities.Entities
{
    public class TemplatePhaseStepAttachment
    {
        public int Id { get; set; }

        public int TemplatePhaseStepId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; } = null!;

        public int AttachmentTypeId { get; set; }

        public int Ordinal { get; set; }

        public string? FilePath { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? IsDeleted { get; set; }

        [NotMapped]
        public IFormFile? AttachmentFile { get; set; }

        public virtual DocumentFileType? AttachmentType { get; set; }
    }
}