using ExaminerWebApp.Repository.DataContext;
using ExaminerWebApp.Repository.DataModels;
using ExaminerWebApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace ExaminerWebApp.Repository.Implementation
{
    public class TemplatePhaseRepository : ITemplatePhaseRepository
    {
        private readonly ApplicationDbContext _context;

        public TemplatePhaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<ApplicationTypeTemplatePhase>> TemplatePhases(int templateId)
        {
            var resultList = await _context.ApplicationTypeTemplatePhases
                .Where(attp => attp.TemplateId == templateId && attp.IsDeleted != true)
                .Include(attp => attp.Phase)
                .OrderBy(x => x.Ordinal)
                .ToListAsync();

            return resultList.AsQueryable();
        }

        public async Task<IEnumerable<ApplicationTypeTemplatePhase>> PhaseStepsAsync(int templateId, int phaseId)
        {
            var phasesWithStepCounts = await _context.ApplicationTypeTemplatePhases
                .Where(attp => attp.TemplateId == templateId && attp.PhaseId == phaseId && attp.IsDeleted != true)
                .Select(attp => new ApplicationTypeTemplatePhase
                {
                    Id = attp.Id, //template phase Id
                    TemplateId = attp.TemplateId,
                    Ordinal = attp.Ordinal,
                    Phase = attp.Phase,
                })
                .OrderBy(x => x.Ordinal)
                .ToListAsync();

            return phasesWithStepCounts; //also to be used in edit page for template
        }

        public async Task<IEnumerable<ApplicationTypeTemplatePhase>> PhaseStepsByTemplateAsync(int templateId, int phaseId)
        {
            var phasesWithStepCounts = await _context.ApplicationTypeTemplatePhases
                .Where(attp => attp.TemplateId == templateId && attp.PhaseId == phaseId && attp.IsDeleted != true)
                .Select(attp => new ApplicationTypeTemplatePhase
                {
                    Id = attp.Id, //template phase Id
                    TemplateId = attp.TemplateId,
                    Ordinal = attp.Ordinal,
                    Phase = attp.Phase,
                })
                .OrderBy(x => x.Ordinal)
                .ToListAsync();

            return phasesWithStepCounts; //also to be used in edit page for template
        }

        public async Task<ApplicationTypeTemplatePhase> Create(ApplicationTypeTemplatePhase _object)
        {
            await _context.ApplicationTypeTemplatePhases.AddAsync(_object);
            await _context.SaveChangesAsync();
            return _object;
        }

        public async Task<bool> Update(ApplicationTypeTemplatePhase _object)
        {
            _context.ApplicationTypeTemplatePhases.Update(_object);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            ApplicationTypeTemplatePhase templatePhase = await _context.ApplicationTypeTemplatePhases.Where(x => x.Id == id).FirstAsync();
            templatePhase.IsDeleted = true;

            List<TemplatePhaseStep> templatePhaseSteps = await _context.TemplatePhaseSteps.Where(x => x.TemplatePhaseId == templatePhase.Id).ToListAsync();
            foreach (var item in templatePhaseSteps)
            {
                item.IsDeleted = true;
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ApplicationTypeTemplatePhase> GetById(int id)
        {
            return await _context.ApplicationTypeTemplatePhases.Where(x => x.Id == id).FirstAsync();
        }

        public IQueryable<ApplicationTypeTemplatePhase> GetAllTemplates(int templateId)
        {
            return _context.ApplicationTypeTemplatePhases
               .Where(x => x.TemplateId == templateId && x.IsDeleted != true);
        }

        public ApplicationTypeTemplatePhase AddPhaseWithOrdinal(ApplicationTypeTemplatePhase model)
        {
            var allPhases = GetAllTemplates(model.TemplateId)
                .OrderBy(x => x.Ordinal)
                .ToList();

            var upperTemplatePhase = allPhases
                .Where(tp => tp.Ordinal >= model.Ordinal)
                .ToList();

            var lowerTemplatePhase = allPhases
                .Where(tp => tp.Ordinal < model.Ordinal)
                .ToList();

            for (int i = 0; i < upperTemplatePhase.Count; i++)
            {
                if (i == 0 && upperTemplatePhase[0].Ordinal > model.Ordinal)
                {
                    break;
                }
                else
                {
                    if (i == 0)
                    {
                        upperTemplatePhase[i].Ordinal++;
                    }
                    else if (i > 0 && upperTemplatePhase[i - 1].Ordinal == upperTemplatePhase[i].Ordinal)
                    {
                        upperTemplatePhase[i].Ordinal++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            _context.SaveChanges();

            var newPhase = new ApplicationTypeTemplatePhase
            {
                TemplateId = model.TemplateId,
                PhaseId = model.PhaseId,
                Ordinal = model.Ordinal,
                IsDeleted = false,
                StepCount = model.StepCount,
            };

            _context.Add(newPhase);
            _context.SaveChanges();
            return newPhase;
        }

        public async Task<bool> UpdateOrdinal(int templatePhaseId, int ordinal)
        {
            var currentTemplatePhase = await _context.ApplicationTypeTemplatePhases.FirstAsync(x => x.Id == templatePhaseId);

            var allTemplatePhase = GetAllTemplates(currentTemplatePhase.TemplateId);

            currentTemplatePhase.Ordinal = ordinal;

            var upperTemplatePhase = allTemplatePhase
                .Where(tp => tp.Ordinal >= currentTemplatePhase.Ordinal && tp.PhaseId != currentTemplatePhase.PhaseId)
                .OrderBy(comparer => comparer.Ordinal)
                .ToList();

            var lowerTemplatePhase = allTemplatePhase
                .Where(tp => tp.Ordinal < currentTemplatePhase.Ordinal && tp.PhaseId != currentTemplatePhase.PhaseId)
                .OrderBy(comparer => comparer.Ordinal)
                .ToList();

            for (int i = 0; i < upperTemplatePhase.Count; i++)
            {
                if (i == 0 && upperTemplatePhase[0].Ordinal > ordinal)
                {
                    break;
                }
                else
                {
                    if (i == 0)
                    {
                        upperTemplatePhase[i].Ordinal++;
                    }
                    else if (i > 0 && upperTemplatePhase[i - 1].Ordinal == upperTemplatePhase[i].Ordinal)
                    {
                        upperTemplatePhase[i].Ordinal++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            for (int i = lowerTemplatePhase.Count; i >= 0; i--)
            {
                if (i == lowerTemplatePhase.Count - 1 && lowerTemplatePhase[i].Ordinal < ordinal)
                {
                    break;
                }
                else
                {
                    if (i == lowerTemplatePhase.Count - 1)
                    {
                        lowerTemplatePhase[i].Ordinal--;
                    }
                    else if (i < lowerTemplatePhase.Count - 1 && lowerTemplatePhase[i - 1].Ordinal == lowerTemplatePhase[i].Ordinal)
                    {
                        lowerTemplatePhase[i].Ordinal--;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            var obj = await _context.SaveChangesAsync();

            return true;
        }
    }
}