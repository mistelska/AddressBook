namespace AddressBookTest.Models;

public class Contact // En klass för alla kontakter, här är grunden i vad som behövs matas in av användaren
{
    // KONTAKT INFO - PERSON
    public string FirstName { get; set; } = null!; // Använder mig av { get; set; }, get; hämtar värdet användaren matar in, set; sätter värdet användaren matar in
    public string LastName { get; set; } = null!;
    public string? PhoneNumber { get; set; } = null!; // Ett frågetecken på PhoneNumber då den inte är ett krav på att vara ifylld
    public string Email { get; set; } = null!;

    // KONTAKT INFO - ADDRESS
    public string City { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
}
