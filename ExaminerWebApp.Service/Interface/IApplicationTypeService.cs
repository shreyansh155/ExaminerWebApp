using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface
{
    public interface IApplicationTypeService
    {
        Task<PaginationSet<ApplicationTypeTemplate>> GetAll(PaginationSet<ApplicationTypeTemplate> pager);

        Task<ApplicationTypeTemplate> GetById(int id);

        Task<ApplicationTypeTemplate> Add(ApplicationTypeTemplate model);

        bool ApplicationTemplateExists(string applicationName);

        Task<bool> EditApplicationTemplateExists(int? id, string applicationName);

        Task<bool> Delete(int id);

        Task<bool> DeleteTemplate(int id);

        Task<bool> Update(ApplicationTypeTemplate model);

        Task<object> GetPhaseStepsByTemplateAsync(int templateId, int phaseId);

        Task<object> GetPhaseStepsAsync(int templateId, int phaseId);

        Task<PaginationSet<object>> GetPhaseByTemplate(int templateId, PaginationSet<object> pager);

        IQueryable<Phase> PhaseList(int templateId);

        Task<List<Step>> StepList(int? templatePhaseStepId);

        Task<List<Step>> PhaseStepList(int? templatePhaseId);
    }
}