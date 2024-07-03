using AutoMapper;

namespace ExaminerWebApp.Composition.MappingProfile
{
    public class ApplicantProfile : Profile
    {
        public ApplicantProfile()
        {
            CreateMap<Entities.Entities.Applicant, Repository.DataModels.Applicant>()
                .ForMember(dest=>dest.ApplicantType, opt => opt.MapFrom(src=>src.Setting))
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FormFile != null && src.FormFile.Length > 0 ? src.FormFile.FileName : null));

            CreateMap<Repository.DataModels.Applicant, Entities.Entities.Applicant>()
                .ForMember(dest => dest.ApplicantType, opt => opt.MapFrom(src => src.ApplicantType.ApplicantTypeName))
                .ForMember(dest => dest.FormFile, opt => opt.Ignore());
        }
    }
}
 