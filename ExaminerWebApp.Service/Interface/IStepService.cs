using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface
{
    public interface IStepService
    {
        Task<PaginationSet<Step>> GetAll(int phaseId, PaginationSet<Step> pager);

        Task<Step> GetStepById(int id);

        Task<Step> CreateStep(Step model);

        Task<bool> DeleteStep(int id);

        Task<bool> UpdateStep(Step model);

        Task<List<StepType>> GetStepTypeList();

        Task<IQueryable<Step>> GetStepByPhaseId(int phaseId);

        Task<bool> CheckIfStepExists(Step model);

        Task<bool> CheckIfEditStepExists(Step step);
    }
}