using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface
{
    public interface ITemplatePhaseService
    {
        ApplicationTypeTemplatePhase AddTemplatePhaseStep(ApplicationTypeTemplatePhase model);

        Task<bool> UpdateOrdinal(int templateId, int phaseId, int ordinal);
    }
}
