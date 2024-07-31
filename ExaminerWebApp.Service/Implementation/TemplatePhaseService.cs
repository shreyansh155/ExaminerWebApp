using AutoMapper;
using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Repository.Interface;
using ExaminerWebApp.Service.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace ExaminerWebApp.Service.Implementation
{
    public class TemplatePhaseService : ITemplatePhaseService
    {
        private readonly IMapper _mapper;
        private readonly ITemplatePhaseRepository _templatePhaseRepository;
        private readonly IPhaseStepRepository _phaseStepRepository;
        private readonly IStepRepository _stepRepository;
        private readonly ITemplatePhaseStepAttachmentRepository _templatePhaseStepAttachment;
        private readonly ITemplatePhaseStepDocumentProofRepository _templatePhaseStepDocumentProofRepository;
        private readonly IWebHostEnvironment _environment;



        public TemplatePhaseService(ITemplatePhaseRepository templatePhaseRepository, IMapper mapper, IPhaseStepRepository phaseStepRepository, IStepRepository stepRepository, ITemplatePhaseStepAttachmentRepository templatePhaseStepAttachment, ITemplatePhaseStepDocumentProofRepository templatePhaseStepDocumentProofRepository, IWebHostEnvironment environment)
        {
            _mapper = mapper;
            _templatePhaseRepository = templatePhaseRepository;
            _phaseStepRepository = phaseStepRepository;
            _stepRepository = stepRepository;
            _templatePhaseStepAttachment = templatePhaseStepAttachment;
            _templatePhaseStepDocumentProofRepository = templatePhaseStepDocumentProofRepository;
            _environment = environment;
        }

        public async Task<ApplicationTypeTemplatePhase> AddTemplatePhase(ApplicationTypeTemplatePhase model)
        {
            Repository.DataModels.ApplicationTypeTemplatePhase obj = _mapper.Map<Repository.DataModels.ApplicationTypeTemplatePhase>(model);

            List<Repository.DataModels.TemplatePhaseStep> templatePhaseStep = _mapper.Map<List<Repository.DataModels.TemplatePhaseStep>>(model.TemplatePhaseSteps);

            Repository.DataModels.ApplicationTypeTemplatePhase tempPhase = _templatePhaseRepository.AddPhaseWithOrdinal(obj);

            await _phaseStepRepository.AddStepsWithOrdinal(templatePhaseStep, tempPhase);

            return model;
        }

        public async Task<bool> UpdateOrdinal(int templatePhaseId, int ordinal)
        {
            await _templatePhaseRepository.UpdateOrdinal(templatePhaseId, ordinal);

            return true;
        }

        public async Task<TemplatePhaseStep> GetTemplatePhaseStep(int id)
        {
            Repository.DataModels.TemplatePhaseStep templatePhaseStep = await _phaseStepRepository.GetTemplatePhaseStep(id);
            return _mapper.Map<TemplatePhaseStep>(templatePhaseStep);
        }

        public async Task<int?> GetStepTypeId(int stepId)
        {
            return await _phaseStepRepository.GetStepTypeId(stepId);
        }

        public async Task<TemplatePhaseStep> AddTemplatePhaseStep(TemplatePhaseStep templatePhaseStep)
        {
            Repository.DataModels.TemplatePhaseStep phaseStep = _mapper.Map<Repository.DataModels.TemplatePhaseStep>(templatePhaseStep);
            await _phaseStepRepository.AddPhaseStep(phaseStep);
            if (templatePhaseStep.Instruction != null)
            {
                await _stepRepository.UpdateInstruction(templatePhaseStep.StepId, templatePhaseStep.Instruction ?? "");
            }
            return templatePhaseStep;
        }

        public async Task<TemplatePhaseStep> EditTemplatePhaseStep(TemplatePhaseStep templatePhaseStep)
        {
            Repository.DataModels.TemplatePhaseStep phaseStep = _mapper.Map<Repository.DataModels.TemplatePhaseStep>(templatePhaseStep);
            await _phaseStepRepository.UpdatePhaseStep(phaseStep);
            if (templatePhaseStep.Instruction != null)
            {
                await _stepRepository.UpdateInstruction(templatePhaseStep.StepId, templatePhaseStep.Instruction ?? "");
            }
            return templatePhaseStep;
        }

        public async Task<int?> DeleteStep(int id)
        {
            return await _phaseStepRepository.DeleteStep(id);
        }
        public async Task<int?> DeletePhase(int id)
        {
            return await _templatePhaseRepository.Delete(id);
        }

        public async Task<int> GetNewPhaseOrdinal(int templateId)
        {
            int ordinal = await _phaseStepRepository.GetNewPhaseOrdinal(templateId);
            return ordinal;
        }

        public async Task<int> GetNewStepOrdinal(int templatePhaseId)
        {
            int ordinal = await _phaseStepRepository.GetNewStepOrdinal(templatePhaseId);
            return ordinal;
        }

        public async Task<PaginationSet<TemplatePhaseStepAttachment>> GetAttachments(int? tpsId, PaginationSet<TemplatePhaseStepAttachment> pager)
        {
            IQueryable<Repository.DataModels.TemplatePhaseStepAttachment> list = _templatePhaseStepAttachment.GetAll(tpsId).Include(x => x.AttachmentType);
            pager.Items = _mapper.ProjectTo<TemplatePhaseStepAttachment>(list);
            pager.TotalCount = await list.CountAsync();
            return pager;
        }

        public async Task<PaginationSet<TemplatePhaseStepDocumentProof>> GetDocumentProof(int? tpsId, PaginationSet<TemplatePhaseStepDocumentProof> pager)
        {
            IQueryable<Repository.DataModels.TemplatePhaseStepDocumentProof> list = _templatePhaseStepDocumentProofRepository.GetAll(tpsId).Include(x => x.DocumentFileTypeNavigation);
            pager.Items = _mapper.ProjectTo<TemplatePhaseStepDocumentProof>(list);
            pager.TotalCount = await list.CountAsync();
            return pager;
        }

        public async Task<List<DocumentFileType>> GetDocumentFileTypes()
        {
            return _mapper.Map<List<DocumentFileType>>(await _templatePhaseStepAttachment.GetDocumentTypes());
        }

        public async Task<TemplatePhaseStepDocumentProof> CreateDocumentProof(TemplatePhaseStepDocumentProof model)
        {
            Repository.DataModels.TemplatePhaseStepDocumentProof obj = _mapper.Map<Repository.DataModels.TemplatePhaseStepDocumentProof>(model);
            await _templatePhaseStepDocumentProofRepository.Create(obj);
            return model;
        }
        public async Task<TemplatePhaseStepDocumentProof> EditDocumentProof(TemplatePhaseStepDocumentProof model)
        {
            Repository.DataModels.TemplatePhaseStepDocumentProof obj = _mapper.Map<Repository.DataModels.TemplatePhaseStepDocumentProof>(model);
            await _templatePhaseStepDocumentProofRepository.Update(obj);
            return model;

        }
        public async Task<int> DeleteDocumentProof(int id)
        {
            return await _templatePhaseStepDocumentProofRepository.Delete(id);
        }

        public async Task<TemplatePhaseStepAttachment> CreateAttachment(TemplatePhaseStepAttachment model)
        {
            Repository.DataModels.TemplatePhaseStepAttachment attachment = _mapper.Map<Repository.DataModels.TemplatePhaseStepAttachment>(model);

            attachment.FilePath = model.AttachmentFile.FileName;

            var obj = await _templatePhaseStepAttachment.Create(attachment);

            var fileName = Path.GetFileName(model.AttachmentFile.FileName);

            string rootPath = _environment.WebRootPath + "/UploadedFiles";

            string userFolder = Path.Combine(rootPath, "TPSAttachments", obj.TemplatePhaseStepId.ToString(), obj.Id.ToString());

            if (!Directory.Exists(userFolder))
            {
                Directory.CreateDirectory(userFolder);
            }

            string filePath = Path.Combine(userFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                model.AttachmentFile.CopyTo(stream);
            }

            return model;
        }

        public async Task<TemplatePhaseStepAttachment> EditAttachment(TemplatePhaseStepAttachment model)
        {
            Repository.DataModels.TemplatePhaseStepAttachment obj = _mapper.Map<Repository.DataModels.TemplatePhaseStepAttachment>(model);
            await _templatePhaseStepAttachment.Update(obj);
            return model;
        }
        public async Task<int> DeleteAttachment(int id)
        {
            return await _templatePhaseStepAttachment.Delete(id);
        }
    }
}