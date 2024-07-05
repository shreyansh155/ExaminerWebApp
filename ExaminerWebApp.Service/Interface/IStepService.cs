using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface
{
    public interface IStepService
    {
        IQueryable<Step> GetAll(int phaseId);
        Step GetStepById(int id);
        Task<Step> CreateStep(Step model);
        bool DeleteStep(int id);
        bool UpdateStep(Step model);
    }
}
