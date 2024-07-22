using AutoMapper;
using ExaminerWebApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using ExaminerWebApp.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace PracticeWebApp.Service.Implementation
{
    public class ExaminerService : IExaminerService
    {
        private readonly IExaminerRepository _examinerRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _config;

        public ExaminerService(IExaminerRepository examinerRepository, IMapper mapper, IWebHostEnvironment environment, IConfiguration config)
        {
            _environment = environment;
            _examinerRepository = examinerRepository;
            _mapper = mapper;
            _config = config;
        }

        public async Task<ExaminerWebApp.Entities.Entities.Examiner> GetExaminerById(int id)
        {
            ExaminerWebApp.Repository.DataModels.Examiner result = await _examinerRepository.GetById(id);
            return _mapper.Map<ExaminerWebApp.Entities.Entities.Examiner>(result);
        }

        public IQueryable<ExaminerWebApp.Entities.Entities.Examiner> GetAllExaminer()
        {

            IQueryable<ExaminerWebApp.Repository.DataModels.Examiner> list = _examinerRepository.GetAll()
            .Include(x => x.ExaminerNavigation)
            .Include(x => x.Status)
            .AsQueryable();
            IQueryable<ExaminerWebApp.Entities.Entities.Examiner> obj = _mapper.ProjectTo<ExaminerWebApp.Entities.Entities.Examiner>(list);
            return obj;

        }

        public async Task<ExaminerWebApp.Entities.Entities.Examiner> AddExaminer(ExaminerWebApp.Entities.Entities.Examiner model)
        {

            ExaminerWebApp.Repository.DataModels.Examiner obj = _mapper.Map<ExaminerWebApp.Repository.DataModels.Examiner>(model);

            if (model.FormFile != null && model.FormFile.Length != 0)
            {
                obj.FilePath = SaveFile(model.FormFile);
            }
            //SendEmail(model.Email);
            await _examinerRepository.Create(obj);
            return model;
        }

        public async Task<bool> DeleteExaminer(int id)
        {
            await _examinerRepository.Delete(id);
            return true;
        }

        public async Task<bool> UpdateExaminer(ExaminerWebApp.Entities.Entities.Examiner model)
        {
            if (model.FormFile != null && model.FormFile.Length > 0)
            {
                model.FilePath = SaveFile(model.FormFile);
            }
            var result = _mapper.Map<ExaminerWebApp.Repository.DataModels.Examiner>(model);
            await _examinerRepository.Update(result);
            return true;
        }

        private string? SaveFile(IFormFile? formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return null;
            }
            var rootPath = "PracticeApp/PracticeWebAppSLN/PracticeWebApp/wwwroot/UploadedFiles";
            var uploadsFolder = Path.Combine(_environment.WebRootPath, rootPath);
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                formFile.CopyTo(fileStream);
            }

            return uniqueFileName;
        }

        public bool CheckEmailIfExists(string email)
        {
            return _examinerRepository.CheckEmail(email);
        }

        private T ExecuteWithTryCatch<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing your request.", ex);
            }
        }

        public void SendEmail(string email)
        {
            string? senderEmail = _config.GetSection("OutlookSMTP")["Sender"] ?? "";
            string? senderPassword = _config.GetSection("OutlookSMTP")["Password"];

            SmtpClient client = new("smtp.office365.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            MailMessage mailMessage = new()
            {
                From = new MailAddress(senderEmail, "Examiner"),
                Subject = "Examiner Password Reset",
                IsBodyHtml = true,
                Body = "<h3>Click on the link below to reset your password.</h3><a href=\"" + "www.google.com" + "\">Reset Password</a>",
            };

            mailMessage.To.Add(email);
            client.Send(mailMessage);
        }
    }
}