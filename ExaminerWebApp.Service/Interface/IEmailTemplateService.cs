using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface
{
    public interface IEmailTemplateService
    {
        Task<PaginationSet<EmailTemplate>> GetAll(PaginationSet<EmailTemplate> pager);

        Task<EmailTemplate> Create(EmailTemplate model);
        
        Task<EmailTemplate> GetEmailTemplate(int id);

        Task<EmailTemplate> Update(EmailTemplate model);

        Task<int> Delete(int id);
       
        Task<bool> CheckIfExists(int? id, string name);
    }
}