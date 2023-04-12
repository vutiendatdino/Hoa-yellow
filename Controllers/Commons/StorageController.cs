using OfficeOpenXml;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SP23_G21_SHSMS.Models.Campuses;
using SP23_G21_SHSMS.Models.CustomModels;
using SP23_G21_SHSMS.Models.SHSMS;
using SP23_G21_SHSMS.Services.Campus_Services.ConsigmentServices;
using SP23_G21_SHSMS.Services.Campus_Services.ExportDetailServices;
using SP23_G21_SHSMS.Services.Campus_Services.ExportServices;
using SP23_G21_SHSMS.Services.Campus_Services.ImportServices;
using SP23_G21_SHSMS.Services.Campus_Services.StorageServices;
using System.Dynamic;
using SP23_G21_SHSMS.Services.SHSMS_Services.MedicineServices;
using Microsoft.CodeAnalysis.Differencing;

namespace SP23_G21_SHSMS.Controllers.Commons
{
    public class StorageController : Controller
    {
        private readonly DbCampusContext _contextCampus;
        private readonly DbShsmsContext _contextSHSMS;
        private IStorageSv _storageSv;
        private IImportSv _importSv;
        private IConsignmentSv _consignmentSv;
        private IExportSv _exportSv;
        private IExportDetailSv _exportDetailSv;
        private IMedicineSv _medicineSv;
        public StorageController(DbCampusContext contextCampus, DbShsmsContext contextSHSMS)
        {
            _contextCampus = contextCampus;
            _contextSHSMS = contextSHSMS;
            _storageSv = new StorageSvImpl(_contextCampus);
            _importSv = new ImportSvImpl(_contextCampus);
            _consignmentSv = new ConsignmentSvImpl(_contextCampus);
            _exportSv = new ExportSvImpl(_contextCampus);
            _exportDetailSv = new ExportDetailSvImpl(_contextCampus);
            _medicineSv = new MedicineSvImpl(_contextSHSMS);
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ViewStorage(string searchString, string sortOrder)
        {
            ViewData["MedId"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["ExpDate"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;
            dynamic models = new ExpandoObject();

            List<CustomViewStorageModel> cusModels = new List<CustomViewStorageModel>();

            //search
            if (!string.IsNullOrEmpty(searchString))
            {
                var meds = _medicineSv.GetMedicinesByName(searchString);
                foreach(var med in meds)
                {
                    var medId = med.MedicineId;
                    
                    var storage = _storageSv.GetUsingStorageByMedicineId(medId);

                    foreach (var item in storage)
                    {
                        var cons = item.Consignment;
                        if (cons != null)
                        {
                            cusModels.Add(new CustomViewStorageModel { Consignment = cons, Medicine = med, Storage = item });
                        }
                    }
                }
            }
            else //show all
            {
                var storage = _storageSv.GetStorages();

                foreach (var item in storage)
                {
                    var cons = item.Consignment;
                    var medId = item.MedicineId;
                    var med = _medicineSv.GetMedicineById(medId);
                    if (cons != null)
                    {
                        cusModels.Add(new CustomViewStorageModel { Consignment = cons, Medicine = med, Storage = item });
                    }
                }
            }
            if(cusModels.Count > 0)
            {
                switch (sortOrder)
                {
                    case "name_desc":
                        models.Storage = cusModels.OrderByDescending(x => x.Medicine.MedicineId);
                        break;
                    case "Date":
                        models.Storage = cusModels.OrderBy(x => x.Consignment.ExpiredDate);
                        break;
                    case "date_desc":
                        models.Storage = cusModels.OrderByDescending(x => x.Consignment.ExpiredDate);
                        break;
                    default:
                        models.Storage = cusModels.OrderBy(x => x.Medicine.MedicineId);
                        break;
                }
            }
            return View(models);
        }
        public IActionResult Create()
        {
            var meds = _contextSHSMS.Medicines.Include(x => x.Unit).ToList();
            meds.Insert(0, new Medicine { MedicineId = "0", MedicineName = "Chọn thuốc" });
            ViewData["MedicineId"] = new SelectList(meds, "MedicineId", "MedicineName");
            //ViewData["Unit"] = new SelectList(_contextSHSMS.Units, "UnitId", "UnitName");
            return View();
        }
        //nhap le tung lo mot lan
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConsignmentNumber,MedicineId,ManufactureDate,ExpiredDate,Quantity,Price")] Consignment consignment)
        {
            if (ModelState.IsValid)
            {
                AddNewConsignmentToStorage(consignment);
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ExportTemplate()
        {
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = "Mã thuốc";
                worksheet.Cells["B1"].Value = "Tên thuốc";
                worksheet.Cells["C1"].Value = "Số lượng";
                worksheet.Cells["D1"].Value = "Đơn vị tính";
                worksheet.Cells["E1"].Value = "Số lô";
                worksheet.Cells["F1"].Value = "Ngày sản xuất";
                worksheet.Cells["G1"].Value = "Hạn sử dụng";
                worksheet.Cells["H1"].Value = "Giá nhập";

                worksheet.Cells.AutoFitColumns();

                xlPackage.Save();

            }
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Mẫu nhập thuốc.xlsx");
        }
        //TO DO: DUONGNB check data from file
        public IActionResult ImportFromExcel(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Check if a file was uploaded
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file was uploaded");
            }
            List<Consignment> consignments = new List<Consignment>();
            // Open the Excel package from the uploaded file
            using (var package = new ExcelPackage(file.OpenReadStream()))
            {
                // Get the first worksheet in the package
                var worksheet = package.Workbook.Worksheets.First();

                // Loop through the rows in the worksheet (skipping the first row, which contains column headers)
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    // Read data from the worksheet into variables
                    var medId = worksheet.Cells[row, 1].Value?.ToString();
                    var medName = worksheet.Cells[row, 2].Value?.ToString();
                    var quantity = Int32.Parse(worksheet.Cells[row, 3].Value?.ToString());
                    var unit = worksheet.Cells[row, 4].Value?.ToString();
                    var consigment = worksheet.Cells[row, 5].Value?.ToString();
                    var manuDate = Convert.ToDateTime(worksheet.Cells[row, 6].Value?.ToString());
                    var expDate = Convert.ToDateTime(worksheet.Cells[row, 7].Value?.ToString());
                    var price = float.Parse(worksheet.Cells[row, 8].Value?.ToString());
                    // Create a new object to hold the data
                    var data = new Consignment
                    {
                        MedicineId = medId,
                        ConsignmentNumber = consigment,
                        ManufactureDate = manuDate,
                        ExpiredDate = expDate,
                        Price = price,
                        Quantity = quantity,

                    };
                    consignments.Add(data);
                }
            }
            AddListConsignmentToStorage(consignments);
            // Redirect to a success page 
            return RedirectToAction("Index");
        }
        public IActionResult ExcelHome()
        {
            return View();
        }
        /// <summary>
        /// add 1 record 1 lan
        /// tao import, save import, get lastimport
        /// tao consignment, save consignment
        /// update lastimport => save import detail
        /// tao storage, save storage
        /// case: consignmentNumber is exist: update consignment, storage, save import detail
        /// </summary>
        /// <param name="consignment"></param>
        public void AddNewConsignmentToStorage(Consignment consignment)
        {
            //add import
            var userTmp = HttpContext.Session.GetString("User");
            var user = JsonConvert.DeserializeObject<User>(userTmp);
            
            using (var transaction = _contextCampus.Database.BeginTransaction())
            {
                try
                {
                    Import import = new Import { UserId = user.UserId, ImportDate = DateTime.Now };
                    _importSv.SaveImport(import);
                    _consignmentSv.SaveConsignment(consignment);
                    var lastImp = _importSv.GetLastestImport();
                    var lastCons = _consignmentSv.GetLastestConsignment();
                    //TO DO: fix cai nay DUONGNB
                    /*Storage storage = new Storage { ImportId = lastImp.ImportId, MedicineId = consignment.MedicineId, Quantity = consignment.Quantity };
                    _storageSv.SaveStorage(storage);*/

                    lastImp.Consignments.Add(lastCons);
                    lastCons.Imports.Add(lastImp);

                    _importSv.UpdateImport(lastImp);
                    _consignmentSv.UpdateConsigment(lastCons);

                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            
        }
        public void AddListConsignmentToStorage(List<Consignment> consignments)
        {
            //add import
            var userTmp = HttpContext.Session.GetString("User");
            var user = JsonConvert.DeserializeObject<User>(userTmp);

            using (var transaction = _contextCampus.Database.BeginTransaction())
            {
                try
                {
                    Import import = new Import { UserId = user.UserId, ImportDate = DateTime.Now };
                    _importSv.SaveImport(import);
                    var lastImp = _importSv.GetLastestImport();
                    foreach (Consignment consignment in consignments)
                    {
                        consignment.Imports.Add(lastImp);
                        _consignmentSv.SaveConsignment(consignment);
                        lastImp.Consignments.Add(consignment);
                        _importSv.UpdateImport(lastImp);
                        //TO DO: fix cai nay
                        //Storage storage = new Storage { ImportId = lastImp.ImportId, MedicineId = consignment.MedicineId, Quantity = consignment.Quantity };
                        //_storageSv.SaveStorage(storage);
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }
        public IActionResult GetUnit(string medicineId)
        {
            Unit medUnit = new Unit();
            var un = _contextSHSMS.Medicines.Where(m => m.MedicineId == medicineId).Include(x => x.Unit).FirstOrDefault();
            if (un != null)
            {
                medUnit = un.Unit;
            }
            return Json(medUnit.UnitName);
        }
        public IActionResult WanningExpDate(string searchString, string sortOrder)
        {
            ViewData["MedId"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["ExpDate"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;

            dynamic models = new ExpandoObject();
            IEnumerable<Category> categories = _contextSHSMS.Categories.ToList();
            List<CustomViewStorageModel> cusModels = new List<CustomViewStorageModel>();
            if (!String.IsNullOrEmpty(searchString))
            {
                //lay danh sach storage con su dung duoc => lay ra consignment
                //kiem tra han su dung trong consigment
                //gan vao cusModels
                var med = _contextSHSMS.Medicines.Where(x => x.MedicineName.ToLower().Contains(searchString.ToLower())).Include(x => x.Unit).FirstOrDefault();
                var medId = med.MedicineId;
                var cons = _consignmentSv.GetCanUseConsignmentsByMedicineId(medId);
                foreach (var item in cons)
                {
                    var manufactureDate = item.ManufactureDate;
                    var expDate = item.ExpiredDate;
                    var today = DateTime.Now;
                    TimeSpan totalTime = (TimeSpan)(expDate - manufactureDate);
                    TimeSpan remainingTime = (TimeSpan)(expDate - today);
                    int totalDay = totalTime.Days;
                    int remainingDay = remainingTime.Days;
                    if (remainingDay <= totalDay / 3)
                    {
                        var import = item.Imports.ToList();
                        foreach (var imp in import)
                        {
                            var impId = imp.ImportId;
                            //TO DO: here
                            /*var stock = _storageSv.GetStorageByImportAndMedicine(impId, medId);
                            if (stock != null)
                            {
                                if (stock.Quantity > 0)
                                {
                                    cusModels.Add(new CustomViewStorageModel { Consignment = item, Medicine = med, Storage = stock });
                                }
                            }*/

                        }
                    }
                }
            }
            else
            {
                var cons = _consignmentSv.GetCanUseConsignments();
                foreach (var item in cons)
                {
                    var manufactureDate = item.ManufactureDate;
                    var expDate = item.ExpiredDate;
                    var today = DateTime.Now;
                    TimeSpan totalTime = (TimeSpan)(expDate - manufactureDate);
                    TimeSpan remainingTime = (TimeSpan)(expDate - today);
                    int totalDay = totalTime.Days;
                    int remainingDay = remainingTime.Days;
                    if (remainingDay <= totalDay / 3)
                    {
                        var medId = item.MedicineId;
                        var med = _contextSHSMS.Medicines.Where(x => x.MedicineId == medId).Include(x => x.Unit).FirstOrDefault();
                        var import = item.Imports.ToList();
                        foreach (var imp in import)
                        {
                            //TO DO: here
                            var impId = imp.ImportId;
                            /*var stock = _storageSv.GetStorageByImportAndMedicine(impId, medId);
                            if (stock != null)
                            {
                                if (stock.Quantity > 0)
                                {
                                    cusModels.Add(new CustomViewStorageModel { Consignment = item, Medicine = med, Storage = stock });
                                }
                            }*/

                        }
                    }
                }
            }


            models.Category = categories;
            if (cusModels.Count > 0)
            {
                switch (sortOrder)
                {
                    case "name_desc":
                        models.Storage = cusModels.OrderByDescending(x => x.Medicine.MedicineId);
                        break;
                    case "Date":
                        models.Storage = cusModels.OrderBy(x => x.Consignment.ExpiredDate);
                        break;
                    case "date_desc":
                        models.Storage = cusModels.OrderByDescending(x => x.Consignment.ExpiredDate);
                        break;
                    default:
                        models.Storage = cusModels.OrderBy(x => x.Medicine.MedicineId);
                        break;
                }
            }
            return View(models);
        }
        //tieu huy 1 lo thuoc        
        /// <summary>
        /// tu id cua lo thuoc, tim ra lo thuoc nay trong storage, dua quantity ve 0, them export moi voi export type la tieu huy
        /// </summary>
        /// <param name="consId"> id cua lo thuoc muon tieu huy</param>
        /// <returns></returns>
        public IActionResult AnnihilateConsignment(int consId)
        {
            var cons = _consignmentSv.GetConsignmentById(consId);
            var medId = cons.MedicineId;
            var imports = cons.Imports.ToList();
            //TO DO: here
            /*foreach (var import in imports)
            {
                var impId = import.ImportId;
                //lay ra storage can tieu huy
                var storage = _storageSv.GetStorageByImportAndMedicine(impId, medId);
                //tao export voi export type tieu huy
                var userTmp = HttpContext.Session.GetString("User");
                var user = JsonConvert.DeserializeObject<User>(userTmp);
                Export export = new Export();
                export.ExportTypeId = 3;
                export.UserId = user.UserId;
                export.ExportDate = DateTime.Now;


                _exportSv.SaveExport(export);

                var exp = _exportSv.GetExports().ToList();
                var ex = exp.Last();
                ExportDetail detail = new ExportDetail { ExportId = ex.ExportId, Quantity = storage.Quantity, StorageId = storage.StorageId };
                _exportDetailSv.SaveExportDetail(detail);
                //set so luong trong kho ve 0
                storage.Quantity = 0;
                _storageSv.UpdateStorage(storage);
            }*/
            return RedirectToAction(nameof(ViewStorage));
        }
        //xem lo thuoc ton kho da het han, cho duoc tieu huy
        /// <summary>
        /// thuoc het han cho duoc tieu huy
        /// lay danh sach thuoc het han, danh sach thuoc da tieu huy(lay tu export co export type tieu huy)
        /// bo cac data trung nhau giua 2 list
        /// </summary>
        /// <returns></returns>
        public IActionResult WaitingForDestructionList()
        {
            dynamic models = new ExpandoObject();
            //list het han
            List<Consignment> outOfDateConsignments = _consignmentSv.GetOutOfDateConsignments().ToList();
            var exports = _exportSv.FindExportsByExportTypeId(3).ToList();
            //list tieu huy
            List<Consignment> annihilatedConsignment = new List<Consignment>();
            foreach (var export in exports)
            {
                var exportId = export.ExportId;
                var exportDetails = _exportDetailSv.GetExportDetailsByExportId(exportId).ToList();
                if (exportDetails != null || exportDetails.Count > 0)
                {
                    foreach (var exportDetail in exportDetails)
                    {
                        var storageId = exportDetail.StorageId;
                        var storage = _storageSv.FindStorageById(storageId);
                        //TO DO: fix cai nay
                        /*if (storage != null)
                        {
                            var impId = storage.ImportId;
                            //lay ra impot theo import id
                            var import = _importSv.GetImportById((int)impId);
                            //lay ra consignment
                            if (import != null)
                            {
                                var consignments = import.Consignments.ToList();
                                if (consignments != null || consignments.Count > 0)
                                {
                                    foreach (Consignment consignment in consignments)
                                    {
                                        annihilatedConsignment.Add(consignment);
                                    }
                                }
                            }
                        }*/
                    }
                }
            }
            //loai bo cac lo het han da duoc tieu huy tu cac lo da het han, lay ra duoc nhung lo het han chua duoc tieu huy
            if (annihilatedConsignment.Count > outOfDateConsignments.Count)
            {
                foreach (var item in outOfDateConsignments)
                {
                    if (annihilatedConsignment.Contains(item))
                    {
                        outOfDateConsignments.Remove(item);
                    }
                }
            }
            else
            {
                foreach (var item in annihilatedConsignment)
                {
                    if (outOfDateConsignments.Contains(item))
                    {
                        outOfDateConsignments.Remove(item);
                    }
                }
            }

            List<CustomViewStorageModel> cusModels = new List<CustomViewStorageModel>();
            foreach (var item in outOfDateConsignments)
            {
                var medId = item.MedicineId;
                var med = _contextSHSMS.Medicines.Where(x => x.MedicineId == medId).Include(x => x.Unit).FirstOrDefault();
                var import = item.Imports.ToList();
                foreach (var imp in import)
                {
                    //TO DO:here DUONGNB
                    /*var impId = imp.ImportId;
                    var stock = _storageSv.GetStorageByImportAndMedicine(impId, medId);
                    if (stock != null)
                    {
                        if (stock.Quantity > 0)
                        {
                            cusModels.Add(new CustomViewStorageModel { Consignment = item, Medicine = med, Storage = stock });
                        }
                    }*/
                }
            }
            if (cusModels.Count > 0)
            {
                models.WaitingList = cusModels;
            }
            return View(models);
        }
        //danh sach thuoc da tieu huy
        public IActionResult AnnihilatedList()
        {
            dynamic models = new ExpandoObject();
            //thuoc da tieu huy dua theo export type
            var exports = _exportSv.FindExportsByExportTypeId(3).ToList();
            //lay ra lo tuong ung
            List<Consignment> annihilatedConsignment = new List<Consignment>();
            List<CustomAnihilatedModel> cusModels = new List<CustomAnihilatedModel>();
            foreach (var export in exports)
            {
                var exportId = export.ExportId;
                var exportDetails = _exportDetailSv.GetExportDetailsByExportId(exportId).ToList();
                if (exportDetails != null || exportDetails.Count > 0)
                {
                    foreach (var exportDetail in exportDetails)
                    {
                        var storageId = exportDetail.StorageId;
                        var storage = _storageSv.FindStorageById(storageId);
                        if (storage != null)
                        {
                            //TO DO: fix cai nay DUONGNB
                            /*var impId = storage.ImportId;
                            //lay ra impot theo import id
                            var import = _importSv.GetImportById((int)impId);
                            //lay ra consignment
                            if (import != null)
                            {
                                var consignments = import.Consignments.ToList();
                                if (consignments != null || consignments.Count > 0)
                                {
                                    foreach (Consignment consignment in consignments)
                                    {
                                        var medId = consignment.MedicineId;
                                        var med = _contextSHSMS.Medicines.Where(x => x.MedicineId == medId).Include(x => x.Unit).FirstOrDefault();
                                        cusModels.Add(new CustomAnihilatedModel { Consignment = consignment, Medicine = med, Export = export, ExportDetail = exportDetail });
                                    }
                                }
                            }*/
                        }
                    }
                }
            }

            if (cusModels.Count > 0)
            {
                models.List = cusModels;
            }
            return View(models);
        }
        //xuat thuoc theo don
        /// <summary>
        /// kiem tra tong so luong con lai trong kho cua loai thuoc duoc chon. neu tra ve true thi thuc hien buoc tiep theo(tru thuoc trong kho)
        /// </summary>
        /// <param name="medId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public bool CheckAvaiableInStorage(string medId, int quantity)
        {
            //tổng số lượng thuốc có id medId còn lại trong kho 
            int inStock = 0;
            //lay danh sach cac lo thuoc dua theo medid,
            var cons = _consignmentSv.GetCanUseConsignmentsByMedicineId(medId);
            foreach (var item in cons)
            {
                //lay ra lo thuoc do con lai trong kho
                var import = item.Imports.ToList();
                foreach (var imp in import)
                {
                    var impId = imp.ImportId;
                    //lấy ra storage
                    //TO DO: here
                    /*var stock = _storageSv.GetStorageByImportAndMedicine(impId, medId);
                    if (stock != null)
                    {
                        inStock += (int)stock.Quantity;
                    }*/
                }
            }
            if (inStock >= quantity)
            {
                return true;
            }
            else return false;
            
        }
        /// <summary>
        /// thuc hien tru thuoc trong kho va dua ra so tien phai tra cho loai thuoc duoc chon
        /// </summary>
        /// <param name="medId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        //public int DispenseMed(string medId, int quantity)
        //{
        //    //số tiền phải trả cho loại thuốc có id là medId với số lượng quantity
        //    int totalPrice = 0;
        //    //số thuốc còn thiếu để hoàn thành 1 đơn thuốc(Trường hợp lấy ra từ nhiều lô khác nhau)
        //    int quantityRemain = quantity;
        //    //lay danh sach cac lo thuoc dua theo medid,
        //    //danh sách các lô thuốc còn sử dụng theo medId
        //    var cons = _consignmentSv.GetCanUseConsignmentsByMedicineId(medId);
        //    //lay cac lo co han ngan hon dua len dau
        //    cons = cons.OrderBy(x => x.ExpiredDate);
        //    //duyệt các lô thuốc vừa lấy
        //    foreach (var item in cons)
        //    {
        //        //giá thuốc của lô tính theo đơn vị nhỏ nhất
        //        //gia thuoc tinh theo 1 don vi nho nhat cua thuoc trong 1 lo
        //        int unitPrice = (int)(item.Price / item.Quantity);
        //        //lấy được import id để tìm storage tương ứng
        //        var import = item.Imports.ToList();
        //        foreach (var imp in import)
        //        {
        //            var impId = imp.ImportId;
        //            //lấy ra lô tương ứng trong kho
        //            var stock = _storageSv.GetStorageByImportAndMedicine(impId, medId);
        //            if (stock != null)
        //            {
        //                //lô đó trong storage còn đủ số lượng(chỉ lấy thuốc từ 1 lô)
        //                if (stock.Quantity >= quantityRemain)
        //                {
        //                    totalPrice += unitPrice * quantityRemain;
        //                    //dat lai so luong thuoc con lai trong kho
        //                    stock.Quantity -= quantityRemain;
        //                    //đã lấy đủ số lượng => thiếu 0 viên
        //                    quantityRemain = 0;
        //                    _storageSv.UpdateStorage(stock);
        //                    return totalPrice;
        //                }
        //                //lấy từ nhiều storage
        //                else
        //                {
        //                    //giá thuốc của lô đó
        //                    totalPrice += (int)(unitPrice * stock.Quantity);
        //                    //so luong thuoc con thieu(phai lay o storage khac)
        //                    quantityRemain = (int)(quantity - stock.Quantity);
        //                    //lấy hết thuốc còn lại trong kho của lô đó(toàn bộ quantity của storage)
        //                    stock.Quantity = 0;
        //                    _storageSv.UpdateStorage(stock);
        //                }
        //            }
        //        }
        //    }
        //    //trả về tổng số tiền phải trả của lượng thuốc vừa được cấp
        //    return totalPrice;
        //}
    }
}
