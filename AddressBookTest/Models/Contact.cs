using AddressBookTest.Interfaces;
namespace AddressBookTest.Models;

public class Contact : IContact
{
    // CONTACT INFO - PERSON
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    // Gör en FullName så att användaren kan söka på hela namnet, här set;s namnet in i FullName
    public string FullName => $"{FirstName} {LastName}";
    // string? skriver jag eftersom telefonnummer är inte ett krav att fylla i
    public string? PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;

    // CONTACT INFO - ADDRESS
    public string City { get; set; } = null!;
    public string Street { get; set; } = null!;
    public int PostalCode { get; set; } 
}
