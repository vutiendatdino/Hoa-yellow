using Microsoft.AspNetCore.Mvc;
using SP23_G21_SHSMS.Models.Campuses;
using System.Dynamic;

namespace SP23_G21_SHSMS.Controllers.ClinicHead
{
    public class HeadController : Controller
    {
        public IActionResult Index()
        {
            var userTmp = HttpContext.Session.GetString("User");
            if (userTmp == null)
                return RedirectToAction("Index", "Login");
            dynamic models = new ExpandoObject();
            DbCampusContext context = new DbCampusContext();
            IEnumerable<Symptom> symptoms = context.Symptoms.ToList();
            //IEnumerable<Supplier> suppliers = context.Suppliers.ToList();
			models.Symptom = symptoms;
            //models.Supplier = suppliers;
            return View(models);
        }
    }
}
