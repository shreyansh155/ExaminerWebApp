using ExaminerWebApp.Repository.DataModels;

namespace ExaminerWebApp.Repository.Interface
{
    public interface IStepRepository : IBaseRepository<Step>
    {
        IQueryable<Step> GetAllSteps(int phaseId);

        Task<List<StepType>> GetStepTypeList();

        Task<bool> CheckIfStepExists(Step step);

        Task<bool> CheckIfEditStepExists(Step step);

        Task<bool> UpdateInstruction(int? stepId,string instruction);
    }
}