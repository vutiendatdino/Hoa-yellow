using SP23_G21_SHSMS.Models.SHSMS;
using Microsoft.EntityFrameworkCore;

namespace SP23_G21_SHSMS.Repositories.SHSMS_Repository.ContactRepo
{
    public class ContactRepoImpl : IContactRepo
    {
        private DbShsmsContext _context;
        public ContactRepoImpl(DbShsmsContext context)
        {
            _context = context;
        }
        public IEnumerable<Contact> GetContacts()
        {
            //var contactsByCampus = _context.Contacts
            //    .Include(c => c.Campus)
            //    .GroupBy(c => c.Campus.CampusName)
            //    .Select(g => new Contact
            //    {
            //        Campus = new Campus { CampusName = g.Key },
            //        Phone = string.Join(", ", g.Select(c => c.Phone))
            //    }).ToList();
            //return contactsByCampus;
            return _context.Contacts.Include(c => c.Campus).ToList();
        }

        public IEnumerable<Contact> GetContactList()
        {
            return _context.Contacts.Include(m => m.CampusId).Include(m => m.Phone).ToList();

        }

        //public Contact GetContactById(int id)
        //{
        //    Contact contact = _context.Contacts.Find(id);
        //    contact.Phone = contact.Phone.Trim();
        //    return contact;
        //}

        public Contact GetContactById(int id)
        {
            Contact contact = _context.Contacts
                .Include(c => c.Campus)
                .FirstOrDefault(c => c.ContactId == id);

            contact.Phone = contact.Phone.Trim();

            return contact;
        }

        public bool IsContactExist(int id)
        {
            return _context.Contacts.Any(s => s.ContactId == id);
        }

        public void SaveContact(Contact contact)
        {
            _context.Contacts.Add(contact);
            _context.SaveChanges();
        }

        public void RemoveContact(Contact contact)
        {
            var symptomTmp = _context.Contacts.SingleOrDefault(s => s.ContactId == contact.ContactId);
            _context.Contacts.Remove(symptomTmp!);
            _context.SaveChanges();
        }

        public void UpdateContact(Contact contact)
        {
            _context.Entry<Contact>(contact).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }

        public IEnumerable<Contact> SearchContactList(int campus)
        {
            return _context.Contacts.Include(c => c.Campus).Where(c => c.CampusId == campus).ToList();
        }
    }
}
