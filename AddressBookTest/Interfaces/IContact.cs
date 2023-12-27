namespace AddressBookTest.Interfaces;

public interface IContact
{
    string City { get; set; }
    string Email { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
    string FullName { get; } // Skriver ingen set; här då den sätts i Contact klassen
    string? PhoneNumber { get; set; }
    int PostalCode { get; set; }
    string Street { get; set; }
}