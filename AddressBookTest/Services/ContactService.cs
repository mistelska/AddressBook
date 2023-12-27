using AddressBookTest.Interfaces;
using AddressBookTest.Models;
using AddressBookTest.Models.Responses;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace AddressBookTest.Services;

public class ContactService : IContactService
{
    private static readonly List<IContact> _contacts = [];

    public IServiceResult AddContactToList(IContact contact)
    {
        IServiceResult response = new ServiceResult();

        try
        {
            if (!_contacts.Any(x => x.Email == contact.Email)) 
            {
                _contacts.Add(contact);
                response.Status = Enums.ServiceStatus.SUCCESSFUL;
            }
            else
            {
                response.Status = Enums.ServiceStatus.ALREADY_EXISTS;
            }
        }
        catch (Exception ex) 
        { 
            Debug.WriteLine(ex.Message);
            response.Status = Enums.ServiceStatus.FAILURE;
            response.Result = ex.Message;
        }
        return response;
    }

    public IServiceResult DeleteContactFromEmail(string email)
    {
        IServiceResult response = new ServiceResult();
       
        try
        {
            var contactToDelete = _contacts.FirstOrDefault(x => x.Email == email);
            if (contactToDelete != null)
            {
                _contacts.Remove(contactToDelete);
                response.Status = Enums.ServiceStatus.SUCCESSFUL;
                response.Result = _contacts;
            }
            else
            {
                response.Status = Enums.ServiceStatus.NOT_FOUND;
                response.Result = "\tThe contact was not found.";
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            response.Status = Enums.ServiceStatus.FAILURE;
            response.Result = ex.Message;
        }
        return response;
    }

    public IServiceResult GetAllContactsFromList()
    {
        IServiceResult response = new ServiceResult();

        try
        {
            response.Status = Enums.ServiceStatus.SUCCESSFUL;
            response.Result = _contacts;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            response.Status = Enums.ServiceStatus.FAILURE;
            response.Result = ex.Message;
        }
        return response;
    }

    public IServiceResult GetOneContactFromList(string fullName)
    {
        IServiceResult response = new ServiceResult();
        try
        {
            var contactDetailsToShow = _contacts.FirstOrDefault(x => x.FullName == fullName);

            if (contactDetailsToShow != null)
            {
                response.Status = Enums.ServiceStatus.SUCCESSFUL;
                response.Result = contactDetailsToShow;
            }
            else
            {
                response.Status = Enums.ServiceStatus.NOT_FOUND;
                response.Result = "\tThe contact was not found.";
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            response.Status = Enums.ServiceStatus.FAILURE;
            response.Result = ex.Message;
        }
        return response;
    }
}
