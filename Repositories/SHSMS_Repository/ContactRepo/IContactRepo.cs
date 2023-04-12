using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Repositories.SHSMS_Repository.ContactRepo
{
    public interface IContactRepo
    {
		IEnumerable<Contact> GetContacts();
		bool IsContactExist(int id);
		Contact GetContactById(int id);
		IEnumerable<Contact> GetContactList();

		void SaveContact(Contact contact);
		void RemoveContact(Contact contact);
		void UpdateContact(Contact contact);
		IEnumerable<Contact> SearchContactList(int campus);

	}
}
