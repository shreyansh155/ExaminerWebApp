using ExaminerWebApp.Repository.DataModels;

namespace ExaminerWebApp.Repository.Interface
{
    public interface IPhaseRepository : IBaseRepository<Phase>
    {
        IQueryable<Phase> GetAll();
        bool CheckIfPhaseExists(string phaseName);
    }
}
