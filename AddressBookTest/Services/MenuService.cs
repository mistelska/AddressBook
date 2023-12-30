using AddressBookTest.Interfaces;
using AddressBookTest.Models;
namespace AddressBookTest.Services;

public interface IMenuService
{
    void ShowMenu();
}
public class MenuService : IMenuService
{
    private readonly IContactService _contactService = new ContactService();  // Skapar en instans som bara kan sättas med information i en gång och går inte att ändras sen 

    public void ShowMenu()
    {
        bool isGoing = true; // Sålänge bool:en är sann så kommer loopen att fortsätta

        while (isGoing)
        {
            // MENYN, \n\t är där då det blir en snyggare och en mer läsbar meny

            ClearTitleSpace("ADDRESS BOOK");
            Console.WriteLine("\t\t[1] Add New Contact");
            Console.WriteLine("\t\t[2] Show Contact Details");
            Console.WriteLine("\t\t[3] Show All Contacts");
            Console.WriteLine("\t\t[4] Delete Contact");
            Console.WriteLine("\t\t[5] Quit");
            Console.Write("\n\t\tMenu Option: ");

            string userInput = Console.ReadLine()!; // Läser in vad användaren skriver in
            if (int.TryParse(userInput, out int option)) // Försöker konventera inmatning till ett heltal, skickar sen vidare till option
            {
                switch (option) // Är inmatningen något av 1-5 så går den in dem olika case:en
                {
                    case 1:
                        AddContact();
                        break;

                    case 2:
                        ShowContactDetails();
                        break;

                    case 3:
                        ShowAllContacts();
                        break;

                    case 4:
                        DeleteContact();
                        break;

                    case 5: // Här avslutas programmet, först ett meddelande, sen avslutas loopen genom att sätta isGoing till ett falskt värde
                        ClearTitleSpace("You are now exiting this program");
                        isGoing = false;
                        break;

                    default:  // Har användaren inte matat in en siffra mellan 1-5, kommer detta meddelande upp
                        ClearTitleSpace("Please enter a number between 1-5");
                        break;
                }
            }
            else // Har användaren inte matat in en siffra mellan 1-5, kommer detta meddelande upp
            {
                ClearTitleSpace("Please enter a number between 1-5");
            }
            Console.ReadKey();
        }
    }
    public void AddContact() // HÄR ÄR METODEN FÖR ATT VISA MENYN DÄR EN KONTAKT SKA LÄGGAS TILL
    {
        Contact contact = new Contact();

        ClearTitleSpace("ADD NEW CONTACT");
        contact.FirstName = InputContactInfoAndCannotBeEmpty("First Name"); // Trimmar om användaren har råkat göra ett mellanslag antingen innan eller efter 
        contact.FirstName = BigLetter(contact.FirstName);
        contact.LastName = InputContactInfoAndCannotBeEmpty("Last Name");
        contact.LastName = BigLetter(contact.LastName);

        while (true) // Här testar vi så eposten är gokänd ex. a@a.a
        {
            contact.Email = InputContactInfoAndCannotBeEmpty("Email");
            if (!ValidEmailOrInvalidEmail(contact.Email))
            {
                ClearTitleSpace($"This input <{contact.Email}> is not a valid email"); 
            }
            else
                break;
        }

        Console.Write("\t\tPhone Number: ");
        contact.PhoneNumber = Console.ReadLine() ?? "".Trim(); // Skriver frågetecken då det är okej att telefonnumret inte är ifyllt
        contact.City = InputContactInfoAndCannotBeEmpty("City");
        contact.City = BigLetter(contact.City);
        contact.Street = InputContactInfoAndCannotBeEmpty("Street Name");
        contact.Street = BigLetter(contact.Street);
        contact.PostalCode = InputContactInfoAndCannotBeEmpty("Postal Code");

        var result = _contactService.AddContactToList(contact); 

        switch (result.Status) 
        {
            case Enums.ServiceStatus.SUCCESSFUL: // Kontakten lyckades läggas till
                ClearTitleSpace("The contact was added");
                break;

            case Enums.ServiceStatus.ALREADY_EXISTS: // Kontakten finns redan (eposten finns redan)
                ClearTitleSpace("The contact already exits");
                break;

            case Enums.ServiceStatus.FAILURE: // Något annat gick fel och kontakten las inte till i listan
                ClearTitleSpace("The contact was not added");
                break;
        }
    }

    public void DeleteContact() // HÄR ÄR METODEN FÖR MENYN SOM SKA VISAS NÄR EN KONTAKT SKA RADERAS GENOM EPOST
    {
        bool isGoing = true;
        while(isGoing) // Sålänge isGoing är sann fortsätter loopen
        {
            ClearTitleSpace("DELETE A CONTACT BY EMAIL");
            Console.WriteLine();
            Console.Write("\t\tType the person´s email here: ");
            string contactEmailToDelete = Console.ReadLine() ?? ""; // Läser in inmatningen

            var result = _contactService.DeleteContactFromEmail(contactEmailToDelete); // Åker in i DeleteContactFromEmail med inmatningen och beroende på om det lyckas eller inte får vi sen ett meddelande tillbaka

            switch (result.Status)
            {
                case Enums.ServiceStatus.SUCCESSFUL: // Om kontakten lyckades tas bort kommer detta meddelande upp
                    ClearTitleSpace("The Contact is Deleted");
                    isGoing = false;
                    break;

                case Enums.ServiceStatus.NOT_FOUND: // Om kontakten inte hittades får man testa igen
                    ClearTitleSpace("Contact Not Found");
                    Console.Write("\t\tTry Again? y/n: ");

                    var answer = Console.ReadLine()?.ToLower().Trim();

                    if (answer == "y") // Spelar ingen roll om man skriver y eller Y, inmatningen blir allt till små bokstäver
                        continue;
                    else
                        ClearTitleSpace("Press Enter to go back to Main Menu");
                        isGoing = false;
                    break;
            }
        }
    }

