using AutoMapper;
using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Repository.Interface;
using ExaminerWebApp.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace ExaminerWebApp.Service.Implementation
{
    public class EmailTemplateService : BaseService, IEmailTemplateService
    {
        public readonly IEmailTemplateRepository _emailTemplateRepository;
        public readonly IMapper _mapper;

        public EmailTemplateService(IEmailTemplateRepository emailTemplateRepository, IMapper mapper)
        {
            _emailTemplateRepository = emailTemplateRepository;
            _mapper = mapper;
        }

        public async Task<PaginationSet<Entities.Entities.EmailTemplate>> GetAll(PaginationSet<Entities.Entities.EmailTemplate> pager)
        {

            var list = _emailTemplateRepository.GetAll().AsQueryable();

            //filtering based on search fields
            if (pager.Filter != null && pager.Filter.Filters != null && pager.Filter.Filters.Count > 0)
            {
                foreach (var filter in pager.Filter.Filters)
                {
                    if (filter.Field != "id")
                    {
                        list = ApplyFilter(list, filter);
                    }
                }
            }

            //sorting ascending descending
            if (pager.Sort != null && pager.Sort.Count > 0)
            {
                foreach (var sort in pager.Sort)
                {
                    list = ApplySorting(list, sort);
                }
            }
            var obj = _mapper.ProjectTo<Entities.Entities.EmailTemplate>(list);

            pager.Items = await obj.Skip(pager.Skip).Take(pager.Take).ToListAsync();

            pager.TotalCount = await obj.CountAsync();

            return pager;
        }

        public async Task<Entities.Entities.EmailTemplate> Create(Entities.Entities.EmailTemplate model)
        {
            model.CreatedDate = DateTime.UtcNow;
            model.CreatedBy = 1;
            Repository.DataModels.EmailTemplate emailTemplate = _mapper.Map<Repository.DataModels.EmailTemplate>(model);
            await _emailTemplateRepository.Create(emailTemplate);
            return model;
        }

        public async Task<Entities.Entities.EmailTemplate> Update(Entities.Entities.EmailTemplate model)
        {
            model.ModifiedDate = DateTime.UtcNow;
            model.ModifiedBy = 1;
            Repository.DataModels.EmailTemplate emailTemplate = _mapper.Map<Repository.DataModels.EmailTemplate>(model);
            await _emailTemplateRepository.Update(emailTemplate);
            return model;
        }

        public async Task<int> Delete(int id)
        {
            return await _emailTemplateRepository.Delete(id);
        }

        public async Task<Entities.Entities.EmailTemplate> GetEmailTemplate(int? id)
        {
            Repository.DataModels.EmailTemplate emailTemplate = await _emailTemplateRepository.GetById(id);
            return _mapper.Map<Entities.Entities.EmailTemplate>(emailTemplate);
        }

        public async Task<bool> CheckIfExists(int? id, string name)
        {
            return await _emailTemplateRepository.CheckIfExists(id, name);
        }
    }
}