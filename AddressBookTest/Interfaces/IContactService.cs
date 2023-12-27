using AddressBookTest.Models;
using AddressBookTest.Models.Responses;

namespace AddressBookTest.Interfaces;

// CRUD CREATE READ UPDATE DELETE (??)
public interface IContactService
{
    IServiceResult AddContactToList(IContact contact);
    IServiceResult GetOneContactFromList(string fullName);
    IServiceResult GetAllContactsFromList();
    IServiceResult DeleteContactFromEmail(string email);
}
