using AutoMapper;

namespace ExaminerWebApp.Composition.MappingProfile
{
    public class DocumentProofProfile : Profile
    {
        public DocumentProofProfile()
        {
            CreateMap<Entities.Entities.TemplatePhaseStepDocumentProof, Repository.DataModels.TemplatePhaseStepDocumentProof>().ReverseMap();
        }
    }
}