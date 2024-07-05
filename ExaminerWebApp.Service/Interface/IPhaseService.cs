using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface
{
    public interface IPhaseService
    {
        IQueryable<Phase> GetAll();
        Phase GetPhaseById(int id);
        Task<Phase> CreatePhase(Phase model);
        bool DeletePhase(int id);
        bool UpdatePhase(Phase model);
    }
}