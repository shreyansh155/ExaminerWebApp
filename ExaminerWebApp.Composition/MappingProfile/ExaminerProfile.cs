using AutoMapper;

namespace ExaminerWebApp.Composition.MappingProfile
{
    public class ExaminerProfile : Profile
    {
        public ExaminerProfile()
        {
            CreateMap<Entities.Entities.Examiner, Repository.DataModels.Examiner>()
               .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.FormFile != null && src.FormFile.Length > 0 ? src.FormFile.FileName : null))
               .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => 1))
               .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"))
               .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
               .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
               .ForMember(dest => dest.ModifiedDate, opt =>
                {
                    opt.PreCondition(src => src.ModifiedDate > DateTime.MinValue);
                    opt.MapFrom(src => src.ModifiedDate);
                })
               .ForMember(dest => dest.ExaminerNavigation, opt => opt.Ignore())
               .ForMember(dest => dest.Status, opt => opt.Ignore());

            CreateMap<Repository.DataModels.Examiner, Entities.Entities.Examiner>()
               .ForMember(dest => dest.ExaminerTypeName, opt => opt.MapFrom(src => src.ExaminerNavigation.Name))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.StatusName))
               .ForMember(dest => dest.FormFile, opt => opt.Ignore());
        }
    }
}