using AddressBookTest.Interfaces;
using System.Diagnostics;
namespace AddressBookTest.Services;
public class DataService(string dataFileName) : IDataService
{
  private readonly string _dataFileName = dataFileName; // Ett privat fält med namnet _dataFileName, den gör så att jag kan spara ner data och hämta data 

    public string GetDataFromFile() // Hämta det som ligger i filen
    {
        try
        {
           if (File.Exists(_dataFileName)) // Om det finns något i filen kommer det hämtas upp och skriva ut
            {
               using (var sr = new StreamReader(_dataFileName))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); } // Finns det inget i filen returneras felmedellande
        return null!;
    }

    public bool SaveDataToFile(string data) // Spara ner det som skrivits in av användaren i en fil
    {
        try // Här testar den om det går att spara filen, går det så sparas info
        {
            using (var sw = new StreamWriter(_dataFileName)) // Using används så att sw bara finns levande i detta scopet, data skickas till en fil
            {
                sw.WriteLine(data);
            }
            return true;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); } // Om den inte lyckas spara in filen så returneras falskt värde
        return false;
    }
}
