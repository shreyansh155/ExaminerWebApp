using AutoMapper;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Repository.Interface;
using ExaminerWebApp.Service.Interface;

namespace ExaminerWebApp.Service.Implementation
{
    public class StepService : IStepService
    {
        public readonly IStepRepository _stepRepository;
        public readonly IMapper _mapper;
        public StepService(IStepRepository stepRepository, IMapper mapper)
        {
            _stepRepository = stepRepository;
            _mapper = mapper;
        }
        public IQueryable<Step> GetAll(int phaseId)
        {
            IQueryable<Repository.DataModels.Step> step = _stepRepository.GetAllSteps(phaseId);
            IQueryable<Step> steps = _mapper.ProjectTo<Step>(step);
            return steps;
        }
        public Step GetStepById(int id)
        {
            Repository.DataModels.Step obj = _stepRepository.GetById(id);
            Step step = _mapper.Map<Step>(obj);
            return step;
        }
        public async Task<Step> CreateStep(Step model)
        {
            Repository.DataModels.Step step = _mapper.Map<Repository.DataModels.Step>(model);
            await _stepRepository.Create(step);
            return model;
        }
        public bool DeleteStep(int id)
        {
            try
            {
                _stepRepository.Delete(id);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateStep(Step model)
        {
            try
            {
                Repository.DataModels.Step step = _mapper.Map<Repository.DataModels.Step>(model);
                _stepRepository.Update(step);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
