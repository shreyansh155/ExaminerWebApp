using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface
{
    public interface ITemplatePhaseService
    {
        ApplicationTypeTemplatePhase AddTemplatePhase(ApplicationTypeTemplatePhase model);

        bool UpdateOrdinal(int templateId, int phaseId, int ordinal);
    }
}
