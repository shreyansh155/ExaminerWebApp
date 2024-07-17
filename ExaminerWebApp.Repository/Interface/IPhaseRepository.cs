using ExaminerWebApp.Repository.DataModels;

namespace ExaminerWebApp.Repository.Interface
{
    public interface IPhaseRepository : IBaseRepository<Phase>
    {
        IQueryable<Phase> GetAll();
    
        Task<bool> CheckIfPhaseExists(string phaseName);
        
        Task<bool> CheckPhaseOnUpdateExists(string phaseName,int? phaseId);
    }
}