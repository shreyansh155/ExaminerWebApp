namespace ExaminerWebApp.ViewModels
{
    public class AddStepModel
    {
        public int StepTypeId { get; set; }
        public int Ordinal { get; set; }
        public int PhaseId { get; set; }
        public List<StepsList> Steps { get; set; }  

    }
    public class StepsList
    {
        public int StepTypeId { get; set; }
        public int Ordinal { get; set; }
        public string Name { get; set; }
    }
}
