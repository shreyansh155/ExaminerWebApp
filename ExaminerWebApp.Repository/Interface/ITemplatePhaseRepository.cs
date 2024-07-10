
using ExaminerWebApp.Repository.DataModels;

namespace ExaminerWebApp.Repository.Interface
{
    public interface ITemplatePhaseRepository : IBaseRepository<ApplicationTypeTemplatePhase>
    {
        Object PhaseSteps(int templateId, int phaseId);
      
        Object TemplatePhases(int templateId);
        
        IQueryable<ApplicationTypeTemplatePhase> GetAllTemplates(int templateId);
        
        void AddPhaseWithOrdinal(ApplicationTypeTemplatePhase model);

        bool UpdateOrdinal(int templateId, int phaseId, int ordinal);
    }
}
