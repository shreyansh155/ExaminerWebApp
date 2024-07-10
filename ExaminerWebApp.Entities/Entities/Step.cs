namespace ExaminerWebApp.Entities.Entities
{
    public partial class Step
    {
        public int Id { get; set; }

        public int PhaseId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? Instruction { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public int StepTypeId { get; set; }
        public string? StepType { get; set; }

        public bool? IsDeleted { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}