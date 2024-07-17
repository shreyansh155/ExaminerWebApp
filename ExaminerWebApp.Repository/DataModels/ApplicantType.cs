namespace ExaminerWebApp.Repository.DataModels;

public partial class ApplicantType
{
    public int Id { get; set; }

    public string ApplicantTypeName { get; set; } = null!;

    public virtual ICollection<Applicant> Applicants { get; set; } = new List<Applicant>();
}