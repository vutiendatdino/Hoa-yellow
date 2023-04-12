using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SP23_G21_SHSMS.Models.SHSMS;
using SP23_G21_SHSMS.Services.SHSMS_Services.ContactServices;

namespace SP23_G21_SHSMS.Controllers.Admin
{
    public class ContactsController : Controller
    {
        private readonly DbShsmsContext _context;
        private IContactSv _contactSv;

        public ContactsController(DbShsmsContext context)
        {
            _context = context;
            _contactSv = new ContactSvImpl(_context);
        }

        // GET: ContactsController
        public async Task<IActionResult> Index(int campus)
        {
            var contacts = _contactSv.GetContacts();
            if (campus == 0)
            {
                contacts = _contactSv.GetContacts();
            }
            else
            {
                contacts = _contactSv.SearchContactList(campus);
            }
            //ViewData["CampusId"] = new SelectList(_context.Campuses.Select(c => new { c.CampusId, c.CampusName }).Distinct(), "CampusId", "CampusName");
            return contacts != null ? View(contacts) : Problem("Cant get list contacts!");
        }

        public async Task<IActionResult> SearchContacts(int campus)
        {
            var contacts = _contactSv.SearchContactList(campus);
            //ViewData["CampusId"] = new SelectList(_context.Campuses, "CampusId", "CampusName");
            //return contacts != null ? View(contacts) : Problem("Cant get list contacts!");
            return RedirectToAction("Index", new { campus = campus });
        }
        //public async Task<IActionResult> ContactIndex()
        //{
        //    var myData = _contactSv.GetContactList();
        //    return View("Contact.cshtml", myData);
        //}

        // GET: ContactsController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = _contactSv.GetContactById(id.Value);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: ContactsController/Create
        public IActionResult Create()
        {
            ViewData["CampusId"] = new SelectList(_context.Campuses, "CampusId", "CampusName");
            return View();
        }

        // POST: ContactsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _contactSv.SaveContact(contact);
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        // GET: ContactsController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = _contactSv.GetContactById(id.Value);
            if (contact == null)
            {
                return NotFound();
            }
            ViewData["CampusId"] = new SelectList(_context.Campuses, "CampusId", "CampusName");
            return View(contact);
        }

        // POST: ContactsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contact contact)
        {
            if (id != contact.ContactId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (_contactSv.UpdateContact(id, contact))
                    return RedirectToAction(nameof(Index));
                return NotFound();
            }
            return View(contact);
        }

        // GET: ContactsController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.Include(c => c.Campus)
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: ContactsController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Contacts == null)
            {
                return Problem("Entity set 'DbShsmsContext.Contacts'  is null.");
            }
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _contactSv.RemoveContact(contact);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
