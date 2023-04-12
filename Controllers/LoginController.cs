using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SP23_G21_SHSMS.Models.SHSMS;
using SP23_G21_SHSMS.Services.SHSMS_Services.CampusServices;
using SP23_G21_SHSMS.Ultilities;

namespace SP23_G21_SHSMS.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DbShsmsContext _context;
        private ICampusSv _campusServices;

        public LoginController(ILogger<HomeController> logger, DbShsmsContext context)
        {
            _logger = logger;
            _context = context;
            _campusServices = new CampusSvImpl(_context);
        }

        public IActionResult Index()
        {
            var userTmp = HttpContext.Session.GetString("User");
            if (userTmp is null)
            {
                var campuses = _campusServices.GetCampuses();
                return View(campuses);
            }
            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        public IActionResult Login(string number)
        {
            if (string.IsNullOrEmpty(number) || number.ToLower().Equals("default"))
            {
                TempData[Constants.Error] = Constants.ErrorMessageSelectCampus;
                return RedirectToAction("Index");
            }
            HttpContext.Session.SetString(Constants.Campus, number);
            return RedirectToAction("LoginGoogle", "Home");
        }
    }
}
