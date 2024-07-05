using AutoMapper;

namespace ExaminerWebApp.Composition.MappingProfile
{

    public class StepProfile : Profile
    {
        public StepProfile()
        {
            CreateMap<Repository.DataModels.Step, Entities.Entities.Step>();

            CreateMap<Entities.Entities.Step, Repository.DataModels.Step>();
        }
    }
}