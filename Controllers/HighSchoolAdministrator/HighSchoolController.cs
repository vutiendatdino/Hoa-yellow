using Microsoft.AspNetCore.Mvc;

namespace SP23_G21_SHSMS.Controllers.HighSchoolAdministrator
{
    public class HighSchoolController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