    private void ShowAllContacts() // HÄR ÄR METODEN FÖR MENYN SOM SKA VISA ALLA KONTAKTER SOM FINNS I LISTAN
    {
        ClearTitleSpace("All Contacts");
        var response = _contactService.GetAllContactsFromList(); // Åker in i GetALlContactsFromList, ifall kontakter finns visas dem, ifall kontakter inte finns kommer meddelande upp

        if(response is List<Contact> contactList) // ??????
        {
            if(!contactList.Any()) // Ifall det INTE finns något i listan 
            {
                ClearTitleSpace("The list of contacts is empty");
            }
            else
            {
                int i = 1; // Börjar listan på 1, så att det visas på ett snyggt och prydligt sett
                foreach (var contact in contactList) // Loopen avslutas när det inte finns några fler kontakter i listan 
                {
                    Console.WriteLine($"\t{i}. NAME: {contact.FirstName} {contact.LastName}  EMAIL: <{contact.Email}>\n"); // Visar upp varje kontakt på en ny rad
                    i++;
                }
                DesignLine();
            }
        }
    }

    private void ShowContactDetails() // HÄR ÄR METODEN FÖR MENYN SOM VISAS DÅ MAN VILL SE MER INFO OM EN SPECIFIK KONTAKT
    {
        bool isGoing = true;
        while (isGoing)
        {
            ClearTitleSpace("DISPLAY CONTACT DETAILS");
            Console.WriteLine();
            Console.Write("\t\tType the person´s email here: ");
            string inputEmail = Console.ReadLine() ?? "";

            var result = _contactService.GetOneContactFromList(inputEmail);

            switch (result.Status)
            {
                case Enums.ServiceStatus.SUCCESSFUL: // Allt gick bra och här visas sen kontakt-informationen upp
                    var contact = result.Result as Contact;
                    if (contact != null) // Ifall contact INTE är likamed null fortsätter vi
                    {
                        ClearTitleSpace($"Contact Information for {contact.FirstName} {contact.LastName}");
                        Console.WriteLine($"\t\tEMAIL: {contact.Email}");
                        if (!string.IsNullOrEmpty(contact.PhoneNumber))
                            Console.WriteLine($"\t\tPHONE NUMBER: {contact.PhoneNumber}");
                        Console.WriteLine($"\t\tCITY: {contact.City}");
                        Console.WriteLine($"\t\tSTREET NAME: {contact.Street}");
                        Console.WriteLine($"\t\tPOSTAL CODE: {contact.PostalCode}");
                        DesignLine();
                        return;
                    }
                    break;

                case Enums.ServiceStatus.NOT_FOUND: // Ifall kontakt inte hittas får man testa igen genom att skriva "y"
                    ClearTitleSpace("Contact Not Found");
                    Console.Write("\t\tTry Again? y/n: ");
                    var answer = Console.ReadLine()?.Trim().ToLower();
                    if (answer == "y")
                        continue;
                    else
                        ClearTitleSpace("Press Enter to go back to Main Menu");
                        isGoing = false;
                        break;
            }
        }
    }
    public bool ValidEmailOrInvalidEmail(string email) // HÄR ÄR METODEN FÖR EN ENKLARE EPOST-VALIDERING
    {
        try
        {
            var mail = new System.Net.Mail.MailAddress(email); // Exempel: a@a.a
            return mail.Address == email;
        }
        catch
        {
            return false;
        }
    }
    public void ClearTitleSpace(string title) // HÄR ÄR METODEN FÖR EN SNYGGARE TITEL OCH TEXT
    {
        Console.Clear();
        Console.WriteLine(new string('.', Console.WindowWidth)); // Punkterna kommer ta upp hela konsol rutans bredd
        Console.Write($"\n\t\t|{title}|\n");
        Console.WriteLine();
    }
    public void DesignLine() // HÄR ÄR METODEN FÖR EN DESIGN LINJE SÅ JAG INTE BEHÖVER SKRIVA OM DEN FLERA GÅNGER
    {
        Console.WriteLine(new string('.',Console.WindowWidth));
    }
    static string BigLetter(string input) // METOD SOM GÖR SÅ ATT FÖRSTA BOKSTAVEN I VARJE ORD BLIR STOR
    {
        return string.Join(" ", input.Split(" ")
            .Select(word => char.ToUpper(word[0]) + word.Substring(1)));
    }
    string InputContactInfoAndCannotBeEmpty(string title) // METOD SOM TAR IN INPUT FRÅN ANVÄNDAREN OCH ÄVEN KOLLAR SÅ DET INTE ÄR TOMT
    {
        string? input;
        do
        {
            Console.Write($"\t\t{title}: "); // Här ändrar man vad det ska stå ex. First Name
            input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input)) // Ifall användaren inte skriver inte något kommer meddelande upp
            {
                ClearTitleSpace("You need to write something. Try again");
            }
        } while (string.IsNullOrWhiteSpace(input)); //Fortsätter enda tills inmatningen inte är tom eller null
        return input; 
    }
}


