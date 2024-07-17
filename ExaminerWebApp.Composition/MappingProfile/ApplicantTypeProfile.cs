using AutoMapper;

namespace ExaminerWebApp.Composition.MappingProfile
{
    public class ApplicantTypeProfile : Profile
    {
        public ApplicantTypeProfile()
        {
            CreateMap<Repository.DataModels.ApplicantType, Entities.Entities.ApplicantType>().ReverseMap();
        }
    }
}