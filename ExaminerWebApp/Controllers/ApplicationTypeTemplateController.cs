using Microsoft.AspNetCore.Mvc;

namespace ExaminerWebApp.Controllers
{
    public class ApplicationTypeTemplateController : Controller
    {
        public ApplicationTypeTemplateController() { }  
        public IActionResult Index()
        {
            return View();
        }
    }
}
