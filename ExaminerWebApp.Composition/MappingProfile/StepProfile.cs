using AutoMapper;

namespace ExaminerWebApp.Composition.MappingProfile
{
    public class StepProfile : Profile
    {
        public StepProfile()
        {
            CreateMap<Repository.DataModels.Step, Entities.Entities.Step>().
                ForMember(dest => dest.StepType, opt => opt.MapFrom(src => src.StepType.Name));

            CreateMap<Entities.Entities.Step, Repository.DataModels.Step>()
               .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"))
               .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
               .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
               .ForMember(dest => dest.ModifiedDate, opt =>
               {
                   opt.PreCondition(src => src.ModifiedDate > DateTime.MinValue);
                   opt.MapFrom(src => src.ModifiedDate);
               });

            CreateMap<Entities.Entities.StepType, Repository.DataModels.StepType>().ReverseMap();
        }
    }
}