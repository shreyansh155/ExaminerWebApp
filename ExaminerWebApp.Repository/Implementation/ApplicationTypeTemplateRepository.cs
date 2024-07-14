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
     
        public IQueryable<ApplicationTypeTemplate> GetAll(string s)
        {
            return _context.ApplicationTypeTemplates.Where(x => x.IsDeleted != true && (string.IsNullOrEmpty(s) || x.Name.ToLower().Contains(s.ToLower()) || x.Description.ToLower().Contains(s.ToLower()))).OrderBy(x => x.Id).AsQueryable();
        }

        public bool ApplicationTemplateExists(string name)
        {
            return _context.ApplicationTypeTemplates.Where(x => x.Name == name && x.IsDeleted != true).Any();
        }

        public async Task<ApplicationTypeTemplate> Create(ApplicationTypeTemplate model)
        {
            await _context.ApplicationTypeTemplates.AddAsync(model);
            _context.SaveChanges();
            return model;
        }

        public void Update(ApplicationTypeTemplate _object)
        {
            _context.ApplicationTypeTemplates.Update(_object);
            _context.SaveChanges();
        }

        public async Task<bool> Delete(int id)
        {
            var applicationType = await _context.ApplicationTypeTemplates.FirstAsync(x => x.Id == id);
            applicationType.IsDeleted = true;
            _context.ApplicationTypeTemplates.Update(applicationType);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ApplicationTypeTemplate> GetById(int id)
        {
            return await _context.ApplicationTypeTemplates.Where(x => x.Id == id).FirstAsync();

        }
    }
}
