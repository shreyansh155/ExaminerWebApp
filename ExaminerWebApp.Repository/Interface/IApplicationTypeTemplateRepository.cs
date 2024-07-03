using ExaminerWebApp.Repository.DataModels;

namespace ExaminerWebApp.Repository.Interface
{
    public interface IApplicationTypeTemplateRepository /*: IBaseRepository<ApplicationTypeTemplate>*/
    {
        public IQueryable<ApplicationTypeTemplate> GetAll();
    }
}
