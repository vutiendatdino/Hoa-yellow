using Microsoft.AspNetCore.Mvc;

namespace SP23_G21_SHSMS.Controllers.Commons
{
	public class ReportController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
