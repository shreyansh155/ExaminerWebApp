namespace ExaminerWebApp.ViewModels
{
    public class StepViewModel
    {
        public int PhaseId { get; set; }
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Instruction { get; set; }
        public int TypeId { get; set; }
        public string? StepType { get; set; }
    }
}