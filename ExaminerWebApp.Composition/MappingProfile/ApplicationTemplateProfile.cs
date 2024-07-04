using AutoMapper;

namespace ExaminerWebApp.Composition.MappingProfile
{
    public class ApplicationTemplateProfile : Profile
    {
        public ApplicationTemplateProfile()
        {
            CreateMap<Repository.DataModels.ApplicationTypeTemplate, Entities.Entities.ApplicationTypeTemplate>();

            CreateMap<Entities.Entities.ApplicationTypeTemplate, Repository.DataModels.ApplicationTypeTemplate>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "1"))
                .ForMember(dest => dest.CreatedDate, opt =>
                {
                    opt.PreCondition(src => src.CreatedDate > DateTime.MinValue);
                    opt.MapFrom(src => src.CreatedDate);
                })
                .ForMember(dest => dest.Phases, opt => opt.Ignore());

        }
    }
}