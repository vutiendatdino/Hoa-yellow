using Microsoft.AspNetCore.Mvc;

namespace SP23_G21_SHSMS.Controllers.Admin
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
