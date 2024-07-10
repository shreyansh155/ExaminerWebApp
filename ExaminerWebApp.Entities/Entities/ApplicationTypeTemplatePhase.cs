namespace ExaminerWebApp.Entities.Entities
{
    public partial class ApplicationTypeTemplatePhase
    {
        public int Id { get; set; }

        public int TemplateId { get; set; }

        public int PhaseId { get; set; }

        public int Ordinal { get; set; }

        public bool? IsDeleted { get; set; }

    }
}