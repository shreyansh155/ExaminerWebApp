using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface
{
    public interface ITemplatePhaseService
    {
        ApplicationTypeTemplatePhase AddTemplatePhaseStep(ApplicationTypeTemplatePhase model);

        Task<bool> UpdateOrdinal(int templatePhaseId, int ordinal);

        TemplatePhaseStep GetTemplatePhaseStep(int id);

        public int GetStepTypeId(int stepId);

        Task<TemplatePhaseStep> EditTemplatePhaseStep(TemplatePhaseStep templatePhaseStep);

        Task<bool> DeleteStep(int id);
    }
}
