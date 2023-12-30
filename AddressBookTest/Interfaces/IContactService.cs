using AddressBookTest.Models;
namespace AddressBookTest.Interfaces;
public interface IContactService //Interface för ContactService med 4 st metoder
{
    IServiceResult AddContactToList(Contact contact);
    IServiceResult GetOneContactFromList(string email);
    IEnumerable<Contact> GetAllContactsFromList();
    IServiceResult DeleteContactFromEmail(string email);
}
