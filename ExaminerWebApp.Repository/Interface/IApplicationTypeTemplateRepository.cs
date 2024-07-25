using ExaminerWebApp.Repository.DataModels;

namespace ExaminerWebApp.Repository.Interface
{
    public interface IApplicationTypeTemplateRepository : IBaseRepository<ApplicationTypeTemplate>
    {      
        Task<bool> CheckIfExists(int? id, string name);
        
        IQueryable<ApplicationTypeTemplate> GetAll();
    }
}