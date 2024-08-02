using System.ComponentModel.DataAnnotations;

namespace ExaminerWebApp.Entities.Entities
{
    public class TemplatePhaseStepDocumentProof
    {
        public int Id { get; set; }

        public int TemplatePhaseStepId { get; set; }

        [MaxLength(50)]
        public string Title { get; set; } = null!;

        [MaxLength(250)]
        public string? Description { get; set; }

        public int DocumentFileType { get; set; }

        public bool IsRequired { get; set; }

        [Range(1, int.MaxValue)]
        public int Ordinal { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? IsDeleted { get; set; }

        public DocumentFileType? DocumentFileTypeNavigation { get; set; }
    }
}