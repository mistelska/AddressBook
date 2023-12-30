namespace AddressBookTest.Interfaces;
public interface IDataService // Interface för DataService
{
    string GetDataFromFile();
    bool SaveDataToFile(string data);
}