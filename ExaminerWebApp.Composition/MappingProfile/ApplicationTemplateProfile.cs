using AutoMapper;

namespace ExaminerWebApp.Composition.MappingProfile
{
    public class ApplicationTemplateProfile : Profile
    {
        public ApplicationTemplateProfile()
        {
            CreateMap<Repository.DataModels.ApplicationTypeTemplate, Entities.Entities.ApplicationTypeTemplate>();
        }
    }
}
