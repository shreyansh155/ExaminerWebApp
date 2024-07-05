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
        public Phase GetPhaseById(int id)
        {
            Repository.DataModels.Phase dbphase = _phaseRepository.GetById(id);
            Phase phase = _mapper.Map<Phase>(dbphase);
            return phase;
        }
        public async Task<Phase> CreatePhase(Phase model)
        {
            Repository.DataModels.Phase phase = _mapper.Map<Repository.DataModels.Phase>(model);
            var obj = _phaseRepository.Create(phase);
            return model;
        }
        public bool DeletePhase(int id)
        {
            return true;
        }
        public bool UpdatePhase(Phase model)
        {
            return true;
        }
    }
}
