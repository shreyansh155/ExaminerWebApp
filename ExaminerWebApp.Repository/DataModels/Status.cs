namespace ExaminerWebApp.Repository.DataModels
{
    public partial class Status
    {
        public int Id { get; set; }

        public string StatusName { get; set; } = null!;

        public virtual ICollection<Examiner> Examiners { get; set; } = new List<Examiner>();
    }
}