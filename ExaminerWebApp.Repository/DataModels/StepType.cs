namespace ExaminerWebApp.Repository.DataModels
{
    public partial class StepType
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<Step> Steps { get; set; } = new List<Step>();
    }
}