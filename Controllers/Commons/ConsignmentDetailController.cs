using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SP23_G21_SHSMS.Models.Campuses;
using SP23_G21_SHSMS.Models.CustomModels;
using SP23_G21_SHSMS.Models.SHSMS;
using SP23_G21_SHSMS.Services.Campus_Services.ConsigmentServices;
using SP23_G21_SHSMS.Services.Campus_Services.StorageServices;
using SP23_G21_SHSMS.Services.Campus_Services.SymptomServices;
using System.Dynamic;

namespace SP23_G21_SHSMS.Controllers.Commons
{
    public class ConsignmentDetailController : Controller
    {
        private readonly DbCampusContext _contextCampus;
        private readonly DbShsmsContext _contextSHSMS;
        private IStorageSv _storageSv;
        private IConsignmentSv _consignmentSv;

        public ConsignmentDetailController(DbCampusContext contextCampus, DbShsmsContext contextSHSMS)
        {
            _contextCampus = contextCampus;
            _contextSHSMS = contextSHSMS;
            _storageSv = new StorageSvImpl(_contextCampus);
            _consignmentSv = new ConsignmentSvImpl(_contextCampus);
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int consId, int? impId, string? med)
        {
            //TO DO: fix here
            dynamic models = new ExpandoObject();
            if (!impId.HasValue || med is null)
            {
                return NotFound();
            }
            /*var cons = _consignmentSv.GetConsignmentById(consId);
            var medicine = _contextSHSMS.Medicines.Where(x => x.MedicineId.Equals(med)).Include(x => x.Unit).FirstOrDefault();
            var stock = _storageSv.GetStorageByImportAndMedicine((int)impId, med);
            CustomViewStorageModel model = new CustomViewStorageModel { Consignment = cons, Medicine = medicine, Storage = stock };
            models.Detail = model;*/

            return View(models);
        }
        
    }
}
