using AutoMapper;
using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Repository.Interface;
using ExaminerWebApp.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace ExaminerWebApp.Service.Implementation
{
    public class ApplicationTypeService : BaseService<Repository.DataModels.ApplicationTypeTemplate>, IApplicationTypeService
    {
        private readonly IMapper _mapper;
        private readonly IApplicationTypeTemplateRepository _applicationTypeRepository;
        private readonly IPhaseRepository _phaseRepository;
        private readonly ITemplatePhaseRepository _templatePhaseRepository;
        private readonly IPhaseStepRepository _phaseStepRepository;

        public ApplicationTypeService(IMapper mapper, IApplicationTypeTemplateRepository applicationTypeRepository, IPhaseRepository phaseRepository, ITemplatePhaseRepository templatePhaseRepository, IPhaseStepRepository phaseStepRepository)
        {
            _phaseRepository = phaseRepository;
            _applicationTypeRepository = applicationTypeRepository;
            _mapper = mapper;
            _phaseStepRepository = phaseStepRepository;
            _templatePhaseRepository = templatePhaseRepository;
        }

        public async Task<PaginationSet<ApplicationTypeTemplate>> GetAll(PaginationSet<ApplicationTypeTemplate> pager)
        {
            var list = _applicationTypeRepository.GetAll().AsQueryable();
            if (pager.Filter != null && pager.Filter.Filters != null && pager.Filter.Filters.Count > 0)
            {
                foreach (var filter in pager.Filter.Filters)
                {
                    if (filter.Field != "id")
                    {
                        list = ApplyFilter(list, filter);
                    }
                }
            }

            //sorting ascending descending
            if (pager.Sort != null && pager.Sort.Count > 0)
            {
                foreach (var sort in pager.Sort)
                {
                    list = ApplySorting(list, sort);
                }
            }
            var obj = _mapper.ProjectTo<ApplicationTypeTemplate>(list);

            pager.Items = await obj.Skip(pager.Skip).Take(pager.Take).ToListAsync();

            pager.TotalCount = await obj.CountAsync();

            return pager;
        }

        public async Task<ApplicationTypeTemplate> GetById(int id)
        {
            Repository.DataModels.ApplicationTypeTemplate obj = await _applicationTypeRepository.GetById(id);
            var result = _mapper.Map<ApplicationTypeTemplate>(obj);
            return result;
        }

        public async Task<ApplicationTypeTemplate> Add(ApplicationTypeTemplate model)
        {
            Repository.DataModels.ApplicationTypeTemplate obj = _mapper.Map<Repository.DataModels.ApplicationTypeTemplate>(model);
            await _applicationTypeRepository.Create(obj);
            return model;
        }

        public bool ApplicationTemplateExists(string applicationName)
        {
            return _applicationTypeRepository.ApplicationTemplateExists(applicationName);
        }

        public async Task<bool> EditApplicationTemplateExists(int? id, string applicationName)
        {
            return await _applicationTypeRepository.EditApplicationTemplateExists(id, applicationName);
        }

        public async Task<bool> Delete(int id)
        {
            return await _templatePhaseRepository.Delete(id);
        }

        public async Task<bool> DeleteTemplate(int id)
        {
            return await _applicationTypeRepository.Delete(id);
        }

        public async Task<bool> Update(ApplicationTypeTemplate model)
        {
            Repository.DataModels.ApplicationTypeTemplate obj = _mapper.Map<Repository.DataModels.ApplicationTypeTemplate>(model);

            await _applicationTypeRepository.Update(obj);

            return true;
        }

        public IQueryable<Phase> PhaseList(int templateId)
        {
            var templatePhaseIds = _templatePhaseRepository.GetAllTemplates(templateId)
                .Select(tp => tp.PhaseId)
                .ToList();  

            IQueryable<Repository.DataModels.Phase> filteredPhases = _phaseRepository
                .GetAll()
                .Where(p => !templatePhaseIds.Contains(p.Id));

            IQueryable<Phase> list = _mapper.ProjectTo<Phase>(filteredPhases);

            return list;
        }

        public async Task<PaginationSet<object>> GetPhaseByTemplate(int templateId, PaginationSet<object> pager)
        {
            IQueryable<Repository.DataModels.ApplicationTypeTemplatePhase> templatePhases = await _templatePhaseRepository.TemplatePhases(templateId);

            IQueryable<ApplicationTypeTemplatePhase> templatePhaseList = _mapper.Map<List<ApplicationTypeTemplatePhase>>(templatePhases).AsQueryable();

            if (pager.Sort != null && pager.Sort.Count > 0)
            {
                foreach (var sort in pager.Sort)
                {
                    templatePhaseList = ApplySortings(templatePhaseList, sort);
                }
            }
            pager.Items = await templatePhaseList.Skip(pager.Skip).Take(pager.Take).ToDynamicListAsync();

            pager.TotalCount = templatePhaseList.Count();

            return pager;
        }

        public async Task<object> GetPhaseStepsByTemplateAsync(int templateId, int phaseId)
        {
            IEnumerable<Repository.DataModels.ApplicationTypeTemplatePhase> templatePhases = await _templatePhaseRepository.PhaseStepsByTemplateAsync(templateId, phaseId);

            var phaseSteps = await _phaseStepRepository.GetPhaseStepsByTemplateAsync(templatePhases, templateId, phaseId);

            return phaseSteps;
        }

        public async Task<object> GetPhaseStepsAsync(int templateId, int phaseId)
        {
            IEnumerable<Repository.DataModels.ApplicationTypeTemplatePhase> templatePhases = await _templatePhaseRepository.PhaseStepsAsync(templateId, phaseId);

            var phaseSteps = await _phaseStepRepository.GetPhaseStepsAsync(templatePhases, templateId, phaseId);

            return phaseSteps;
        }

        public async Task<List<Step>> StepList(int? templatePhaseStepId)
        {
            List<Repository.DataModels.Step> steps = await _phaseStepRepository.GetStepList(templatePhaseStepId);
            List<Step> step = _mapper.Map<List<Step>>(steps);
            return step;
        }

        public async Task<List<Step>> PhaseStepList(int? templatePhaseId)
        {
            List<Repository.DataModels.Step> steps = await _phaseStepRepository.GetPhaseStepList(templatePhaseId);
            List<Step> step = _mapper.Map<List<Step>>(steps);
            return step;
        }

        protected static IQueryable<ApplicationTypeTemplatePhase> ApplySortings(IQueryable<ApplicationTypeTemplatePhase> query, GridSort sort)
        {
            var parameter = Expression.Parameter(typeof(ApplicationTypeTemplatePhase), "x");
            var property = Expression.Property(parameter, sort.Field);
            var lambda = Expression.Lambda(property, parameter);

            string methodName = sort.Dir == "asc" ? "OrderBy" : "OrderByDescending";
            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { query.ElementType, property.Type },
                query.Expression,
                Expression.Quote(lambda)
            );

            return query.Provider.CreateQuery<ApplicationTypeTemplatePhase>(resultExpression);
        }
    }
}