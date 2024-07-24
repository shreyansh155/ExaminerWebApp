using ExaminerWebApp.Repository.DataModels;

namespace ExaminerWebApp.Repository.Interface
{
    public interface IApplicantRepository : IBaseRepository<Applicant>
    {
        Task<bool> CheckEmail(string email);

        IQueryable<Applicant> GetAll();
    }
}