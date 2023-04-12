using SP23_G21_SHSMS.Models.SHSMS;
using SP23_G21_SHSMS.Repositories.SHSMS_Repository.ContactRepo;

namespace SP23_G21_SHSMS.Services.SHSMS_Services.ContactServices
{
    public class ContactSvImpl : IContactSv
    {
        private DbShsmsContext _context;
        private IContactRepo _contactRepo;

        public ContactSvImpl(DbShsmsContext context)
        {
            _context = context;
            _contactRepo = new ContactRepoImpl(_context);
        }

        public Contact GetContactById(int id)
        {
            try
            {
                if (_context.Contacts == null || !IsContactExist(id)) return null;
                return _contactRepo.GetContactById(id);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Contact> GetContacts()
        {
            try
            {
                if (_context.Contacts == null) return null;
                var contacts = _contactRepo.GetContacts();
                if (contacts is null || contacts.Count() == 0)
                    return null;
                return contacts;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Contact> GetContactList()
        {
            try
            {
                if (_context.Contacts == null) return null;
                var contacts = _contactRepo.GetContactList();
                if (contacts is null || contacts.Count() == 0)
                    return null;
                return contacts;
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<Contact> SearchContactList(int campus)
        {
            try
            {
                if (_context.Contacts == null) return null;
                var contacts = _contactRepo.SearchContactList(campus);
                if (contacts is null || contacts.Count() == 0)
                    return null;
                return contacts;
            }
            catch
            {
                return null;
            }
        }
        public bool IsContactExist(int id)
        {
            try
            {
                return _contactRepo.IsContactExist(id);
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveContact(Contact contact)
        {
            try
            {
                if (_context.Contacts == null || contact is null || !_contactRepo.IsContactExist(contact.ContactId)) return false;
                _contactRepo.RemoveContact(contact);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SaveContact(Contact contact)
        {
            try
            {
                if (_context.Contacts == null || contact is null || string.IsNullOrEmpty(contact.Phone)) return false;
                contact.Phone.Trim();
                _contactRepo.SaveContact(contact);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateContact(int id, Contact contact)
        {
            if (id != contact.ContactId) return false;
            try
            {
                if (_context.Contacts == null || contact is null || !_contactRepo.IsContactExist(contact.ContactId)) return false;
                _contactRepo.UpdateContact(contact);
                return true;
            }
            catch
            {
                return false;
            }
        }

        
    }
}
