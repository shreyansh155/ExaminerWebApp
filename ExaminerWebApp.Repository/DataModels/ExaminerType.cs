namespace ExaminerWebApp.Repository.DataModels
{
    public partial class ExaminerType
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<Examiner> Examiners { get; set; } = new List<Examiner>();
    }
}