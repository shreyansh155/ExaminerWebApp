using Microsoft.AspNetCore.Mvc;

namespace ExaminerWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
