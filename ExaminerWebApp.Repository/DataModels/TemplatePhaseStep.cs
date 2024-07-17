namespace ExaminerWebApp.Repository.DataModels
{
    public partial class TemplatePhaseStep
    {
        public int Id { get; set; }

        public int TemplatePhaseId { get; set; }

        public int StepId { get; set; }

        public int Ordinal { get; set; }

        public bool? IsDeleted { get; set; }

        public virtual Step Step { get; set; } = null!;

        public virtual ApplicationTypeTemplatePhase TemplatePhase { get; set; } = null!;
    }
}