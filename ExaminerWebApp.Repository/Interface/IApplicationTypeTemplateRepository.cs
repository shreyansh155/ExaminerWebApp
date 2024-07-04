using ExaminerWebApp.Repository.DataModels;

namespace ExaminerWebApp.Repository.Interface
{
    public interface IApplicationTypeTemplateRepository : IBaseRepository<ApplicationTypeTemplate>
    {
        public bool ApplicationTemplateExists(string name);
        public IQueryable<ApplicationTypeTemplate> GetAll(string s);
    }
}
