using AutoMapper;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Repository.Interface;
using ExaminerWebApp.Service.Interface;

namespace ExaminerWebApp.Service.Implementation
{
    public class ExaminerTypeService : IExaminerTypeService
    {
        private readonly IExaminerTypeRepository _examinerTypeRepository;
        private readonly IMapper _mapper;

        public ExaminerTypeService(IExaminerTypeRepository examinerTypeRepository, IMapper mapper)
        {
            _mapper = mapper;
            _examinerTypeRepository = examinerTypeRepository;
        }

        public async Task<List<ExaminerType>> GetExaminerTypeList()
        {
            List<Repository.DataModels.ExaminerType> obj = await _examinerTypeRepository.GetList();

            var list = _mapper.Map<List<ExaminerType>>(obj);
            return list;
        }
    }
}