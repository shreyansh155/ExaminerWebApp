
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminerWebApp.Entities.Entities
{
    public partial class TemplatePhaseStep
    {
        public int? Id { get; set; }

        public int? TemplatePhaseId { get; set; }

        public int? StepId { get; set; }

        [Range(1, int.MaxValue)]
        [Required(ErrorMessage = "Please enter a valid ordinal number")]
        public int? Ordinal { get; set; }

        public bool? IsInTemplatePhaseSteps { get; set; }

        public bool? IsDeleted { get; set; }

        [NotMapped]
        public string? Instruction { get; set; }

        public Step? Step { get; set; }
    }
}