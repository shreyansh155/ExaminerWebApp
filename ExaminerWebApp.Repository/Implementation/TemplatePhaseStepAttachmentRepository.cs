using ExaminerWebApp.Repository.DataContext;
using ExaminerWebApp.Repository.DataModels;
using ExaminerWebApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace ExaminerWebApp.Repository.Implementation
{
    public class TemplatePhaseStepAttachmentRepository : ITemplatePhaseStepAttachmentRepository
    {
        private readonly ApplicationDbContext _context;

        public TemplatePhaseStepAttachmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<TemplatePhaseStepAttachment> Create(TemplatePhaseStepAttachment model)
        {
            var obj = await _context.TemplatePhaseStepAttachments.Where(x => x.IsDeleted != true).OrderByDescending(x => x.Ordinal).FirstOrDefaultAsync();
            if (obj != null)
            {
                model.Ordinal = obj.Ordinal + 1;
            }
            else
            {
                model.Ordinal = 1;
            }

            await _context.TemplatePhaseStepAttachments.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<TemplatePhaseStepAttachment> Update(TemplatePhaseStepAttachment model)
        {
            var obj = _context.TemplatePhaseStepAttachments.Update(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<int> Delete(int id)
        {
            TemplatePhaseStepAttachment obj = await _context.TemplatePhaseStepAttachments.FirstAsync(x => x.Id == id);
            obj.IsDeleted = true;
            await _context.SaveChangesAsync();
            return id;
        }

        public async Task<TemplatePhaseStepAttachment?> GetById(int? id)
        {
            TemplatePhaseStepAttachment obj = await _context.TemplatePhaseStepAttachments.FirstAsync(x => x.Id == id);
            return obj.IsDeleted == true ? null : obj;
        }
        public IQueryable<TemplatePhaseStepAttachment> GetAll(int? tpsId)
        {
            return _context.TemplatePhaseStepAttachments
                  .Where(x => x.TemplatePhaseStepId == tpsId && x.IsDeleted != true)
                  .OrderBy(x => x.Ordinal)
                  .AsQueryable();
        }
        public async Task<List<DocumentFileType>> GetDocumentTypes()
        {
            return await _context.DocumentFileTypes.ToListAsync();
        }
    }
}
