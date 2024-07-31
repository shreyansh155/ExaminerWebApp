using AutoMapper;

namespace ExaminerWebApp.Composition.MappingProfile
{
    public class DocumentTypeProfile : Profile
    {
        public DocumentTypeProfile()
        {
            CreateMap<Entities.Entities.DocumentFileType, Repository.DataModels.DocumentFileType>().ReverseMap();
        }
    }
}