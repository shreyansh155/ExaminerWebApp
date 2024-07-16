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

        #region Constructor
        public ExaminerService(IExaminerRepository examinerRepository, IMapper mapper, IWebHostEnvironment environment, IConfiguration config)
        {
            _environment = environment;
            _examinerRepository = examinerRepository;
            _mapper = mapper;
            _config = config;
        }
        #endregion

        #region INTERFACE METHODS

        #region READ SPECIFIC EXAMINER
        public async Task<ExaminerWebApp.Entities.Entities.Examiner> GetExaminerById(int id)
        {
            ExaminerWebApp.Repository.DataModels.Examiner result = await _examinerRepository.GetById(id);
            return _mapper.Map<ExaminerWebApp.Entities.Entities.Examiner>(result);
        }
        #endregion

        #region READ ALL EXAMINER
        public IQueryable<ExaminerWebApp.Entities.Entities.Examiner> GetAllExaminer()
        {

            IQueryable<ExaminerWebApp.Repository.DataModels.Examiner> list = _examinerRepository.GetAll()
            .Include(x => x.ExaminerNavigation)
            .Include(x => x.Status)
            .AsQueryable();
            IQueryable<ExaminerWebApp.Entities.Entities.Examiner> obj = _mapper.ProjectTo<ExaminerWebApp.Entities.Entities.Examiner>(list);
            return obj;

        }
        #endregion

        #region ADD EXAMINER
        public async Task<ExaminerWebApp.Entities.Entities.Examiner> AddExaminer(ExaminerWebApp.Entities.Entities.Examiner model)
        {

            ExaminerWebApp.Repository.DataModels.Examiner obj = _mapper.Map<ExaminerWebApp.Repository.DataModels.Examiner>(model);

            if (model.FormFile != null && model.FormFile.Length != 0)
            {
                obj.FilePath = SaveFile(model.FormFile);
            }
            SendEmail(model.Email);
            await _examinerRepository.Create(obj);
            return model;
        }
        #endregion

        #region DELETE EXAMINER
        public async Task<bool> DeleteExaminer(int id)
        {
            await _examinerRepository.Delete(id);
            return true;
        }
        #endregion

        #region GET EXAMINER DETAILS
        public bool UpdateExaminer(ExaminerWebApp.Entities.Entities.Examiner model)
        {
            if (model.FormFile != null && model.FormFile.Length > 0)
            {
                model.FilePath = SaveFile(model.FormFile);
            }
            var result = _mapper.Map<ExaminerWebApp.Repository.DataModels.Examiner>(model);
            _examinerRepository.Update(result);
            return true;
        }
        #endregion

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
        #region CHECK IF EMAIL EXISTS
        public bool CheckEmailIfExists(string email)
        {
            return _examinerRepository.CheckEmail(email);
        }
        #endregion
        #endregion

        #region HELPER METHODS
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
        #endregion
    }
}
