using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface
{
    public interface IApplicationTypeService
    {
        IQueryable<ApplicationTypeTemplate> GetAll();
        //ApplicationTypeTemplate GetById(int id);
        
        //Task<ApplicationTypeTemplate> Add(ApplicationTypeTemplate model);
        
        //bool Delete(int id);
        
        //bool Update(Applicant model);
    }
}
