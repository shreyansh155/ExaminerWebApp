using ExaminerWebApp.Repository.DataModels;

namespace ExaminerWebApp.Repository.Interface
{
    public interface IApplicationTypeTemplateRepository : IBaseRepository<ApplicationTypeTemplate>
    {
        bool ApplicationTemplateExists(string name);
        Task<bool> EditApplicationTemplateExists(int? id, string name);
        IQueryable<ApplicationTypeTemplate> GetAll(string s);
    }
}
