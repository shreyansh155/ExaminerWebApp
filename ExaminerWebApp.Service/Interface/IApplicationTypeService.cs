using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface
{
    public interface IApplicationTypeService
    {
        IQueryable<ApplicationTypeTemplate> GetAll(string search);

        Task<ApplicationTypeTemplate> GetById(int id);

        Task<ApplicationTypeTemplate> Add(ApplicationTypeTemplate model);

        bool ApplicationTemplateExists(string applicationName);

        bool Delete(int id);

        bool Update(ApplicationTypeTemplate model);
        Task<object> GetPhaseStepsByTemplateAsync(int templateId, int phaseId);

        Task<object> GetPhaseStepsAsync(int templateId, int phaseId);

        object GetPhaseByTemplate(int templateId);

        IQueryable<Phase> PhaseList(int templateId);
    }
}
