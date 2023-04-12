using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Services.SHSMS_Services.ContactServices
{
    public interface IContactSv
    {
        IEnumerable<Contact> GetContacts();
        IEnumerable<Contact> GetContactList();
        IEnumerable<Contact> SearchContactList(int campus);

        Contact GetContactById(int id);
        bool SaveContact(Contact contact);
        bool RemoveContact(Contact contact);
        bool UpdateContact(int id, Contact contact);
        bool IsContactExist(int id);
    }
}
