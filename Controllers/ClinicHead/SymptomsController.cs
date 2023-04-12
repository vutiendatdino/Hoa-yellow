using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SP23_G21_SHSMS.Models.Campuses;
using SP23_G21_SHSMS.Repositories.Campus_Repository.SymptomRepo;
using SP23_G21_SHSMS.Services.Campus_Services.SymptomServices;

namespace SP23_G21_SHSMS.Controllers.ClinicHead
{
    public class SymptomsController : Controller
    {
        private readonly DbCampusContext _context;
        private ISymptomSv _symptomSv;

        public SymptomsController(DbCampusContext context)
        {
            _context = context;
            _symptomSv = new SymptomSvImpl(_context);
        }

        // GET: Symptoms
        public async Task<IActionResult> Index()
        {
            var symptoms = _symptomSv.GetSymptoms();
            return symptoms != null ? View(symptoms) : Problem("Cant get list symptom!");
        }

        // GET: Symptoms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Symptoms == null)
            {
                return NotFound();
            }

            var symptom = _symptomSv.FindSymptomsById(id.Value);
            if (symptom == null)
            {
                return NotFound();
            }

            return View(symptom);
        }

        // GET: Symptoms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Symptoms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SymptomId,SymptomName")] Symptom symptom)
        {
            if (ModelState.IsValid)
            {
                _symptomSv.SaveSymptom(symptom);
                return RedirectToAction(nameof(Index));
            }
            return View(symptom);
        }

        // GET: Symptoms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Symptoms == null)
            {
                return NotFound();
            }

            var symptom = _symptomSv.FindSymptomsById(id.Value);
            if (symptom == null)
            {
                return NotFound();
            }
            return View(symptom);
        }

        // POST: Symptoms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SymptomId,SymptomName")] Symptom symptom)
        {
            if (id != symptom.SymptomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (_symptomSv.UpdateSymptom(id, symptom))
                    return RedirectToAction(nameof(Index));
                return NotFound();
            }
            return View(symptom);
        }

        // GET: Symptoms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Symptoms == null)
            {
                return NotFound();
            }

            var symptom = _symptomSv.FindSymptomsById(id.Value);
            if (symptom == null)
            {
                return NotFound();
            }
            return View(symptom);
        }

        // POST: Symptoms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Symptoms == null)
            {
                return Problem("Entity set 'DbCampusContext.Symptoms'  is null.");
            }
            var symptom = _symptomSv.FindSymptomsById(id);
            if (symptom != null)
            {
                _symptomSv.RemoveSymptom(symptom);
            }
            return RedirectToAction(nameof(Index));
        }

        //private bool SymptomExists(int id)
        //{
        //    return (_context.Symptoms?.Any(e => e.SymptomId == id)).GetValueOrDefault();
        //}
    }
}
