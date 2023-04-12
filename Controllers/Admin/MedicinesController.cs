using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Controllers.Admin
{
    public class MedicinesController : Controller
    {
        private readonly DbShsmsContext _context;

        public MedicinesController(DbShsmsContext context)
        {
            _context = context;
        }

        // GET: Medicines
        public async Task<IActionResult> Index()
        {
            var dbShsmsContext = _context.Medicines.Include(m => m.Category).Include(m => m.Manufacture).Include(m => m.Unit);
            return View(await dbShsmsContext.ToListAsync());
        }

        // GET: Medicines/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Medicines == null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicines
                .Include(m => m.Category)
                .Include(m => m.Manufacture)
                .Include(m => m.Unit)
                .FirstOrDefaultAsync(m => m.MedicineId == id);
            if (medicine == null)
            {
                return NotFound();
            }

            return View(medicine);
        }

        // GET: Medicines/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["ManufactureId"] = new SelectList(_context.Manufactures, "ManufactureId", "ManufactureName");
            ViewData["UnitId"] = new SelectList(_context.Units, "UnitId", "UnitName");
            return View();
        }

        // POST: Medicines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MedicineId,MedicineName,ManufactureId,UnitId,CategoryId,Direction,Status")] Medicine medicine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", medicine.CategoryId);
            ViewData["ManufactureId"] = new SelectList(_context.Manufactures, "ManufactureId", "ManufactureId", medicine.ManufactureId);
            ViewData["UnitId"] = new SelectList(_context.Units, "UnitId", "UnitId", medicine.UnitId);
            return View(medicine);
        }

        // GET: Medicines/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Medicines == null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", medicine.CategoryId);
            ViewData["ManufactureId"] = new SelectList(_context.Manufactures, "ManufactureId", "ManufactureName", medicine.ManufactureId);
            ViewData["UnitId"] = new SelectList(_context.Units, "UnitId", "UnitName", medicine.UnitId);
            return View(medicine);
        }

        // POST: Medicines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MedicineId,MedicineName,ManufactureId,UnitId,CategoryId,Direction,Status")] Medicine medicine)
        {
            if (id != medicine.MedicineId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicineExists(medicine.MedicineId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", medicine.CategoryId);
            ViewData["ManufactureId"] = new SelectList(_context.Manufactures, "ManufactureId", "ManufactureId", medicine.ManufactureId);
            ViewData["UnitId"] = new SelectList(_context.Units, "UnitId", "UnitId", medicine.UnitId);
            return View(medicine);
        }

        // GET: Medicines/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Medicines == null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicines
                .Include(m => m.Category)
                .Include(m => m.Manufacture)
                .Include(m => m.Unit)
                .FirstOrDefaultAsync(m => m.MedicineId == id);
            if (medicine == null)
            {
                return NotFound();
            }

            return View(medicine);
        }

        // POST: Medicines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Medicines == null)
            {
                return Problem("Entity set 'DbShsmsContext.Medicines'  is null.");
            }
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine != null)
            {
                _context.Medicines.Remove(medicine);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicineExists(string id)
        {
          return _context.Medicines.Any(e => e.MedicineId == id);
        }
    }
}
