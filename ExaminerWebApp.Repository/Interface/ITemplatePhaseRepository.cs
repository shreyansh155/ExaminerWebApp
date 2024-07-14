
using ExaminerWebApp.Repository.DataModels;

namespace ExaminerWebApp.Repository.Interface
{
    public interface ITemplatePhaseRepository : IBaseRepository<ApplicationTypeTemplatePhase>
    {
        Task<IEnumerable<ApplicationTypeTemplatePhase>> PhaseStepsAsync(int templateId, int phaseId);
        Task<IEnumerable<ApplicationTypeTemplatePhase>> PhaseStepsByTemplateAsync(int templateId, int phaseId);

        Object TemplatePhases(int templateId);

        IQueryable<ApplicationTypeTemplatePhase> GetAllTemplates(int templateId);

        ApplicationTypeTemplatePhase AddPhaseWithOrdinal(ApplicationTypeTemplatePhase model);

        Task<bool> UpdateOrdinal(int templateId, int phaseId, int ordinal);
    }
}
