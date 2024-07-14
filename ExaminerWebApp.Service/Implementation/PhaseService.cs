using AutoMapper;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Repository.Interface;
using ExaminerWebApp.Service.Interface;

namespace ExaminerWebApp.Service.Implementation
{
    public class PhaseService : IPhaseService
    {
        public readonly IPhaseRepository _phaseRepository;

        public readonly IMapper _mapper;

        public PhaseService(IPhaseRepository phaseRepository, IMapper mapper)
        {
            _mapper = mapper;
            _phaseRepository = phaseRepository;
        }

        public IQueryable<Phase> GetAll()
        {
            IQueryable<Repository.DataModels.Phase> list = _phaseRepository.GetAll();
            IQueryable<Phase> phases = _mapper.ProjectTo<Phase>(list);
            return phases;
        }

        public async Task<Phase> GetPhaseById(int id)
        {
            Repository.DataModels.Phase dbphase = await _phaseRepository.GetById(id);
            Phase phase = _mapper.Map<Phase>(dbphase);
            return phase;
        }

        public async Task<Phase> CreatePhase(Phase model)
        {
            Repository.DataModels.Phase phase = _mapper.Map<Repository.DataModels.Phase>(model);
            await _phaseRepository.Create(phase);
            return model;
        }

        public async Task<bool> DeletePhase(int id)
        {
            await _phaseRepository.Delete(id);
            return true;
        }

        public async Task<bool> UpdatePhase(Phase model)
        {
            Repository.DataModels.Phase phase = _mapper.Map<Repository.DataModels.Phase>(model);
            await Task.Run(() => _phaseRepository.Update(phase));
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
