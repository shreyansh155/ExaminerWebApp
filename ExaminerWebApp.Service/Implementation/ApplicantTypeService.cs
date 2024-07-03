using AutoMapper;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Repository.Interface;
using ExaminerWebApp.Service.Interface;

namespace ExaminerWebApp.Service.Implementation
{
    public class ApplicantTypeService : IApplicantTypeService
    {
        private readonly IApplicantTypeRepository _dropdown;
        private readonly IMapper _mapper;
        public ApplicantTypeService(IApplicantTypeRepository dropdown, IMapper mapper)
        {
            _mapper = mapper;
            _dropdown = dropdown;
        }

        public List<ApplicantType> GetApplicantTypeList()
        {
            List<Repository.DataModels.ApplicantType> applicantTypes = _dropdown.GetList();
            var obj = _mapper.Map<List<ApplicantType>>(applicantTypes);
            return obj;
        }
    }
}
