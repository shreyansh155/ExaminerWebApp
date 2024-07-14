
namespace ExaminerWebApp.Entities.Entities
{
    public partial class TemplatePhaseStep
    {
        public int Id { get; set; }

        public int TemplatePhaseId { get; set; }

        public int StepId { get; set; }

        public int? Ordinal { get; set; }

        public bool? IsInTemplatePhaseSteps { get; set; }

        public bool? IsDeleted { get; set; }
    }
}