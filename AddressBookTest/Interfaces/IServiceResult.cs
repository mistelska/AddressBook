using AddressBookTest.Enums;
namespace AddressBookTest.Interfaces
{
    public interface IServiceResult // Interface för ServiceResult
    {
        object Result { get; set; }
        ServiceStatus Status { get; set; }
    }
}