using Microsoft.AspNetCore.Mvc;

namespace ExaminerWebApp.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View());
        }
    }
}