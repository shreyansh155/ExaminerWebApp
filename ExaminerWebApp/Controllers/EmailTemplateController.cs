using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ExaminerWebApp.Controllers
{
    public class EmailTemplateController : BaseController
    {
        public readonly IEmailTemplateService _emailTemplateService;

        public EmailTemplateController(IEmailTemplateService emailTemplateService)
        {
            _emailTemplateService = emailTemplateService;
        }

        public async Task<ActionResult> Index()
        {
            return await Task.FromResult(View());
        }

        public async Task<ActionResult> GetAll([FromBody] PaginationSet<EmailTemplate> pager)
        {
            return Json(await _emailTemplateService.GetAll(pager));
        }

        public async Task<IActionResult> EmailTemplate()
        {
            return Json(await Task.FromResult(new { success = true }));
        }

        public async Task<ActionResult> Create()
        {
            EmailTemplate model = new();
            return await Task.FromResult(View("EmailTemplate", model));
        }

        [HttpPost]
        public async Task<ActionResult> Create(EmailTemplate model)
        {
            if (ModelState.IsValid)
            {
                if (await _emailTemplateService.CheckIfExists(null, model.Name))
                    return Json(new { success = false, errors = "Email Template already exists" });

                await _emailTemplateService.Create(model);
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = ModelStateErrorSerializer(ModelState) });
        }

        public async Task<IActionResult> EditTemplate(int id)
        {
            return Json(await Task.FromResult(new { redirectUrl = Url.Action("Edit", "EmailTemplate", new { id }) }));
        }

        public async Task<ActionResult> Edit(int id)
        {
            return View("EmailTemplate", await _emailTemplateService.GetEmailTemplate(id));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EmailTemplate model)
        {
            if (ModelState.IsValid)
            {
                if (await _emailTemplateService.CheckIfExists(model.Id, model.Name))
                    return Json(new { success = false, errors = "Email Template already exists" });

                await _emailTemplateService.Update(model);
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = ModelStateErrorSerializer(ModelState) });
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            await _emailTemplateService.Delete(id);
            return Json(new { success = true });
        }
    }
}