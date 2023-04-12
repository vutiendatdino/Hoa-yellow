using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SP23_G21_SHSMS.Models.Campuses;
using SP23_G21_SHSMS.Models.SHSMS;
using SP23_G21_SHSMS.Services.Campus_Services.MedicalRecordServices;
using SP23_G21_SHSMS.Services.Campus_Services.StudentServices;
using SP23_G21_SHSMS.Services.Campus_Services.SymptomServices;
using SP23_G21_SHSMS.Services.Campus_Services.TreatmentServices;
using SP23_G21_SHSMS.Services.SHSMS_Services.DiseaseServices;
using SP23_G21_SHSMS.Services.SHSMS_Services.UserServices;
using System.Dynamic;

namespace SP23_G21_SHSMS.Controllers.Commons
{
    public class MedicalRecordsController : Controller
    {
        private DbShsmsContext _shsmsContext;
        private DbCampusContext _campusContext;
        private ISymptomSv _symptomSv;
        private IStudentSv _studentSv;
        private IMedicalRecordSv _medicalRecordSv;
        private IDiseaseSv _diseaseSv;
        private IUserSv _userSv;
        private ITreatmentSv _treatmentSv;
        private dynamic models;
        public MedicalRecordsController(DbShsmsContext shsmsContext, DbCampusContext campusContext)
        {
            _shsmsContext = shsmsContext;
            _campusContext = campusContext;
            _symptomSv = new SymptomSvImpl(_campusContext);
            _studentSv = new StudentSvImpl(_campusContext);
            _medicalRecordSv = new MedicalRecordSvImpl(_campusContext);
            _diseaseSv = new DiseaseSvImpl(_shsmsContext);
            _userSv = new UserSvImpl(_shsmsContext);
            _treatmentSv = new TreatmentSvImpl(_campusContext);
            models = new ExpandoObject();
        }
        public IActionResult Index()
        {
            var userTmp = HttpContext.Session.GetString("User");
            if (userTmp == null)
                return RedirectToAction("Index", "Login");
            //User user = JsonConvert.DeserializeObject<User>(userTmp);
            //models.User = user;
            //models.Users = _userSv.GetUsers();
            //models.Symptoms = _symptomSv.GetSymptoms();
            //models.Diseases = _diseaseSv.GetDiseases();
            //models.Students = _studentSv.GetStudents();
            //models.Treatments = _treatmentSv.GetTreatments();
            //models.MedicalRecords = _medicalRecordSv.GetMedicalRecords();
            //return View(models);
            return View();
        }
        [HttpGet]
        public IActionResult GetAllRecords()
        {
            var userTmp = HttpContext.Session.GetString("User");
            if (userTmp == null)
                return RedirectToAction("Index", "Login");
            User user = JsonConvert.DeserializeObject<User>(userTmp);
            models.User = user;
            models.Users = _userSv.GetUsers();
            models.Symptoms = _symptomSv.GetSymptoms();
            models.Diseases = _diseaseSv.GetDiseases();
            models.Students = _studentSv.GetStudents();
            models.Treatments = _treatmentSv.GetTreatments();
            models.MedicalRecords = _medicalRecordSv.GetMedicalRecords();
            return View(models);
        }
        [HttpGet]
        public IActionResult GetAllAnonymousRecords()
        {
            var userTmp = HttpContext.Session.GetString("User");
            if (userTmp == null)
                return RedirectToAction("Index", "Login");
            User user = JsonConvert.DeserializeObject<User>(userTmp);
            models.User = user;
            models.Users = _userSv.GetUsers();
            models.Symptoms = _symptomSv.GetSymptoms();
            models.Diseases = _diseaseSv.GetDiseases();
            models.Student = _studentSv.FindStudentByRollNumber("Temp");
            models.Treatments = _treatmentSv.GetTreatments();
            models.MedicalRecords = _medicalRecordSv.GetMedicalRecordsByRollNumber("Temp");
            return View(models);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Index));
            var record = _medicalRecordSv.GetMedicalRecordById(id.Value);
            if (record == null)
                return NotFound();
            models.Users = _userSv.GetUsers();
            models.Diseases = _diseaseSv.GetDiseases();
            models.Treatments = _treatmentSv.GetTreatments();
            models.Student = _studentSv.FindStudentByRollNumber(record.RollNumber);
            models.MedicalRecord = record;
            return View(models);
        }

        public IActionResult Create()
        {
            var userTmp = HttpContext.Session.GetString("User");
            if (userTmp == null)
            {
                return RedirectToAction("Index", "Login");
            }
            //User user = JsonConvert.DeserializeObject<User>(userTmp);
            models.Record = new MedicalRecord();
            models.Symptoms = _symptomSv.GetSymptoms();
            models.Diseases = _diseaseSv.GetDiseases();
            models.Treatments = _treatmentSv.GetTreatments();
            ViewData["DiseasesList"] = new SelectList(_diseaseSv.GetDiseases(), "DiseasesId", "DiseasesName");
            ViewData["TreatmentList"] = new SelectList(_treatmentSv.GetTreatments(), "TreatmentId", "TreatmentName");

            return View(models);
        }

        public IActionResult CreateTemp()
        {
            var userTmp = HttpContext.Session.GetString("User");
            if (userTmp == null)
            {
                return RedirectToAction("Index", "Login");
            }
            models.Record = new MedicalRecord();
            models.Symptoms = _symptomSv.GetSymptoms();
            models.Diseases = _diseaseSv.GetDiseases();
            models.Treatments = _treatmentSv.GetTreatments();
            ViewData["DiseasesList"] = new SelectList(_diseaseSv.GetDiseases(), "DiseasesId", "DiseasesName");
            ViewData["TreatmentList"] = new SelectList(_treatmentSv.GetTreatments(), "TreatmentId", "TreatmentName");
            return View(models);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTemp(string txtSys, string txtDia, string txtBodyTemperature, string txtHeartRate, string txtSpo2, string cbSymptom, string txtSymptomNote, string slDisease, string txtDiseaseNote, string slTreatment, string txtTreatmentNote, string cbTreatment, string txtNote)
        {
            var userTmp = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userTmp))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                User user = JsonConvert.DeserializeObject<User>(userTmp);
                MedicalRecord medicalRecord = new MedicalRecord();
                medicalRecord.UserId = user.UserId;
                medicalRecord.Sys = int.Parse(txtSys);
                medicalRecord.Dia = int.Parse(txtDia);
                medicalRecord.BodyTemperature = double.Parse(txtBodyTemperature);
                medicalRecord.HeartRate = int.Parse(txtHeartRate);
                medicalRecord.Spo2 = int.Parse(txtSpo2);
                var cbxValues = Request.Form["cbSymptom"];
                foreach (var value in cbxValues)
                {
                    int symptomId;
                    if (int.TryParse(value, out symptomId))
                        medicalRecord.Symptoms.Add(_symptomSv.FindSymptomsById(symptomId));
                }
                medicalRecord.SymptomNote = string.IsNullOrEmpty(txtSymptomNote) == true ? null : txtSymptomNote;

                medicalRecord.DiseaseId = int.Parse(slDisease);
                medicalRecord.DiseaseNote = string.IsNullOrEmpty(txtDiseaseNote) == true ? null : txtDiseaseNote;

                medicalRecord.TreatmentId = int.Parse(slTreatment);
                medicalRecord.TreatmentNote = string.IsNullOrEmpty(txtTreatmentNote) == true ? null : txtTreatmentNote;

                medicalRecord.RollNumber = "Temp";
                medicalRecord.Note = txtNote;
                medicalRecord.ExaminationTime = DateTime.Now;
                medicalRecord.Status = true;

                int medicalRecordId = _medicalRecordSv.SaveMedicalRecord(medicalRecord);
                if (medicalRecordId != -1)
                {
                    int cbxTreatment;
                    if (int.TryParse(cbTreatment, out cbxTreatment))
                    {
                        return RedirectToAction("Create", "Exports", new { id = medicalRecordId });
                    }
                    else
                    {
                        return RedirectToAction(nameof(Details), new { id = medicalRecordId });
                    }
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string txtStudent, string txtSearch, string txtSys, string txtDia, string txtBodyTemperature, string txtHeartRate, string txtSpo2, string cbSymptom, string txtSymptomNote, string slDisease, string txtDiseaseNote, string slTreatment, string txtTreatmentNote, string txtNote)
        {
            var userTmp = HttpContext.Session.GetString("User");
            User user = JsonConvert.DeserializeObject<User>(userTmp);

            MedicalRecord medicalRecord = new MedicalRecord();
            medicalRecord.UserId = user.UserId;
            medicalRecord.Sys = int.Parse(txtSys);
            medicalRecord.Dia = int.Parse(txtDia);
            medicalRecord.BodyTemperature = double.Parse(txtBodyTemperature);
            medicalRecord.HeartRate = int.Parse(txtHeartRate);
            medicalRecord.Spo2 = int.Parse(txtSpo2);

            var cbxValues = Request.Form["cbSymptom"];
            foreach (var value in cbxValues)
            {
                int symptomId;
                if (int.TryParse(value, out symptomId))
                    medicalRecord.Symptoms.Add(_symptomSv.FindSymptomsById(symptomId));
            }
            medicalRecord.SymptomNote = string.IsNullOrEmpty(txtSymptomNote) == true ? null : txtSymptomNote;

            medicalRecord.DiseaseId = int.Parse(slDisease);
            medicalRecord.DiseaseNote = string.IsNullOrEmpty(txtDiseaseNote) == true ? null : txtDiseaseNote;

            medicalRecord.TreatmentId = int.Parse(slTreatment);
            medicalRecord.TreatmentNote = string.IsNullOrEmpty(txtTreatmentNote) == true ? null : txtTreatmentNote;

            medicalRecord.RollNumber = string.IsNullOrEmpty(txtStudent) ? txtSearch : txtStudent;
            medicalRecord.Note = txtNote;
            medicalRecord.ExaminationTime = DateTime.Now;
            medicalRecord.Status = true;
            int medicalRecordId = _medicalRecordSv.SaveMedicalRecord(medicalRecord);
            if (medicalRecordId != -1)
            {
                return RedirectToAction(nameof(Details), new { id = medicalRecordId });
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            //var a = medicalRecord;
            //return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var userTmp = HttpContext.Session.GetString("User");
            if (userTmp == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (id == null)
                return RedirectToAction(nameof(Index));
            var record = _medicalRecordSv.GetMedicalRecordById(id.Value);
            if (record == null || !record.RollNumber.ToLower().Equals("Temp".ToLower()))
                return RedirectToAction(nameof(Index));

            models.Users = _userSv.GetUsers();
            models.Diseases = _diseaseSv.GetDiseases();
            models.Treatments = _treatmentSv.GetTreatments();
            models.Student = _studentSv.FindStudentByRollNumber(record.RollNumber);
            models.MedicalRecord = record;

            return View(models);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int medicalRecordId, string txtStudent)
        {
            var userTmp = HttpContext.Session.GetString("User");
            if (userTmp == null)
            {
                return RedirectToAction("Index", "Login");
            }
            MedicalRecord medicalRecord = _medicalRecordSv.GetMedicalRecordById(medicalRecordId);
            if(string.IsNullOrEmpty(txtStudent) || medicalRecord is null)
            {
                return RedirectToAction(nameof(Index));
            }
            medicalRecord.RollNumber = txtStudent;

            if (_medicalRecordSv.UpdateMedicalRecord(medicalRecordId, medicalRecord))
            {
                return RedirectToAction(nameof(Details), new {id = medicalRecordId});
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Edit), new { id = medicalRecordId });
        }

        public JsonResult GetStudent(string rollNumber)
        {
            Student student = _studentSv.FindStudentByRollNumber(rollNumber);
            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };
            return Json(student, jsonSettings);
        }
    }
}
