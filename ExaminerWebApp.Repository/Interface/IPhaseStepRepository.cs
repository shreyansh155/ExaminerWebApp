using ExaminerWebApp.Repository.DataModels;

namespace ExaminerWebApp.Repository.Interface
{
    public interface IPhaseStepRepository
    {
        Task<object> GetPhaseStepsAsync(IEnumerable<ApplicationTypeTemplatePhase> templatePhases, int templateId, int phaseId);

        Task<object> GetPhaseStepsByTemplateAsync(IEnumerable<ApplicationTypeTemplatePhase> templatePhases, int templateId, int phaseId);

        List<TemplatePhaseStep> AddStepsWithOrdinal(List<TemplatePhaseStep> model, ApplicationTypeTemplatePhase tempPhase);

        Task<List<Step>> GetStepList(int? templatePhaseStepId);
      
        Task<List<Step>> GetPhaseStepList(int? templatePhaseId);

        TemplatePhaseStep GetTemplatePhaseStep(int id);

        int GetStepTypeId(int stepId);

        Task<TemplatePhaseStep> AddPhaseStep(TemplatePhaseStep templatePhaseStep);
        Task<TemplatePhaseStep> UpdatePhaseStep(TemplatePhaseStep templatePhaseStep);

        Task<bool> DeleteStep(int id);

        Task<int> GetNewPhaseOrdinal(int templateId);

        Task<int> GetNewStepOrdinal(int templatePhaseId);

    }
}