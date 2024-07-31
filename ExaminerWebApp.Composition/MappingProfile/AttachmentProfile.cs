using AutoMapper;

namespace ExaminerWebApp.Composition.MappingProfile
{
    public class AttachmentProfile : Profile
    {
        public AttachmentProfile()
        {
            CreateMap<Entities.Entities.TemplatePhaseStepAttachment, Repository.DataModels.TemplatePhaseStepAttachment>().ReverseMap();
        }
    }
}