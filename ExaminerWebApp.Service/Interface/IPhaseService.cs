using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface
{
    public interface IPhaseService
    {
        Task<PaginationSet<Phase>> GetAll(PaginationSet<Phase> pager);

        Task<Phase> GetPhaseById(int id);

        Task<Phase> CreatePhase(Phase model);

        Task<bool> DeletePhase(int id);

        Task<bool> UpdatePhase(Phase model);

        Task<bool> CheckIfPhaseExists(string phaseName);

        Task<bool> CheckPhaseOnUpdateExists(string phaseName, int? phaseId);
    }
}