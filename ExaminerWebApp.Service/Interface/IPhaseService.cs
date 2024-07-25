using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface
{
    public interface IPhaseService
    {
        Task<PaginationSet<Phase>> GetAll(PaginationSet<Phase> pager);

        Task<Phase> GetPhaseById(int id);

        Task<Phase> CreatePhase(Phase model);

        Task<int> DeletePhase(int id);

        Task<Phase> UpdatePhase(Phase model);

        Task<bool> CheckIfExists(int? id,string phaseName);
    }
}