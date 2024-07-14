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

        public Object TemplatePhases(int templateId)
        {
            using (_context)
            {
                var phasesWithStepCounts = _context.ApplicationTypeTemplatePhases
                    .Where(attp => attp.TemplateId == templateId && attp.IsDeleted != true)
                    .Select(attp => new
                    {
                        TemplatePhaseId = attp.Id,
                        attp.Ordinal,
                        attp.Phase,
                        StepCount = attp.TemplatePhaseSteps.Count(),
                    })
                    .OrderBy(x => x.Ordinal)
                    .ToList();

                return phasesWithStepCounts;
            }
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

        public void Update(ApplicationTypeTemplatePhase _object) { }

        public async Task<bool> Delete(int id)
        {
            ApplicationTypeTemplatePhase templatePhase = await _context.ApplicationTypeTemplatePhases.Where(x => x.Id == id).FirstAsync();
            templatePhase.IsDeleted = true;
            _context.ApplicationTypeTemplatePhases.Update(templatePhase);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ApplicationTypeTemplatePhase> GetById(int id)
        {
            return await _context.ApplicationTypeTemplatePhases.Where(x => x.Id == id).FirstAsync();
        }

        public IQueryable<ApplicationTypeTemplatePhase> GetAllTemplates(int templateId)
        {
            return _context.ApplicationTypeTemplatePhases.Where(x => x.TemplateId == templateId).AsQueryable();
        }

        public ApplicationTypeTemplatePhase AddPhaseWithOrdinal(ApplicationTypeTemplatePhase model)
        {
            var allPhases = GetAllTemplates(model.TemplateId)
                .OrderBy(x => x.Ordinal)
                .ToList();

            var upperPhase = allPhases
                .Where(tp => tp.Ordinal >= model.Ordinal)
                .ToList();

            var lowerPhase = allPhases
                .Where(tp => tp.Ordinal < model.Ordinal)
                .ToList();

            for (int i = 0; i < upperPhase.Count; i++)
            {
                if (i == 0 && upperPhase[0].Ordinal > model.Ordinal)
                {
                    break;
                }
                else
                {
                    if (i == 0)
                    {
                        upperPhase[i].Ordinal++;
                    }
                    else if (i > 0 && upperPhase[i - 1].Ordinal == upperPhase[i].Ordinal)
                    {
                        upperPhase[i].Ordinal++;
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
                IsDeleted = false
            };

            _context.Add(newPhase);
            _context.SaveChanges();
            return newPhase;
        }

        public async Task<bool> UpdateOrdinal(int templateId, int phaseId, int ordinal)
        {
            var allPhases = GetAllTemplates(templateId).OrderBy(x => x.Ordinal).ToList();

            var currentPhase = allPhases.First(tp => tp.PhaseId == phaseId);
            currentPhase.Ordinal = ordinal;

            var upperPhase = allPhases.Where(tp => tp.Ordinal >= currentPhase.Ordinal && tp.PhaseId != phaseId).ToList();
            var lowerPhase = allPhases.Where(tp => tp.Ordinal < currentPhase.Ordinal && tp.PhaseId != phaseId).ToList();

            for (int i = 0; i < upperPhase.Count; i++)
            {
                if (i == 0 && upperPhase[0].Ordinal > ordinal)
                {
                    break;
                }
                else
                {
                    if (i == 0)
                    {
                        upperPhase[i].Ordinal++;
                    }
                    else if (i > 0 && upperPhase[i - 1].Ordinal == upperPhase[i].Ordinal)
                    {
                        upperPhase[i].Ordinal++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            for (int i = lowerPhase.Count; i >= 0; i--)
            {
                if (i == lowerPhase.Count - 1 && lowerPhase[i].Ordinal < ordinal)
                {
                    break;
                }
                else
                {
                    if (i == lowerPhase.Count - 1)
                    {
                        lowerPhase[i].Ordinal--;
                    }
                    else if (i < lowerPhase.Count - 1 && lowerPhase[i - 1].Ordinal == lowerPhase[i].Ordinal)
                    {
                        lowerPhase[i].Ordinal--;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

