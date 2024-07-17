using ExaminerWebApp.Repository.DataModels;

namespace ExaminerWebApp.Repository.Interface
{
    public interface IApplicantRepository : IBaseRepository<Applicant>
    {
        public bool CheckEmail(string email);
     
        public IQueryable<Applicant> GetAll();
    }
}