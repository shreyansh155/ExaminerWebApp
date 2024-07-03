using AutoMapper;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Repository.Interface;
using ExaminerWebApp.Service.Interface;
using Microsoft.EntityFrameworkCore;

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
        public IQueryable<ApplicationTypeTemplate> GetAll()
        {
            var list = _applicationTypeRepository.GetAll().AsQueryable();
            var obj = _mapper.ProjectTo<ApplicationTypeTemplate>(list);
            return obj;
        }
        //public ApplicationTypeTemplate GetById(int id)
        //{
        //    return new ApplicationTypeTemplate();
        //}
        //public Task<ApplicationTypeTemplate> Add(ApplicationTypeTemplate model)
        //{
        //    return null;
        //}
        //public bool Delete(int id)
        //{
        //    return true;
        //}
        //public bool Update(Applicant model)
        //{
        //    return true;
        //}
    }
}
