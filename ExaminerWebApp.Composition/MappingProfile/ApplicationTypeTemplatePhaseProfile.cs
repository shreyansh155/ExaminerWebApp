using AutoMapper;

namespace ExaminerWebApp.Composition.MappingProfile
{
    public class ApplicationTypeTemplatePhaseProfile : Profile
    {
        public ApplicationTypeTemplatePhaseProfile()
        {
            CreateMap<Entities.Entities.ApplicationTypeTemplatePhase, Repository.DataModels.ApplicationTypeTemplatePhase>();

            CreateMap<Repository.DataModels.ApplicationTypeTemplatePhase, Entities.Entities.ApplicationTypeTemplatePhase>()
               .ForMember(dest => dest.PhaseName, opt => opt.MapFrom(src => src.Phase.Name));
        }
    }
}