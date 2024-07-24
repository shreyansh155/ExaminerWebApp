using AutoMapper;
using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Repository.Interface;
using ExaminerWebApp.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace ExaminerWebApp.Service.Implementation
{
    public class StepService : BaseService, IStepService
    {
        public readonly IStepRepository _stepRepository;

        public readonly IMapper _mapper;

        public StepService(IStepRepository stepRepository, IMapper mapper)
        {
            _stepRepository = stepRepository;
            _mapper = mapper;
        }

        public async Task<PaginationSet<Step>> GetAll(int phaseId, PaginationSet<Step> pager)
        {
            IQueryable<Repository.DataModels.Step> step =
                _stepRepository.GetAllSteps(phaseId)
                .Include(x => x.StepType)
                .AsQueryable();
            IQueryable<Step> steps = _mapper.ProjectTo<Step>(step);

            //filtering as per field names
            if (pager.Filter != null && pager.Filter.Filters != null && pager.Filter.Filters.Count > 0)
            {
                foreach (var filter in pager.Filter.Filters)
                {
                    step = ApplyFilter(step, filter);
                }
            }

            //sorting ascending descending
            if (pager.Sort != null && pager.Sort.Count > 0)
            {
                foreach (var sort in pager.Sort)
                {
                    step = ApplySorting(step, sort);
                }
            }
            pager.TotalCount = await steps.CountAsync();
            pager.Items = await steps.Skip(pager.Skip).Take(pager.Take).ToListAsync();
            return pager;
        }

        public async Task<Step> GetStepById(int id)
        {
            Repository.DataModels.Step obj = await _stepRepository.GetById(id);
            Step step = _mapper.Map<Step>(obj);
            return step;
        }

        public async Task<Step> CreateStep(Step model)
        {
            model.CreatedDate = DateTime.UtcNow;
            model.CreatedBy = "1";
            Repository.DataModels.Step step = _mapper.Map<Repository.DataModels.Step>(model);
            await _stepRepository.Create(step);
            return model;
        }

        public async Task<bool> DeleteStep(int id)
        {
            return await _stepRepository.Delete(id);
        }

        public async Task<bool> UpdateStep(Step model)
        {
            model.ModifiedDate = DateTime.UtcNow;
            model.ModifiedBy = "2";
            Repository.DataModels.Step step = _mapper.Map<Repository.DataModels.Step>(model);
            await _stepRepository.Update(step);
            return true;
        }

        public async Task<List<StepType>> GetStepTypeList()
        {
            List<Repository.DataModels.StepType> stepType = await _stepRepository.GetStepTypeList();
            List<StepType> list = _mapper.Map<List<StepType>>(stepType);
            return list;
        }

        public async Task<IQueryable<Step>> GetStepByPhaseId(int phaseId)
        {
            IQueryable<Repository.DataModels.Step> steps = await Task.Run(() => _stepRepository.GetAllSteps(phaseId));
            IQueryable<Step> obj = _mapper.ProjectTo<Step>(steps);
            return obj;
        }

        public async Task<bool> CheckIfStepExists(Step model)
        {
            Repository.DataModels.Step step = _mapper.Map<Repository.DataModels.Step>(model);
            return await _stepRepository.CheckIfStepExists(step);
        }

        public async Task<bool> CheckIfEditStepExists(Step model)
        {
            Repository.DataModels.Step step = _mapper.Map<Repository.DataModels.Step>(model);
            return await _stepRepository.CheckIfEditStepExists(step);
        }
    }
}