using AutoMapper;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Repository.Interface;
using ExaminerWebApp.Service.Interface;

namespace ExaminerWebApp.Service.Implementation
{
    public class ApplicationTypeService : IApplicationTypeService
    {
        private readonly IMapper _mapper;
        private readonly IApplicationTypeTemplateRepository _applicationTypeRepository;
        public ApplicationTypeService(IMapper mapper, IApplicationTypeTemplateRepository applicationTypeRepository)
        {
            _applicationTypeRepository = applicationTypeRepository;
            _mapper = mapper;
        }
        public IQueryable<ApplicationTypeTemplate> GetAll(string s)
        {
            var list = _applicationTypeRepository.GetAll(s).AsQueryable();
            var obj = _mapper.ProjectTo<ApplicationTypeTemplate>(list);
            return obj;
        }
        public ApplicationTypeTemplate GetById(int id)
        {
            Repository.DataModels.ApplicationTypeTemplate obj = _applicationTypeRepository.GetById(id);
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
        public bool Delete(int id)
        {
            _applicationTypeRepository.Delete(id);
            return true;
        }
        public bool Update(ApplicationTypeTemplate model)
        {
            Repository.DataModels.ApplicationTypeTemplate obj = _mapper.Map<Repository.DataModels.ApplicationTypeTemplate>(model);
            try
            {
                _applicationTypeRepository.Update(obj);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
