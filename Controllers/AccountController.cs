using Microsoft.AspNetCore.Mvc;

namespace Talk2Me.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
