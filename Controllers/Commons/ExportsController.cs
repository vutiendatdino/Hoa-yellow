using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SP23_G21_SHSMS.Models.Campuses;
using SP23_G21_SHSMS.Models.SHSMS;
using SP23_G21_SHSMS.Services.Campus_Services.ExportServices;
using SP23_G21_SHSMS.Services.Campus_Services.MedicalRecordServices;
using SP23_G21_SHSMS.Services.Campus_Services.StudentServices;
using SP23_G21_SHSMS.Services.Campus_Services.SymptomServices;
using SP23_G21_SHSMS.Services.Campus_Services.TreatmentServices;
using SP23_G21_SHSMS.Services.SHSMS_Services.CategoryServices;
using SP23_G21_SHSMS.Services.SHSMS_Services.DiseaseServices;
using SP23_G21_SHSMS.Services.SHSMS_Services.MedicineServices;
using SP23_G21_SHSMS.Services.SHSMS_Services.UserServices;

namespace SP23_G21_SHSMS.Controllers.Commons
{
    public class ExportsController : Controller
    {
        private DbCampusContext _campusContext;
        private DbShsmsContext _shsmsContext;
        private IExportSv _exportSv;
        private IMedicalRecordSv _medicalRecordSv;
        private IMedicineSv _medicineSv;
        private ISymptomSv _symptomSv;
        private IStudentSv _studentSv;
        private IDiseaseSv _diseaseSv;
        private IUserSv _userSv;
        private ITreatmentSv _treatmentSv;
        private ICategorySv _categorySv;
        private dynamic models;
        public ExportsController(DbCampusContext campusContext, DbShsmsContext shsmsContext)
        {
            _campusContext = campusContext;
            _shsmsContext = shsmsContext;
            _exportSv = new ExportSvImpl(_campusContext);
            _medicalRecordSv = new MedicalRecordSvImpl(_campusContext);
            _symptomSv = new SymptomSvImpl(_campusContext);
            _medicineSv = new MedicineSvImpl(_shsmsContext);
            _studentSv = new StudentSvImpl(_campusContext);
            _diseaseSv = new DiseaseSvImpl(_shsmsContext);
            _userSv = new UserSvImpl(_shsmsContext);
            _treatmentSv = new TreatmentSvImpl(_campusContext);
            _categorySv = new CategorySvImpl(_shsmsContext);
            models = new ExpandoObject();
        }

        // GET: Exports
        public async Task<IActionResult> Index()
        {
            var dbCampusContext = _campusContext.Exports.Include(e => e.MedicalRecords);
            return View(await dbCampusContext.ToListAsync());
        }

        // GET: Exports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _campusContext.Exports == null)
            {
                return RedirectToAction(nameof(Index));
            }

            //TODO: Gọi đến exports services
            var export = await _campusContext.Exports
                .Include(e => e.MedicalRecords)
                .Include(e => e.ExportType)
                .FirstOrDefaultAsync(m => m.ExportId == id);

            if (export == null)
            {
                return NotFound();
            }

            MedicalRecord medicalRecord = _medicalRecordSv.GetMedicalRecordById(export.MedicalRecordsId.Value);
            Student student = _studentSv.FindStudentByRollNumber(medicalRecord.RollNumber);
            //TODO: Gọi đến exportDetails services (chưa sửa đc services và repo vì có commits on change file này)
            IEnumerable<ExportDetail> exportDetails = _campusContext.ExportDetails
                .Include(e => e.Export)
                .Include(e => e.Storage)
                .Where(e => e.ExportId == export.ExportId)
                .ToList();
            IEnumerable<Medicine> medicines = _medicineSv.GetMedicines();
            IEnumerable<User> users = _userSv.GetUsers();

            models.MedicalRecord = medicalRecord;
            models.Users = users;
            models.Student = student;
            models.Export = export;
            models.ExportDetails = exportDetails;
            models.Medicines = medicines;
            return View(models);
        }

