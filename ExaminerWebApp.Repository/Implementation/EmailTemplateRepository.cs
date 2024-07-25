using ExaminerWebApp.Repository.DataContext;
using ExaminerWebApp.Repository.DataModels;
using ExaminerWebApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace ExaminerWebApp.Repository.Implementation
{
    public class EmailTemplateRepository : IEmailTemplateRepository
    {
        private readonly ApplicationDbContext _context;

        public EmailTemplateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<EmailTemplate> GetAll()
        {
            return _context.EmailTemplates.Where(x => x.IsDeleted != true).OrderBy(x => x.Id).AsQueryable();
        }

        public async Task<EmailTemplate> Create(EmailTemplate emailTemplate)
        {
            if (emailTemplate.IsDefault == true)
            {
                List<EmailTemplate> list = await _context.EmailTemplates
                    .Where(x => x.IsDeleted != true)
                    .ToListAsync();

                await UpdateDefaultTemplate(list);
            }
            await _context.EmailTemplates.AddAsync(emailTemplate);
            await _context.SaveChangesAsync();
            return emailTemplate;
        }

        public async Task<EmailTemplate> Update(EmailTemplate emailTemplate)
        {
            if (emailTemplate.IsDefault == true)
            {
                List<EmailTemplate> list = await _context.EmailTemplates
                    .Where(x => x.Id != emailTemplate.Id && x.IsDeleted != true)
                    .ToListAsync();

                await UpdateDefaultTemplate(list);
            }
            _context.EmailTemplates.Update(emailTemplate);
            await _context.SaveChangesAsync();
            return emailTemplate;
        }

        public async Task<int> Delete(int id)
        {
            EmailTemplate emailTemplate = await _context.EmailTemplates.FirstAsync(x => x.Id == id);
            emailTemplate.IsDeleted = true;
            await _context.SaveChangesAsync();
            return id;
        }

        public async Task<EmailTemplate> GetById(int id)
        {
            EmailTemplate emailTemplate= await _context.EmailTemplates.FirstAsync(x => x.Id == id);
            if(emailTemplate.IsDeleted == true)
            {
                return null;
            }
            return emailTemplate;
        }

        public async static Task UpdateDefaultTemplate(List<EmailTemplate> emailTemplates)
        {
            foreach (var item in emailTemplates)
            {
                await SetIsDefaultAsync(item, false);
            }
        }

        public async Task<bool> CheckIfExists(int? id, string name)
        {
            if (id != null)
            {
                return await _context.EmailTemplates
                    .Where(x => x.Id != id && x.Name.ToLower() == name.ToLower() && x.IsDeleted != true)
                    .AnyAsync();
            }
            return await _context.EmailTemplates
                .Where(x => x.Name.ToLower() == name.ToLower())
                .AnyAsync();
        }

        private static Task SetIsDefaultAsync(EmailTemplate item, bool isDefault)
        {
            item.IsDefault = isDefault;
            return Task.CompletedTask;
        }
    }
}