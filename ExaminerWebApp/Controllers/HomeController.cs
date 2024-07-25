using Microsoft.AspNetCore.Mvc;

namespace ExaminerWebApp.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View());
        }

        [Route("Error")]
        public IActionResult ErrorPage()
        {
            var exceptionMessage = HttpContext.Items["ExceptionMessage"]?.ToString();
            ViewBag.ExceptionMessage = exceptionMessage;

            return View();
        }
    }
}