using ExaminerWebApp.Repository.DataModels;

namespace ExaminerWebApp.Repository.Interface
{
    public interface IPhaseStepRepository
    {
        Task<object> GetPhaseStepsAsync(IEnumerable<ApplicationTypeTemplatePhase> templatePhases, int templateId, int phaseId);

        Task<object> GetPhaseStepsByTemplateAsync(IEnumerable<ApplicationTypeTemplatePhase> templatePhases, int templateId, int phaseId);

        Task<List<TemplatePhaseStep>> AddStepsWithOrdinal(List<TemplatePhaseStep> model, ApplicationTypeTemplatePhase tempPhase);

        Task<List<Step>> GetStepList(int? templatePhaseStepId);

        Task<List<Step>> GetPhaseStepList(int? templatePhaseId);

        Task<TemplatePhaseStep> GetTemplatePhaseStep(int id);

        Task<int?> GetStepTypeId(int stepId);

        Task<TemplatePhaseStep?> AddPhaseStep(TemplatePhaseStep templatePhaseStep);
        Task<TemplatePhaseStep> UpdatePhaseStep(TemplatePhaseStep templatePhaseStep);

        Task<int?> DeleteStep(int id);

        Task<int> GetNewPhaseOrdinal(int templateId);

        Task<int> GetNewStepOrdinal(int templatePhaseId);

    }
}