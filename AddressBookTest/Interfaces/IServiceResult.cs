using AddressBookTest.Enums;

namespace AddressBookTest.Interfaces
{
    public interface IServiceResult
    {
        object Result { get; set; }
        ServiceStatus Status { get; set; }
    }
}