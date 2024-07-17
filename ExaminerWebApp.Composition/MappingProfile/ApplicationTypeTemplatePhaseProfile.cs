using AutoMapper;

namespace ExaminerWebApp.Composition.MappingProfile
{
    public class ApplicationTypeTemplatePhaseProfile : Profile
    {
        public ApplicationTypeTemplatePhaseProfile()
        {
            CreateMap<Entities.Entities.ApplicationTypeTemplatePhase, Repository.DataModels.ApplicationTypeTemplatePhase>().ReverseMap();
        }
    }
}