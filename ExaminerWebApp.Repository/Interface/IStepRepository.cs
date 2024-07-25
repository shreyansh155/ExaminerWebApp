using ExaminerWebApp.Repository.DataModels;

namespace ExaminerWebApp.Repository.Interface
{
    public interface IStepRepository : IBaseRepository<Step>
    {
        IQueryable<Step> GetAllSteps(int phaseId);

        Task<List<StepType>> GetStepTypeList();

        Task<bool> CheckIfExists(int? id, string name);

        Task<bool> UpdateInstruction(int? stepId,string instruction);
    }
}