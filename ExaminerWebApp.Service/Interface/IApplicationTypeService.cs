using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface
{
    public interface IApplicationTypeService
    {
        IQueryable<ApplicationTypeTemplate> GetAll(string search);

        ApplicationTypeTemplate GetById(int id);

        Task<ApplicationTypeTemplate> Add(ApplicationTypeTemplate model);

        bool ApplicationTemplateExists(string applicationName);

        bool Delete(int id);

        bool Update(ApplicationTypeTemplate model);
        object GetPhaseStepsByTemplate(int templateId, int phaseId);

        object GetPhaseByTemplate(int templateId);

        IQueryable<Phase> PhaseList(int templateId);
    }
}
