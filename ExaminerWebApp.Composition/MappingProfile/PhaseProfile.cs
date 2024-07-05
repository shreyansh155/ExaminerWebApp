using AutoMapper;

namespace ExaminerWebApp.Composition.MappingProfile
{
    public class PhaseProfile : Profile
    {
        public PhaseProfile()
        {
            CreateMap<Entities.Entities.Phase, Repository.DataModels.Phase>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(x => "1"))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(x => DateTime.UtcNow));

            CreateMap<Repository.DataModels.Phase, Entities.Entities.Phase>();
        }
    }
}