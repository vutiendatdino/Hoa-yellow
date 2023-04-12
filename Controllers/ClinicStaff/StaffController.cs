using Microsoft.AspNetCore.Mvc;

namespace SP23_G21_SHSMS.Controllers.ClinicStaff
{
    public class StaffController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
