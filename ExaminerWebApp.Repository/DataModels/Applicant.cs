namespace ExaminerWebApp.Repository.DataModels;

public partial class Applicant
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string? Phone { get; set; }

    public string Email { get; set; } = null!;

    public int ApplicantTypeId { get; set; }

    public string? FileName { get; set; }

    public string? FilePath { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ApplicantType ApplicantType { get; set; } = null!;
}