        // GET: Exports/Create
        public IActionResult Create(int? id)
        {
            //id này là của medicalRecord
            if (id != null)
            {
                var record = _medicalRecordSv.GetMedicalRecordById(id.Value);
                models.Medicines = _medicineSv.GetMedicines();
                models.Students = _studentSv.GetStudents();
                models.Users = _userSv.GetUsers();
                models.Symptoms = record.Symptoms;
                models.Diseases = _diseaseSv.GetDiseases();
                models.MedicalRecord = record;
                models.Treatments = _treatmentSv.GetTreatments();
                models.Export = new Export();
                models.ExportDetail = new ExportDetail();
                List<Category> categories = new List<Category>(_categorySv.GetCategories());
                models.ListOfCategory = categories;
                var export = new Export();
                export.MedicalRecordsId = id.Value;
                ViewData["MedicalRecordsId"] = new SelectList(_medicalRecordSv.GetMedicalRecords(), "MedicalRecordsId", "MedicalRecordsId", export.MedicalRecordsId);
            }
            else
            {
                ViewData["MedicalRecordsId"] = new SelectList(_campusContext.MedicalRecords, "MedicalRecordsId", "MedicalRecordsId");
            }
            return View(models);
        }

        // POST: Exports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string txtRecordId, string txtAmount, string txtNote)
        {
            var userTmp = HttpContext.Session.GetString("User");
            User user = JsonConvert.DeserializeObject<User>(userTmp);
            Export export = new Export();
            var selectedMedicines = Request.Form["slMedicine"];
            var selectedQuantity = Request.Form["txtQuantity"];
            var selectedDirection = Request.Form["txtDirection"];
            int checkQuantity;
            for (int i = 0; i < selectedQuantity.Count; i++)
            {
                if (int.TryParse(selectedQuantity[i], out checkQuantity))
                    continue;
                else
                {
                    //TODO: Message Error
                    break;
                }
            }

            IDictionary<string, int> dictMedicinesQuantity = new Dictionary<string, int>();
            for (int i = 0; i < selectedMedicines.Count; i++)
            {
                int quantity;
                if (!dictMedicinesQuantity.ContainsKey(selectedMedicines[i]))
                {
                    dictMedicinesQuantity.Add(selectedMedicines[i], int.Parse(selectedQuantity[i]));
                }
                else
                {
                    dictMedicinesQuantity[selectedMedicines[i]] += int.Parse(selectedQuantity[i]);
                }
            }

            IDictionary<string, string> dictMedicinesDirection = new Dictionary<string, string>();
            for (int i = 0; i < selectedMedicines.Count; i++)
            {
                if (!dictMedicinesDirection.ContainsKey(selectedMedicines[i]))
                {
                    dictMedicinesDirection.Add(selectedMedicines[i], selectedDirection[i]);
                }
            }

            float total = 0;
            foreach (var item in dictMedicinesQuantity)
            {
                int quantity = item.Value;
                var medicine = _medicineSv.GetMedicineById(item.Key);

                //TODO: Lấy danh sách kho có thuốc có thuốc cần lấy
                var storages = GetStoragesByMedicineId(item.Key);
                int quantityTmp = quantity;

                List<Storage> listStorage = new List<Storage>();

                //TODO: kiểm tra xem tổng số lượng có sản phẩm cần tìm trong kho có >= quantity ko; nếu có thì thực hiện, ko thì phải báo lỗi
                //if(service storage get quantity of all storage have medicineId >= quantity)
                foreach (Storage storage in storages)
                {
                    if (storage.Quantity - quantity >= 0)
                    {
                        listStorage.Add(storage);
                        quantity = 0;
                        break;
                    }
                    else
                    {
                        listStorage.Add(storage);
                        quantity -= storage.Quantity is null ? 0 : storage.Quantity.Value;
                    }
                }

                foreach (Storage storage in listStorage)
                {
                    ExportDetail exportDetail = new ExportDetail();
                    if (quantityTmp - storage.Quantity > 0)
                    {
                        exportDetail.Quantity = storage.Quantity;
                        quantityTmp -= storage.Quantity is null ? 0 : (int)storage.Quantity.Value;
                        //TODO:update quantity trong storage
                        storage.Quantity = 0;
                    }
                    else
                    {
                        exportDetail.Quantity = quantityTmp;
                        //TODO:update quantity trong storage
                        storage.Quantity -= quantityTmp;
                        quantityTmp = 0;
                    }

                    exportDetail.StorageId = storage.StorageId;

                    //TODO: Lấy giá tiền của thuốc khi nhập
                    //TODO: Lấy giá từng loại thuốc khi nhập!
                    if (medicine.IsFree == true)
                    {
                        exportDetail.Price = 0;
                    }
                    else
                    {
                        exportDetail.Price = 1300;
                    }
                    total += exportDetail.Price.Value * exportDetail.Quantity.Value;
                    exportDetail.Direction = string.IsNullOrEmpty(dictMedicinesDirection[storage.MedicineId]) ? medicine.Direction : dictMedicinesDirection[storage.MedicineId];
                    export.ExportDetails.Add(exportDetail);
                }
            }

            int userId, medicalRecordId;
            float amount;
            string note;

            export.UserId = user.UserId;
            if (int.TryParse(txtRecordId, out medicalRecordId))
                export.MedicalRecordsId = medicalRecordId;
            //if (float.TryParse(txtAmount, out amount))
                export.Amount = total;
            export.ExportTypeId = 1;
            if (!string.IsNullOrEmpty(txtNote))
                export.Note = txtNote;
            export.Status = true;
            export.ExportDate = DateTime.Now;
            _exportSv.SaveExport(export);
            //TODO: Nếu success thì update storage!!!

            return RedirectToAction("Details", "MedicalRecords", new { id = txtRecordId });
        }

