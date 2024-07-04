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
    }
}
