using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface
{
    public interface IApplicantService
    {
        Task<Applicant> GetApplicantById(int id);

        Task<PaginationSet<Entities.Entities.Applicant>> GetAllApplicants(PaginationSet<Applicant> pager);

        Task<Applicant> AddApplicant(Applicant model);

        Task<bool> CheckEmailIfExists(string email);

        Task<int> DeleteApplicant(int id);

        Task<Applicant> UpdateApplicant(Applicant model);
    }
}