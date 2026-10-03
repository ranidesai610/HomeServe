using Microsoft.AspNetCore.Mvc;

namespace HomeServiceManagementSystem.Controllers
{
    public class ProviderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}