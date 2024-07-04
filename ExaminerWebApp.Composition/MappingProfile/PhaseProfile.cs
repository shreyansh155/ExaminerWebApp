using AutoMapper;

namespace ExaminerWebApp.Composition.MappingProfile
{
    public class PhaseProfile : Profile
    {
        public PhaseProfile()
        {
            CreateMap<Entities.Entities.Phase, Repository.DataModels.Phase>();

            CreateMap<Repository.DataModels.Applicant, Entities.Entities.Applicant>();
        }
    }
}
