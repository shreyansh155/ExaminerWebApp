using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface
{
    public interface IApplicantService
    {
        Applicant GetApplicantById(int id);
        IQueryable<Applicant> GetAllApplicants();
        Task<Applicant> AddApplicant(Applicant model);
        bool CheckEmailIfExists(string email);
        bool DeleteApplicant(int id);
        bool UpdateApplicant(Applicant model);
    }
}
