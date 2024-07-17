using AutoMapper;
namespace ExaminerWebApp.Composition.MappingProfile
{
    public class TemplatePhaseStepProfile : Profile
    {
        public TemplatePhaseStepProfile()
        {
            CreateMap<Repository.DataModels.TemplatePhaseStep, Entities.Entities.TemplatePhaseStep>()
                             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                             .ForMember(dest => dest.TemplatePhaseId, opt => opt.MapFrom(src => src.TemplatePhaseId))
                             .ForMember(dest => dest.Ordinal, opt => opt.MapFrom(src => src.Ordinal))
                             .ForMember(dest => dest.StepId, opt => opt.MapFrom(src => src.StepId));

            CreateMap<Entities.Entities.TemplatePhaseStep, Repository.DataModels.TemplatePhaseStep>();
        }
    }
}