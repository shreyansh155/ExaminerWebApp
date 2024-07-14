using ExaminerWebApp.Repository.DataModels;

namespace ExaminerWebApp.Repository.Interface
{
    public interface IPhaseStepRepository
    {
        Task<object> GetPhaseStepsAsync(IEnumerable<ApplicationTypeTemplatePhase> templatePhases, int templateId, int phaseId);
        Task<object> GetPhaseStepsByTemplateAsync(IEnumerable<ApplicationTypeTemplatePhase> templatePhases, int templateId, int phaseId);
        List<TemplatePhaseStep> AddStepsWithOrdinal(List<TemplatePhaseStep> model, ApplicationTypeTemplatePhase tempPhase);
    }
}
