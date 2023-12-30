namespace AddressBookTest.Testing;
public class BigLetter_Test // Test-klass
{
    [Fact] // Använder mig av xUnit
    public void ShouldSplitInput_MakeFirstLetterBigForEachWord_ThenReturnBack() // Test-metod, här testar vi om BigLetter Metoden funkar som jag vill att den ska 
    {
        // Arrange
        string input = "iris mistelska strandqvist"; // Exempel på vad som kan matas in i applikationen
        string expected = "Iris Mistelska Strandqvist"; // Så jag vill att det ska sparas och skrivas ut senare i applikationen

        // Act
        string  actualResult= BigLetter(input); // Här testas metoden, blir det korrekt?

        // Assert
        Assert.Equal(expected, actualResult); // Här jämförs vad metoden returnerad och hur jag ville ha det, och det gick!

        static string BigLetter(string input) // METOD SOM GÖR SÅ ATT FÖRSTA BOKSTAVEN BLIR STOR I VARJE ORD
        {
          return string.Join(" ", input.Split(" ")
             .Select(word => char.ToUpper(word[0]) + word.Substring(1)));
        }
    }
}
