using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface
{
    public interface IExaminerService
    {
        Examiner GetExaminerById(int id);
        IQueryable<Examiner> GetAllExaminer();
        Task<Examiner> AddExaminer(Examiner model);
        bool DeleteExaminer(int id);
        bool CheckEmailIfExists(string email);
        bool UpdateExaminer(Examiner model);
       // bool ResetPassword();
    }
}
