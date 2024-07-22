using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface
{
    public interface ITemplatePhaseService
    {
        Task<ApplicationTypeTemplatePhase> AddTemplatePhase(ApplicationTypeTemplatePhase model);

        Task<bool> UpdateOrdinal(int templatePhaseId, int ordinal);

        TemplatePhaseStep GetTemplatePhaseStep(int id);

        public int GetStepTypeId(int stepId);

        Task<TemplatePhaseStep> AddTemplatePhaseStep(TemplatePhaseStep templatePhaseStep);
        Task<TemplatePhaseStep> EditTemplatePhaseStep(TemplatePhaseStep templatePhaseStep);

        Task<bool> DeleteStep(int id);

        Task<int> GetNewPhaseOrdinal(int templateId);

        Task<int> GetNewStepOrdinal(int templatePhaseId);
    }
}