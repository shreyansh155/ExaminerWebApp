using AutoMapper;

namespace ExaminerWebApp.Composition.MappingProfile
{

    public class StepProfile : Profile
    {
        public StepProfile()
        {
            CreateMap<Repository.DataModels.Step, Entities.Entities.Step>().
                ForMember(dest => dest.StepType, opt => opt.MapFrom(src => src.StepType.Name));

            CreateMap<Entities.Entities.Step, Repository.DataModels.Step>();

            CreateMap<Entities.Entities.StepType, Repository.DataModels.StepType>().ReverseMap();
        }
    }
}