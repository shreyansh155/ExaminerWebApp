using ExaminerWebApp.Repository.DataContext;
using ExaminerWebApp.Repository.DataModels;
using ExaminerWebApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace ExaminerWebApp.Repository.Implementation
{
    public class ApplicationTypeTemplateRepository : IApplicationTypeTemplateRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationTypeTemplateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<ApplicationTypeTemplate> GetAll()
        {
            return _context.ApplicationTypeTemplates.Where(x => x.IsDeleted != true).OrderBy(x => x.Id).AsQueryable();
        }

       

        public async Task<bool> CheckIfExists(int? id, string name)
        {
            if (id != null)
            {
                return await _context.ApplicationTypeTemplates.Where(x => x.Id != id && x.Name == name && x.IsDeleted != true).AnyAsync();
            }
            return await _context.ApplicationTypeTemplates.Where(x => x.Name == name && x.IsDeleted != true).AnyAsync();
        }

        public async Task<ApplicationTypeTemplate> Create(ApplicationTypeTemplate model)
        {
            await _context.ApplicationTypeTemplates.AddAsync(model);
            _context.SaveChanges();
            return model;
        }

        public async Task<ApplicationTypeTemplate> Update(ApplicationTypeTemplate _object)
        {
            _context.ApplicationTypeTemplates.Update(_object);
            await _context.SaveChangesAsync();
            return _object;
        }

        public async Task<int> Delete(int id)
        {
            var applicationType = await _context.ApplicationTypeTemplates.FirstAsync(x => x.Id == id);
            applicationType.IsDeleted = true;
            await _context.SaveChangesAsync();
            return id;
        }

        public async Task<ApplicationTypeTemplate> GetById(int id)
        {
            return await _context.ApplicationTypeTemplates.Where(x => x.Id == id).FirstAsync();

        }
    }
}