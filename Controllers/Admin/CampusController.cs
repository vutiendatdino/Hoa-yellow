using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SP23_G21_SHSMS.Models.SHSMS;
using SP23_G21_SHSMS.Services.Common_Services;

namespace SP23_G21_SHSMS.Controllers.Admin
{
    public class CampusController : Controller
    {
        private readonly DbShsmsContext _context;
        private readonly ConfigurationService _configService;
        public CampusController(DbShsmsContext context, ConfigurationService configService)
        {
            _context = context;
            _configService = configService;
        }

        // GET: Campus
        public async Task<IActionResult> Index()
        {
            var dbShsmsContext = _context.Campuses.Include(c => c.Edu);
            return View(await dbShsmsContext.ToListAsync());
        }

        // GET: Campus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Campuses == null)
            {
                return NotFound();
            }

            var campus = await _context.Campuses
                .Include(c => c.Edu)
                .FirstOrDefaultAsync(m => m.CampusId == id);
            if (campus == null)
            {
                return NotFound();
            }

            return View(campus);
        }

        // GET: Campus/Create
        public IActionResult Create()
        {
            ViewData["EduId"] = new SelectList(_context.Fes, "EduId", "EduName");
            return View();
        }

        // POST: Campus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CampusId,CampusName,ConnectionString,Status,EduId")] Campus campus)
        {
            if (ModelState.IsValid)
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                IConfigurationRoot configurationRoot = builder.Build();
                string dbMasterCS = configurationRoot.GetConnectionString("DB_master");
                SqlConnection myConn = new SqlConnection(dbMasterCS);
                string str = "CREATE DATABASE " + campus.CampusName + ";";
                SqlCommand myCommand = new SqlCommand(str, myConn);
                try
                {
                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if(myConn.State == ConnectionState.Open)
                        myConn.Close();
                }

                dbMasterCS += ";Initial Catalog=" + campus.CampusName;
                myConn = new SqlConnection(dbMasterCS);
                //str = "CREATE TABLE imports(importID int PRIMARY KEY IDENTITY,userID int,importDate datetime)CREATE TABLE storages(storageID int PRIMARY KEY IDENTITY(1,1),importID int,medicineID varchar(50),quantity int,FOREIGN KEY (importID) REFERENCES imports(importID))CREATE TABLE symptoms(symptomID int PRIMARY KEY IDENTITY(1,1),symptomName nvarchar(200))CREATE TABLE medicalRecords(medicalRecordsID int PRIMARY KEY IDENTITY(1,1),userID int,diseaseID int,rollNumber varchar(50),note ntext,treatments nvarchar(500),examinationTime datetime,bloodPressure int,bodyTemperature float,[status] bit)CREATE TABLE symptoms_medicalRecords(symptomID int, medicalRecordsID int, PRIMARY KEY(symptomID,medicalRecordsID),FOREIGN KEY (symptomID) REFERENCES symptoms(symptomID),FOREIGN KEY (medicalRecordsID) REFERENCES medicalRecords(medicalRecordsID))CREATE TABLE exports(exportID int PRIMARY KEY IDENTITY(1,1),userID int,medicalRecordsID int,freight real,exportDate datetime,note ntext,[status] bit,FOREIGN KEY (medicalRecordsID) REFERENCES medicalRecords(medicalRecordsID))CREATE TABLE suppliers(supplierID int PRIMARY KEY IDENTITY(1,1),supplierName nvarchar(200),[address] nvarchar(250),phoneNumber varchar(50),[status] bit)CREATE TABLE consignments(consignmentID int PRIMARY KEY IDENTITY(1,1),supplierID int,consignmentNumber nvarchar(120),FOREIGN KEY (supplierID) REFERENCES suppliers(supplierID))CREATE TABLE exportDetails(exportID int,medicineID varchar(50),consignmentID int,quantity int,price real,direction ntext, PRIMARY KEY(exportID,medicineID),FOREIGN KEY (exportID) REFERENCES exports(exportID), FOREIGN KEY (consignmentID) REFERENCES consignments(consignmentID))CREATE TABLE importDetails(importID int,medicineID varchar(50),consignmentID int,manufactureDate date,expiredDate date,quantity int,price real, PRIMARY KEY(importID,medicineID),FOREIGN KEY (importID) REFERENCES imports(importID), FOREIGN KEY (consignmentID) REFERENCES consignments(consignmentID))";
                str = "CREATE TABLE students(rollNumber varchar(50) PRIMARY KEY,campusID int,studentEmail nvarchar(150),studentName nvarchar(150),dob date,healthInsurance varchar(30),phone varchar(20),studentAddress nvarchar(500),parentPhone varchar(20),identification varchar(30))CREATE TABLE symptoms(symptomID int PRIMARY KEY IDENTITY(1,1),symptomName nvarchar(200))CREATE TABLE treatments(treatmentID int PRIMARY KEY IDENTITY(1,1),treatmentName nvarchar(200))CREATE TABLE exportType(exportTypeID int PRIMARY KEY IDENTITY(1,1),exportTypeName nvarchar(200))CREATE TABLE consignments(consignmentID int PRIMARY KEY IDENTITY(1,1),consignmentNumber nvarchar(120),medicineID varchar(50),manufactureDate datetime,expiredDate datetime,quantity int,price real)CREATE TABLE imports(importID int PRIMARY KEY IDENTITY,userID int,importDate datetime)CREATE TABLE importDetails(importID int,consignmentID int,PRIMARY KEY(importID,consignmentID),FOREIGN KEY (importID) REFERENCES imports(importID), FOREIGN KEY (consignmentID) REFERENCES consignments(consignmentID))CREATE TABLE storages(storageID int PRIMARY KEY IDENTITY(1,1),importID int,medicineID varchar(50),quantity int,FOREIGN KEY (importID) REFERENCES imports(importID))CREATE TABLE medicalRecords(medicalRecordsID int PRIMARY KEY IDENTITY(1,1),userID int,diseaseID int,rollNumber varchar(50),treatmentID int,examinationTime datetime,[sys] int,dia int,bodyTemperature float,heartRate int,spo2 int,symptomNote nvarchar(500),treatmentNote nvarchar(500),diseaseNote nvarchar(500),note ntext,[status] bit,FOREIGN KEY (rollNumber) REFERENCES students(rollNumber),FOREIGN KEY (treatmentID) REFERENCES treatments(treatmentID))CREATE TABLE symptoms_medicalRecords(symptomID int, medicalRecordsID int, PRIMARY KEY(symptomID,medicalRecordsID),FOREIGN KEY (symptomID) REFERENCES symptoms(symptomID),FOREIGN KEY (medicalRecordsID) REFERENCES medicalRecords(medicalRecordsID))CREATE TABLE exports(exportID int PRIMARY KEY IDENTITY(1,1),userID int,medicalRecordsID int,amount real,exportDate datetime,exportTypeID int,note ntext,[status] bit,FOREIGN KEY (medicalRecordsID) REFERENCES medicalRecords(medicalRecordsID),FOREIGN KEY (exportTypeID) REFERENCES exportType(exportTypeID))CREATE TABLE exportDetails(exportID int,storageID int,quantity int,price real,direction ntext, PRIMARY KEY(exportID,storageID),FOREIGN KEY (exportID) REFERENCES exports(exportID), FOREIGN KEY (storageID) REFERENCES storages(storageID))";
                myCommand = new SqlCommand(str, myConn);
                try
                {
                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open)
                        myConn.Close();
                }
                campus.ConnectionString = dbMasterCS;
                _context.Add(campus);
                await _context.SaveChangesAsync();
                _configService.AddNewConnectionString(campus.CampusName, campus.ConnectionString);
                return RedirectToAction(nameof(Index));
            }
            ViewData["EduId"] = new SelectList(_context.Fes, "EduId", "EduName", campus.EduId);
            return View(campus);
        }

        // GET: Campus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Campuses == null)
            {
                return NotFound();
            }

            var campus = await _context.Campuses.FindAsync(id);
            if (campus == null)
            {
                return NotFound();
            }
            ViewData["EduId"] = new SelectList(_context.Fes, "EduId", "EduName", campus.EduId);
            return View(campus);
        }

        // POST: Campus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CampusId,CampusName,ConnectionString,Status,EduId")] Campus campus)
        {
            if (id != campus.CampusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(campus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CampusExists(campus.CampusId))
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
            ViewData["EduId"] = new SelectList(_context.Fes, "EduId", "EduName", campus.EduId);
            return View(campus);
        }

        // GET: Campus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Campuses == null)
            {
                return NotFound();
            }

            var campus = await _context.Campuses
                .Include(c => c.Edu)
                .FirstOrDefaultAsync(m => m.CampusId == id);
            if (campus == null)
            {
                return NotFound();
            }

            return View(campus);
        }

        // POST: Campus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Campuses == null)
            {
                return Problem("Entity set 'DbShsmsContext.Campuses'  is null.");
            }
            var campus = await _context.Campuses.FindAsync(id);
            if (campus != null)
            {
                _context.Campuses.Remove(campus);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CampusExists(int id)
        {
            return _context.Campuses.Any(e => e.CampusId == id);
        }
    }
}
