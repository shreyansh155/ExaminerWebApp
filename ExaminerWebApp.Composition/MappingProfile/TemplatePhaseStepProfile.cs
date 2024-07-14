using AutoMapper;
 namespace ExaminerWebApp.Composition.MappingProfile
{

    public class TemplatePhaseStepProfile : Profile
    {
        public TemplatePhaseStepProfile()
        {
            CreateMap<Repository.DataModels.TemplatePhaseStep, Entities.Entities.TemplatePhaseStep>()
                .ReverseMap();
        }
    }
}

