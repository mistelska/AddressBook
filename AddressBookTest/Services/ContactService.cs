using AddressBookTest.Interfaces;
using AddressBookTest.Models;
using AddressBookTest.Models.Responses;
using System.Diagnostics;
using System.Text.Json;
namespace AddressBookTest.Services;

public class ContactService : IContactService
{
    private List<Contact> _contacts = []; // Skapar en lista i Contact klassen som jag döpte till _contacts
    private readonly DataService _dataService = new DataService(@"C:\all-courses-ec\c-sharp-projects\DataForAddressBook\allContacts.json"); // Var jag vill filen ska sparas 
   
    public IServiceResult AddContactToList(Contact contact) // EN METOD SOM LÄGGER TILL EN KONTAKT TILL LISTAN
    {
        IServiceResult response = new ServiceResult(); // Skapar ny instans av ServiceResult så vi sen kan returnera "response"

        try
        {
            if (!_contacts.Any(x => x.Email == contact.Email))  // Kollar om det finns en kontakt i listan med samma epost som användaren skrev in
            {
                _contacts.Add(contact); // Lägger till kontakten i listan
                string jsonData = JsonSerializer.Serialize(_contacts); // Omvandlar hela listan av kontakter till JSON
                _dataService.SaveDataToFile(jsonData); // Sparar json-datan till filen 
                response.Status = Enums.ServiceStatus.SUCCESSFUL; 
            }
            else
            {
                response.Status = Enums.ServiceStatus.ALREADY_EXISTS; // Om användaren redan finns
            }
        }
        catch (Exception ex)  // Om något går fel går den in här
        { 
            Debug.WriteLine(ex.Message); // Meddelandet kommer visas endast när man kör applikationen i ett visst felsökningsläge
            response.Status = Enums.ServiceStatus.FAILURE;
            response.Result = ex.Message;
        }
        return response; // Returnerar om det blev godkänt, icke godkänt eller already exists
    }

    public IServiceResult DeleteContactFromEmail(string email) // EN METOD SOM TAR BORT EN KONTAKT GENOM EPOST
    {
        IServiceResult response = new ServiceResult();
       
        try
        {
            var contactToDelete = _contacts.FirstOrDefault(x => x.Email == email); // Kollar igenom listan ifall eposten som användaren skrev in är samma som någon i listan
            if (contactToDelete != null)  // Om användarens inmatning inte är null tar den bort kontakten
            {
                _contacts.Remove(contactToDelete); // Tar bort kontakten 
                UpdateJsonFile(); // Metod som uppdaterar Json-Filen
                response.Status = Enums.ServiceStatus.SUCCESSFUL;
                response.Result = _contacts;
            }
            else
            {
                response.Status = Enums.ServiceStatus.NOT_FOUND; 
                response.Result = "\tThe contact was not found.";
            }

        }
        catch (Exception ex) // Det gick inte
        {
            Debug.WriteLine(ex.Message);
            response.Status = Enums.ServiceStatus.FAILURE;
            response.Result = ex.Message;
        }
        return response;
    }
    private void UpdateJsonFile() // Privat metod som uppdaterar JSON-filen 
    {
        try
        {
            string jsonData = JsonSerializer.Serialize(_contacts); // Konverterar listans innehåll till en JSON-sträng
            _dataService.SaveDataToFile(jsonData); // Här sparas sen det genom Metoden -SaveDataToFile
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    public IEnumerable<Contact> GetAllContactsFromList() // Metod som hämtar ut alla kontakter från listan och returnerar innehållet, IEnumerable då den inte ska kunna ändras av användaren och bara vara läsbar
    {
        try
        {
            var data = _dataService.GetDataFromFile();
            if (!string.IsNullOrEmpty(data)) // Kollar om listan är null eller tom
            {
                _contacts = JsonSerializer.Deserialize<List<Contact>>(data)!; // Deserialisera JSON-datan till en lista av kontakter
            }
            else
            {
                _contacts = new List<Contact>(); // Om något går fel i deserialiseringen av datan så skapas en tom lista av kontakter
            }
            string jsonData = JsonSerializer.Serialize(_contacts);
            _dataService.SaveDataToFile(jsonData);
        }
        catch (Exception ex) // Uppstår det något fel åker den in i catchen
        {
            Debug.WriteLine(ex.Message);
        }
        return _contacts;
    }

    public IServiceResult GetOneContactFromList(string email) // Metod där vi får ut mer info om en specifik kontakt
    {
        IServiceResult response = new ServiceResult();
        try
        {
            var contactDetailsToShow = _contacts.FirstOrDefault(x => x.Email == email); // Letar en likadan epost som matades in av användaren

            if (contactDetailsToShow != null) // Användar-inmatningen är inte null = gå vidare
            {
                response.Status = Enums.ServiceStatus.SUCCESSFUL;
                response.Result = contactDetailsToShow; // Visar upp kontakt info
            }
            else // Kontakten hittades inte
            {
                response.Status = Enums.ServiceStatus.NOT_FOUND; 
                response.Result = "\tThe contact was not found.";
            }
        }
        catch (Exception ex) // Något annat gick fel
        {
            Debug.WriteLine(ex.Message);
            response.Status = Enums.ServiceStatus.FAILURE;
            response.Result = ex.Message;
        }
        return response;
    }
}
