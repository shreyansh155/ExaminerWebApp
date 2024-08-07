﻿using ExaminerWebApp.Repository.DataContext;
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
            var all = GetAll(model.TemplatePhaseStepId)
                                    .OrderBy(x => x.Ordinal)
                                    .ToList();

            var upper = all
                .Where(tp => tp.Ordinal >= model.Ordinal)
                .ToList();

            var lower = all
                .Where(tp => tp.Ordinal < model.Ordinal)
                .ToList();

            for (int i = 0; i < upper.Count; i++)
            {
                if (i == 0 && upper[0].Ordinal > model.Ordinal)
                {
                    break;
                }
                else
                {
                    if (i == 0)
                    {
                        upper[i].Ordinal++;
                    }
                    else if (i > 0 && upper[i - 1].Ordinal == upper[i].Ordinal)
                    {
                        upper[i].Ordinal++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<TemplatePhaseStepDocumentProof> Update(TemplatePhaseStepDocumentProof model)
        {
            _context.TemplatePhaseStepDocumentProofs.Update(model);
            var all = GetAll(model.TemplatePhaseStepId)
                            .Where(x => x.Id != model.Id)
                            .OrderBy(x => x.Ordinal)
                            .ToList();

            var upper = all
                .Where(tp => tp.Ordinal >= model.Ordinal)
                .ToList();

            var lower = all
                .Where(tp => tp.Ordinal < model.Ordinal)
                .ToList();

            for (int i = 0; i < upper.Count; i++)
            {
                if (i == 0 && upper[0].Ordinal > model.Ordinal)
                {
                    break;
                }
                else
                {
                    if (i == 0)
                    {
                        upper[i].Ordinal++;
                    }
                    else if (i > 0 && upper[i - 1].Ordinal == upper[i].Ordinal)
                    {
                        upper[i].Ordinal++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
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