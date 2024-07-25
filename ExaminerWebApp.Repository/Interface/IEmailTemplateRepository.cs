using ExaminerWebApp.Repository.DataModels;

namespace ExaminerWebApp.Repository.Interface
{
    public interface IEmailTemplateRepository : IBaseRepository<EmailTemplate>
    {
        IQueryable<EmailTemplate> GetAll();
        Task<bool> CheckIfExists(int? id, string name);
    }
}