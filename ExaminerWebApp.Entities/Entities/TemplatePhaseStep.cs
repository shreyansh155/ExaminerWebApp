
namespace ExaminerWebApp.Entities.Entities
{
    public partial class TemplatePhaseStep
    {
        public int Id { get; set; }

        public int TemplatePhaseId { get; set; }

        public int StepId { get; set; }

        public bool? IsDeleted { get; set; }
    }
}