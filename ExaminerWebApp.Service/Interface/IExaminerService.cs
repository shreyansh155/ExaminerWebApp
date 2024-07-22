using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface
{
    public interface IExaminerService
    {
        Task<Examiner> GetExaminerById(int id);

        IQueryable<Examiner> GetAllExaminer();

        Task<Examiner> AddExaminer(Examiner model);

        Task<bool> DeleteExaminer(int id);


        Task<bool> UpdateExaminer(Examiner model);
        bool CheckEmailIfExists(string email);
    }
}