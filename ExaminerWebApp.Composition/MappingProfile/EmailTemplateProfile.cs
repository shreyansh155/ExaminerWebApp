using AutoMapper;

namespace ExaminerWebApp.Composition.MappingProfile
{
    public class EmailTemplateProfile : Profile
    {
        public EmailTemplateProfile()
        {
            CreateMap<Entities.Entities.EmailTemplate, Repository.DataModels.EmailTemplate>();

            CreateMap<Repository.DataModels.EmailTemplate, Entities.Entities.EmailTemplate>();
        }
    }
}