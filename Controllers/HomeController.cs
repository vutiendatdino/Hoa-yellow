using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SP23_G21_SHSMS.Models;
using SP23_G21_SHSMS.Models.SHSMS;
using SP23_G21_SHSMS.Services.SHSMS_Services.UserServices;
using System.Diagnostics;
using System.Net.Mail;
using SP23_G21_SHSMS.Ultilities;
using System.Text.Json;
namespace SP23_G21_SHSMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DbShsmsContext _context;
        private IUserSv _userServices;
        public HomeController(ILogger<HomeController> logger, DbShsmsContext context)
        {
            _logger = logger;
            _context = context;
            _userServices = new UserSvImpl(_context);
        }

        public IActionResult Index()
        {
            var userTmp = HttpContext.Session.GetString("User");
            if (userTmp is null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var user = JsonConvert.DeserializeObject<User>(userTmp);
                if (user.Role.RoleName.Equals("System Admin"))
                    return RedirectToAction("Index", "Admin");
                if (user.Role.RoleName.Equals("Clinic Head"))
                    return RedirectToAction("Index", "Head");
                if (user.Role.RoleName.Equals("Clinic Staff"))
                    return RedirectToAction("Index", "Staff");
                if (user.Role.RoleName.Equals("Quản Nhiệm"))
                    return RedirectToAction("Index", "HighSchool");
                else
                    return View();
            }
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task LoginGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
        }
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Where(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Select(claim => claim.Value);
            var emailAddress = claims.FirstOrDefault();

            MailAddress address = new MailAddress(emailAddress!);
            string host = address.Host;
            //if (address.Equals("duongnbhe140622@fpt.edu.vn"))
            var campus = HttpContext.Session.GetString(Constants.Campus);
            int campusId = 0;
            if (campus == null)
            {
                RedirectToAction("Index", "Login");
            }
            else
            {
                campusId = int.Parse(campus);
            }
            if (_userServices.Login(address.Address, campusId))
            {
                //HttpContext.Session.SetString("User", System.Text.Json.JsonSerializer.Serialize(_userServices.GetUserByEmail(address.Address)));
                HttpContext.Session.SetString("User", JsonConvert.SerializeObject(_userServices.GetUserByEmail(address.Address)));
                //var a = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User")!);
                return RedirectToAction("Index");
            };
            return RedirectToAction("Logout");
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}