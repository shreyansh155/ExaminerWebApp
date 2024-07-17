namespace ExaminerWebApp.Repository.DataModels
{
    public partial class Step
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? Instruction { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public int StepTypeId { get; set; }

        public bool? IsDeleted { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? PhaseId { get; set; }

        public virtual Phase? Phase { get; set; }

        public virtual StepType StepType { get; set; } = null!;

        public virtual ICollection<TemplatePhaseStep> TemplatePhaseSteps { get; set; } = new List<TemplatePhaseStep>();
    }
}