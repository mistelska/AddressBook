using AddressBookTest.Enums;
using AddressBookTest.Interfaces;
namespace AddressBookTest.Models.Responses;

public class ServiceResult : IServiceResult
{
    public ServiceStatus Status { get; set; }
    public object Result { get; set; } = null!;
}
