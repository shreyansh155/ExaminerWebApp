using AutoMapper;

namespace PracticeWebApp.Composition.MappingProfile
{
    public class ExaminerTypeProfile : Profile
    {
        public ExaminerTypeProfile()
        {
            CreateMap<ExaminerWebApp.Repository.DataModels.ExaminerType, ExaminerWebApp.Entities.Entities.ExaminerType>().ReverseMap();
        }
    }
}