        // GET: Exports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _campusContext.Exports == null)
            {
                return NotFound();
            }

            var export = await _campusContext.Exports.FindAsync(id);
            if (export == null)
            {
                return NotFound();
            }
            ViewData["MedicalRecordsId"] = new SelectList(_campusContext.MedicalRecords, "MedicalRecordsId", "MedicalRecordsId", export.MedicalRecordsId);
            return View(export);
        }

        // POST: Exports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExportId,UserId,MedicalRecordsId,Freight,ExportDate,Note,Status")] Export export)
        {
            if (id != export.ExportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _campusContext.Update(export);
                    await _campusContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExportExists(export.ExportId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MedicalRecordsId"] = new SelectList(_campusContext.MedicalRecords, "MedicalRecordsId", "MedicalRecordsId", export.MedicalRecordsId);
            return View(export);
        }

        // GET: Exports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _campusContext.Exports == null)
            {
                return NotFound();
            }

            var export = await _campusContext.Exports
                .Include(e => e.MedicalRecords)
                .FirstOrDefaultAsync(m => m.ExportId == id);
            if (export == null)
            {
                return NotFound();
            }

            return View(export);
        }

        // POST: Exports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_campusContext.Exports == null)
            {
                return Problem("Entity set 'DbCampusContext.Exports'  is null.");
            }
            var export = await _campusContext.Exports.FindAsync(id);
            if (export != null)
            {
                _campusContext.Exports.Remove(export);
            }

            await _campusContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Test()
        {
            List<Category> categories = new List<Category>(_categorySv.GetCategories());
            //categories.Insert(0, new Category { CategoryId = 0, CategoryName = "Select" });
            ViewBag.ListOfCategory = categories;
            var s = Json(categories);
            return View();
        }

        [HttpPost]
        public IActionResult TestPost()
        {
            return View();
        }

        public JsonResult GetMedicines(int categoryId)
        {
            List<Medicine> medicines = new List<Medicine>(_medicineSv.GetMedicinesByCategory(categoryId));
            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };
            //return Json(new SelectList(medicines, "MedicineId", "MedicineName"), jsonSettings);
            return Json(medicines, jsonSettings);
        }

        public JsonResult GetMedicine(string medicineId)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };
            //return Json(new SelectList(medicines, "MedicineId", "MedicineName"), jsonSettings);
            return Json(_medicineSv.GetMedicineById(medicineId), jsonSettings);
        }
        //Services storage:
        //TO DO: fix cai nay
        private IEnumerable<Storage> GetStoragesByMedicineId(string medicineId)
        {
            //DbCampusContext _context = new DbCampusContext();
            //var storages = _context.Storages.Include(s => s.Import).ThenInclude(i => i.Consignments).Where(s => s.MedicineId.ToLower().Equals(medicineId.ToLower())).ToList();
            //return storages.Count == 0 ? Enumerable.Empty<Storage>() : storages;
            return null;
        }
        private bool ExportExists(int id)
        {
            return (_campusContext.Exports?.Any(e => e.ExportId == id)).GetValueOrDefault();
        }
    }
}
