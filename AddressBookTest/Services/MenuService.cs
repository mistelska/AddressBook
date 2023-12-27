
using AddressBookTest.Interfaces;
using AddressBookTest.Models;
using System.Numerics;

namespace AddressBookTest.Services;

public interface IMenuService
{
    void ShowMenu();
}
public class MenuService : IMenuService
{
    private readonly IContactService _contactService = new ContactService();

    public void ShowMenu()
    {
        bool isGoing = true;

        while (isGoing)
        {
            ClearTitleSpace("ADDRESS BOOK");
            Console.WriteLine("\t[1] Add New Contact");
            Console.WriteLine("\t[2] Show Contact Details");
            Console.WriteLine("\t[3] Show All Contacts");
            Console.WriteLine("\t[4] Delete Contact");
            Console.WriteLine("\t[5] Quit");
            Console.WriteLine("");
            Console.Write("\tEnter Menu Option Here: ");
            string userInput = Console.ReadLine()!;
            if (int.TryParse(userInput, out int option))
            {
                switch (option)
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

                    case 5:
                        Console.Clear();
                        Console.WriteLine("\tYou are now exiting this program.");
                        isGoing = false;
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("\tPlease enter a number between 1-5, press any key to contiune");
                        break;
                }
            }
            else
            {
                ClearTitleSpace("Please enter a number between 1-5, press any key to continue");
            }
            Console.ReadKey();
        }
    }
    private void AddContact()
    {
        IContact contact = new Contact();

        ClearTitleSpace("ADD NEW CONTACT");
        contact.FirstName = GetAndCheckInputString("\tFirst Name: ");
        contact.LastName = GetAndCheckInputString("\tLast Name: ");
        while (true)
        {
            Console.Write("\tEmail: ");
            contact.Email = Console.ReadLine()!.Trim();
            if (!ValidEmailOrInvalidEmail(contact.Email))
            {
                Console.Clear();
                Console.WriteLine($"\t'{contact.Email}' is not a valid email.");
            }
            else
                break;
        }
        Console.Write("\tPhone Number: ");
        contact.PhoneNumber = Console.ReadLine() ?? "".Trim();
        contact.City = GetAndCheckInputString("\tCity: ");
        contact.Street = GetAndCheckInputString("\tStreet: ");
        contact.PostalCode = GetAndCheckInputInt("\tPostal Code: ");

        var result = _contactService.AddContactToList(contact);

        switch (result.Status)
        {
            case Enums.ServiceStatus.SUCCESSFUL:
                Console.WriteLine("\tThe contact was added.");
                break;

            case Enums.ServiceStatus.ALREADY_EXISTS:
                Console.WriteLine("\tThe contact already exits.");
                break;

            case Enums.ServiceStatus.FAILURE:
                Console.WriteLine("\tThe contact was not added.");
                break;
        }
    }

    public void DeleteContact()
    {
        bool isGoing = true;
        while(isGoing)
        {
            ClearTitleSpace("DELETE A CONTACT BY EMAIL");
            Console.WriteLine();
            Console.Write("\tType the person´s email here: ");
            string contactEmailToDelete = Console.ReadLine() ?? "";
            var result = _contactService.DeleteContactFromEmail(contactEmailToDelete);
            switch (result.Status)
            {
                case Enums.ServiceStatus.SUCCESSFUL:
                    ClearTitleSpace("The Contact is Deleted");
                    isGoing = false;
                    break;

                case Enums.ServiceStatus.NOT_FOUND:
                    Console.Clear();
                    ClearTitleSpace("Contact Not Found.");
                    Console.WriteLine("\tTry Again? Type 'yes'");
                    var answer = Console.ReadLine();
                    if (string.Equals(answer?.ToLower(),"yes"))
                        break;
                    else
                    {
                        ClearTitleSpace("You Are Now Going Back To Main Menu");
                        isGoing = false;
                    }
                    break;
            }
        }
    }

    private void ShowAllContacts()
    {
        ClearTitleSpace("All Contacts");
        var response = _contactService.GetAllContactsFromList();

        if(response.Result is List<IContact> contactList)
        {
            if(!contactList.Any())
            {
                ClearTitleSpace("The list of contacts is empty");
            }
            else
            {
                int i = 1;
                foreach (var contact in contactList)
                {
                    Console.WriteLine($"\n\t{i}. Name: {contact.FullName}, Email: {contact.Email}");
                    i++;
                }
            }
        }
    }

    private void ShowContactDetails()
    {
        while (true)
        {
            ClearTitleSpace("SHOW CONTACT DETAILS");
            Console.WriteLine();
            Console.Write("\tType the person´s full name here: ");
            string contactFullName = Console.ReadLine() ?? "";
            var result = _contactService.GetOneContactFromList(contactFullName);
            switch (result.Status)
            {
                case Enums.ServiceStatus.SUCCESSFUL:
                    var contact = result.Result as IContact;
                    if (contact != null)
                    {
                        ClearTitleSpace($"Contact Info for {contactFullName}");
                        Console.WriteLine($"\t\tEmail: {contact.Email}");
                        if (!string.IsNullOrEmpty(contact.PhoneNumber))
                            Console.WriteLine($"\t\tPhone Number: {contact.PhoneNumber}");
                        Console.WriteLine("\n\t\t~~~~ADDRESS~~~~");
                        Console.WriteLine($"\t\tCity: {contact.City}");
                        Console.WriteLine($"\t\tStreet: {contact.Street}");
                        Console.WriteLine($"\t\tPostal Code: {contact.PostalCode}");
                        return;
                    }
                break;

                case Enums.ServiceStatus.NOT_FOUND:
                    ClearTitleSpace("Contact Not Found");
                    Console.Write("\tTry Again? Type 'yes': ");
                    var answer = Console.ReadLine();
                    if (string.Equals(answer?.ToLower(), "yes"))
                        break;
                    else
                    {
                        ClearTitleSpace("You Are Now Going Back To Main Menu");
                        return;
                    }
            }
        
        }
    }

    public string GetAndCheckInputString (string title)
    {
        while (true)
        {
            Console.Write(title);
            string input = Console.ReadLine()!.Trim();
            if (input.All(char.IsLetter) && !string.IsNullOrEmpty(input))
            {
                return input;
            }
            else
            {
                ClearTitleSpace("Invalid Input, Please try Again");
            }
        }
    }
    public int GetAndCheckInputInt(string title)
    {
        while (true)
        {
            Console.Write(title);
            string inputString = Console.ReadLine()!.Trim();
            if (int.TryParse(inputString, out int input) && !string.IsNullOrEmpty(inputString))
            {
                return input;
            }
            else
            {
                ClearTitleSpace("Invalid Input, Please try Again");
            }
        }
    }
    public bool ValidEmailOrInvalidEmail(string email)
    {
        try
        {
            var mail = new System.Net.Mail.MailAddress(email);
            return mail.Address == email;
        }
        catch
        {
            return false;
        }
    }
    public void ClearTitleSpace(string title)
    {
        Console.Clear();
        Console.WriteLine($"\t~~~~|{title}|~~~~");
        Console.WriteLine();
    }
}


