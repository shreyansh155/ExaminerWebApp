using ExaminerWebApp.Repository.DataModels;

namespace ExaminerWebApp.Repository.Interface
{
    public interface IStepRepository : IBaseRepository<Step>
    {
        IQueryable<Step> GetAllSteps(int phaseId);
        List<StepType> GetStepTypeList();
        bool CheckIfStepExists(int phaseId, string stepName);
    }
}
