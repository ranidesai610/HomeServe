using Microsoft.AspNetCore.Mvc;

namespace HomeServiceManagementSystem.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}