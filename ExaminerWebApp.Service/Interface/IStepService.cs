using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface
{
    public interface IStepService
    {
        IQueryable<Step> GetAll(int phaseId);

        Task<Step> GetStepById(int id);

        Task<Step> CreateStep(Step model);

        Task<bool> DeleteStep(int id);

        bool UpdateStep(Step model);

        Task<List<StepType>> GetStepTypeList();

        Task<IQueryable<Step>> GetStepByPhaseId(int phaseId);

        Task<bool> CheckIfStepExists(int phaseId, string stepName);

        Task<bool> CheckIfEditStepExists(int phaseId, int? stepId, string stepName);
    }
}