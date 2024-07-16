using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface
{
    public interface IApplicationTypeService
    {
        IQueryable<ApplicationTypeTemplate> GetAll(string search);

        Task<ApplicationTypeTemplate> GetById(int id);

        Task<ApplicationTypeTemplate> Add(ApplicationTypeTemplate model);

        bool ApplicationTemplateExists(string applicationName);
       
        Task<bool> EditApplicationTemplateExists(int? id, string applicationName);

        Task<bool> Delete(int id);
        Task<bool> DeleteTemplate(int id);

        bool Update(ApplicationTypeTemplate model);
      
        Task<object> GetPhaseStepsByTemplateAsync(int templateId, int phaseId);

        Task<object> GetPhaseStepsAsync(int templateId, int phaseId);

        object GetPhaseByTemplate(int templateId);

        IQueryable<Phase> PhaseList(int templateId);
      
        Task<List<Step>> StepList(int? templatePhaseStepId);
        
        Task<List<Step>> PhaseStepList(int? templatePhaseId);
    }
}