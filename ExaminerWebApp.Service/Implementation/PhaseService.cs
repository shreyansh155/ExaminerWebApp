using AutoMapper;
using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Repository.Interface;
using ExaminerWebApp.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;

namespace ExaminerWebApp.Service.Implementation
{
    public class PhaseService : BaseService, IPhaseService
    {
        public readonly IPhaseRepository _phaseRepository;

        public readonly IMapper _mapper;

        public PhaseService(IPhaseRepository phaseRepository, IMapper mapper)
        {
            _mapper = mapper;
            _phaseRepository = phaseRepository;
        }

        public async Task<PaginationSet<Phase>> GetAll(PaginationSet<Phase> pager)
        {
            IQueryable<Repository.DataModels.Phase> list = _phaseRepository.GetAll();

            //filtering as per field names
            if (pager.Filter != null && pager.Filter.Filters != null && pager.Filter.Filters.Count > 0)
            {
                foreach (var filter in pager.Filter.Filters)
                {
                    list = ApplyFilter(list, filter);
                }
            }

            //sorting: ascending descending
            if (pager.Sort != null && pager.Sort.Count > 0)
            {
                foreach (var sort in pager.Sort)
                {
                    list = ApplySorting(list, sort);
                }
            }

            IQueryable<Phase> phases = _mapper.ProjectTo<Phase>(list);

            pager.Items = await phases.Skip(pager.Skip).Take(pager.Take).ToListAsync();

            pager.TotalCount = await phases.CountAsync();

            return pager;
        }

        public async Task<Phase> GetPhaseById(int id)
        {
            Repository.DataModels.Phase dbphase = await _phaseRepository.GetById(id);
            Phase phase = _mapper.Map<Phase>(dbphase);
            return phase;
        }

        public async Task<Phase> CreatePhase(Phase model)
        {
            model.CreatedDate = DateTime.UtcNow;
            model.CreatedBy = "System"; //change it to int id = 1
            Repository.DataModels.Phase phase = _mapper.Map<Repository.DataModels.Phase>(model);
            await _phaseRepository.Create(phase);
            return model;
        }

        public async Task<bool> DeletePhase(int id)
        {
            return await _phaseRepository.Delete(id);
        }

        public async Task<bool> UpdatePhase(Phase model)
        {
            Repository.DataModels.Phase phase = _mapper.Map<Repository.DataModels.Phase>(model);
            await _phaseRepository.Update(phase);
            return true;
        }

        public Task<bool> CheckIfPhaseExists(string phaseName)
        {
            return _phaseRepository.CheckIfPhaseExists(phaseName);
        }

        public Task<bool> CheckPhaseOnUpdateExists(string phaseName, int? phaseId)
        {
            return _phaseRepository.CheckPhaseOnUpdateExists(phaseName, phaseId);
        }
    }
}