using ExaminerWebApp.Repository.DataContext;
using ExaminerWebApp.Repository.DataModels;
using ExaminerWebApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace ExaminerWebApp.Repository.Implementation
{

    public class TemplatePhaseStepDocumentProofRepository : ITemplatePhaseStepDocumentProofRepository
    {
        private readonly ApplicationDbContext _context;

        public TemplatePhaseStepDocumentProofRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<TemplatePhaseStepDocumentProof> GetAll(int? tpsId)
        {
            return _context.TemplatePhaseStepDocumentProofs
                .Where(x => x.TemplatePhaseStepId == tpsId && x.IsDeleted != true)
                .OrderBy(x => x.Ordinal)
                .AsQueryable();
        }

        public async Task<TemplatePhaseStepDocumentProof> Create(TemplatePhaseStepDocumentProof model)
        {
            await _context.TemplatePhaseStepDocumentProofs.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<TemplatePhaseStepDocumentProof> Update(TemplatePhaseStepDocumentProof model)
        {
            _context.TemplatePhaseStepDocumentProofs.Update(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<int> Delete(int id)
        {
            TemplatePhaseStepDocumentProof obj = await _context.TemplatePhaseStepDocumentProofs.FirstAsync(x => x.Id == id);
            obj.IsDeleted = true;
            await _context.SaveChangesAsync();
            return id;
        }

        public async Task<TemplatePhaseStepDocumentProof?> GetById(int? id)
        {
            TemplatePhaseStepDocumentProof obj = await _context.TemplatePhaseStepDocumentProofs.FirstAsync(x => x.Id == id);
            return obj.IsDeleted == true ? null : obj;
        }
    }
}