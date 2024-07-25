using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface
{
    public interface IStepService
    {
        Task<PaginationSet<Step>> GetAll(int phaseId, PaginationSet<Step> pager);

        Task<Step> GetStepById(int id);

        Task<Step> CreateStep(Step model);

        Task<int> DeleteStep(int id);

        Task<Step> UpdateStep(Step model);

        Task<List<StepType>> GetStepTypeList();

        Task<IQueryable<Step>> GetStepByPhaseId(int phaseId);

        Task<bool> CheckIfExists(int? id, string name);
    }
}